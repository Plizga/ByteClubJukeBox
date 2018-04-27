using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
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

        public Form1()
        {
            
            InitializeComponent();

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

        private void PlayClip(int index)
        {
            player.currentMedia = player.currentMedia;
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
        }

        private void ShuffleBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShuffleBox.Checked)
                player.settings.setMode("shuffle", true);
            else
                player.settings.setMode("shuffle", false);
        }
    }
}