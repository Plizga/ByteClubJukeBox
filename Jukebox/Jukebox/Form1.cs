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
        private WindowsMediaPlayer Player = new WindowsMediaPlayer();
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
                Player.currentMedia.sourceURL.IndexOf(SongSelector.Text) != -1 &&
                Player.playState != WMPLib.WMPPlayState.wmppsStopped)
            {
                if (Player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    PauseClip();
                }
                else if (Player.playState == WMPLib.WMPPlayState.wmppsPaused)
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
            Player.currentMedia = Player.currentMedia;
            Player.controls.play();
            CurrentSongNameLbl.Text = currentSong;
            PlayBtn.Text = "Pause";
        }

        private void PauseClip(){
            Player.controls.pause();
            PlayBtn.Text = "Resume";
        }

        private void EndClip()
        {
            Player.controls.stop();
            CurrentSongNameLbl.Text = "";
        }

        private void SwitchClip()
        {
            EndClip();
            PlayClip(SongSelector.SelectedIndex); 
        }

        private void ResumeClip()
        {
            Player.controls.play();
            PlayBtn.Text = "Pause";
        }

        private void SwitchPlaylist(IWMPPlaylist newPlaylist)
        {
            int index = 0;
            for (int i = 0; i < newPlaylist.count; i++)
            {
                if (Player.currentMedia.name.Equals(newPlaylist.Item[i].name))
                {
                    index = i;
                    CurrentSongNameLbl.Text = "found";
                    break;
                }
            }

            double currentTime = Player.controls.currentPosition;
            Player.controls.stop();
            Player.currentPlaylist = newPlaylist;
            Player.currentMedia = Player.currentPlaylist.Item[index];
            Player.controls.play();
            Player.controls.currentPosition = currentTime;
        }

        private void CreatePlaylist()
        {
            myPlaylist = Player.playlistCollection.newPlaylist("myPlaylist");

            foreach (string s in files)
            {
                IWMPMedia song = Player.newMedia(fileDirectory + s + ".mp3");
                myPlaylist.appendItem(song);
            }

            Player.currentPlaylist = myPlaylist;
            Player.controls.stop();
        }

        private void CreateShuffleList()
        {
            shuffledList = Player.playlistCollection.newPlaylist("shuffledList");
            List<string> names = files.ToList<string>();
            Random rand = new Random();

            for (int i = names.Count - 1; i >= 0; i--)
            {
                int holder = rand.Next(0, i);
                IWMPMedia song = Player.newMedia(fileDirectory + names[holder] + ".mp3");
                names.RemoveAt(holder);
                shuffledList.appendItem(song);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Player.playState == WMPPlayState.wmppsMediaEnded && !RepeatBox.Checked)
            {
                Player.controls.next();
                Player.controls.play();
            }
            else if(Player.playState == WMPPlayState.wmppsMediaEnded && RepeatBox.Checked)
            {
                Player.controls.play();
            }
            else if(Player.playState == WMPPlayState.wmppsLast)
            {
                for (int i = Player.currentPlaylist.count; i > 0; i--)
                {
                    Player.controls.previous();
                }
                Player.controls.play();
            }
        }

        private void ShuffleBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShuffleBox.Checked)
                SwitchPlaylist(shuffledList);
            else
                SwitchPlaylist(myPlaylist);
        }
    }
}