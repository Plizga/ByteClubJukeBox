using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WMPLib;

namespace Jukebox
{

    public partial class Form1 : Form
    {
        private WindowsMediaPlayer player = new WindowsMediaPlayer();
        private string fileDirectory = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("\\bin")).Replace("\\", "/") + "/Assets/AudioFiles/";
        private List<string> files = new List<string>();
        private IWMPPlaylist shuffledList;
        private IWMPPlaylist myPlaylist;
        private string currentSong = "";
        private Random rng = new Random();


        public Form1()
        {
            InitializeComponent();
            //sets volume and sets the volume percent value.
            volumeBar.Value = volumeBar.Maximum / 2; 
            player.settings.volume = volumeBar.Value;
            lblVolumePercent.Text = (volumeBar.Value.ToString() + "%");


            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Replace("\\", "/");
            }

            files.AddRange(System.IO.Directory.GetFiles(fileDirectory, "*.mp3"));

            for (int i = 0; i < files.Count; i++)
            {
                int start = files[i].IndexOf("/AudioFiles/") + 12;
                files[i] = files[i].Substring(start, (files[i].Length - 4) - start);
            }

            CreatePlaylist();
            CreateShuffleList();

            SongSelector.Items.Clear();
            SongSelector.Items.AddRange(files.ToArray());
            SongSelector.SelectedIndex = 0;
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
           if (!currentSong.Equals("") &&
                player.currentMedia.sourceURL.IndexOf(SongSelector.Text) != -1 &&
                player.playState != WMPLib.WMPPlayState.wmppsStopped)
            {
                if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    PauseClip();
                }
                else if (player.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    ResumeClip();
                }
            }
            else
            {
                currentSong = SongSelector.Text;
                PlayClip(SongSelector.SelectedIndex);
            }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            EndClip();
            PlayBtn.Text = "Play";
        }
        private void nextBtn_Click(object sender, EventArgs e)
        {
            //Next button moves the SongSelector combobox to the next song and reverts back to beginning of list if at the end
            if(ShuffleBox.Checked == false && RepeatBox.Checked == false) //Next w/out shuffle or repeat checked
            {
                if (SongSelector.SelectedIndex == SongSelector.Items.Count -1)
                    SongSelector.SelectedIndex = 0;
                else
                    SongSelector.SelectedIndex = (SongSelector.SelectedIndex + 1);
                currentSong = SongSelector.Text;
                PlayClip(SongSelector.SelectedIndex);
            }
            if(RepeatBox.Checked == true) //Repeat
                PlayClip(SongSelector.SelectedIndex);
            else if(ShuffleBox.Checked == true) //Shuffle
            {
                SongSelector.SelectedIndex = rng.Next(0, SongSelector.Items.Count);
                currentSong = SongSelector.Text;
                PlayClip(SongSelector.SelectedIndex);
            }
        }

        private void PlayClip(int index)
        {
            player.URL = fileDirectory + currentSong + ".mp3";
            player.controls.play();
            CurrentSongNameLbl.Text = player.currentMedia.name;
            PlayBtn.Text = "Pause";
        }

        private void PauseClip(){
            player.controls.pause();
            PlayBtn.Text = "Resume";
        }

        private void EndClip()
        {
            player.controls.stop();
            CurrentSongNameLbl.Text = "";
        }

        private void SwitchClip()
        {
            EndClip();
            PlayClip(SongSelector.SelectedIndex); 
        }

        private void ResumeClip()
        {
            player.controls.play();
            PlayBtn.Text = "Pause";
        }

        private void SwitchPlaylist(IWMPPlaylist newPlaylist)
        {
            int index = 0;
            for (int i = 0; i < newPlaylist.count; i++)
            {
                if (player.currentMedia.name.Equals(newPlaylist.Item[i].name))
                {
                    index = i;
                    CurrentSongNameLbl.Text = "found";
                    break;
                }
            }

            double currentTime = player.controls.currentPosition;
            player.controls.stop();
            player.currentPlaylist = newPlaylist;
            player.currentMedia = player.currentPlaylist.Item[index];
            player.controls.play();
            player.controls.currentPosition = currentTime;
        }

        private void CreatePlaylist()
        {
            myPlaylist = player.playlistCollection.newPlaylist("myPlaylist");

            foreach (string s in files)
            {
                IWMPMedia song = player.newMedia(fileDirectory + s + ".mp3");
                myPlaylist.appendItem(song);
            }

            player.currentPlaylist = myPlaylist;
            player.controls.stop();
        }

        private void CreateShuffleList()
        {
            shuffledList = player.playlistCollection.newPlaylist("shuffledList");
            List<string> names = files.ToList<string>();
            Random rand = new Random();

            for (int i = names.Count - 1; i >= 0; i--)
            {
                int holder = rand.Next(0, i);
                IWMPMedia song = player.newMedia(fileDirectory + names[holder] + ".mp3");
                names.RemoveAt(holder);
                shuffledList.appendItem(song);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (player.playState == WMPPlayState.wmppsMediaEnded)
            {
                if(!RepeatBox.Checked)
                    player.controls.next();

                player.controls.play();
                CurrentSongNameLbl.Text = player.currentMedia.name;
            }

            
            if (progressBar.Maximum != player.currentMedia.duration) //If new song, sets progressbar maximum and mm:ss timestamp.
            {
                progressBar.Maximum = (int)player.currentMedia.duration;
                lblMax.Text = player.currentMedia.durationString;
            }

            progressBar.Value = (int)player.controls.currentPosition;

            //sets the current position mm:ss timestamp.
            if (player.controls.currentPosition == 0)
                lblPosition.Text = ("0");
            else
                lblPosition.Text = player.controls.currentPositionString;
   
        }

        private void ShuffleBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShuffleBox.Checked)
                player.settings.setMode("shuffle", true);
            else
                player.settings.setMode("shuffle", false);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //sets volume for the song using trackbar and changes percentage.
            player.settings.volume = volumeBar.Value;
            lblVolumePercent.Text = (volumeBar.Value.ToString()+"%");
        }

       private void progressBar_Click(object sender, EventArgs e)
        {
            //finds mouse absolute location
            double absoluteMouse = (PointToClient(MousePosition).X - progressBar.Bounds.X);
            //computes factor of the width divided by 100 cast as double
            double factor = progressBar.Width / (double)100;
            //relative relative from absolute divided by factor
            double relative = absoluteMouse / factor;
            //converts the 1-100 value into decimal by dividing by 100 cast as double then plays to reset the song to current location
            player.controls.currentPosition = player.currentMedia.duration * (relative) / ((float)100);
            player.controls.play();
        }
    }
}