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

namespace Jukebox
{

    public partial class Form1 : Form
    {
        private WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
        private string url;
        private string fileDirectory = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("\\bin")).Replace("\\", "/") + "/Assets/AudioFiles/";
        private List<string> files = new List<string>();
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

            SongSelector.Items.Clear();
            SongSelector.Items.AddRange(files.ToArray());
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
           if (!currentSong.Equals("") && SongSelector.Text.ToUpper().Equals(player.currentMedia.name.ToUpper()))
            {
                if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    PauseClip();
                    PlayBtn.Text = "Resume";
                }
                else if (player.playState == WMPLib.WMPPlayState.wmppsPaused)
                {
                    ResumeClip();
                    PlayBtn.Text = "Pause";
                }
            }
            else
            {
                currentSong = SongSelector.Text;
                url = fileDirectory + currentSong + ".mp3";
                PlayClip(url);
                PlayBtn.Text = "Pause";
            }
        }

        private void StopBtn_Click(object sender, EventArgs e)
        {
            EndClip();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            url = fileDirectory + SongSelector.Text;
        }

        private void PlayClip(string fileDir)
        {
            player.URL = fileDir;
            player.controls.play();
        }

        private void PauseClip(){
            player.controls.pause();
        }

        private void EndClip()
        {
            player.controls.stop();
        }

        private void SwitchClip(string fileDir)
        {
            EndClip();
            PlayClip(fileDir); 
        }

        private void AddClip(string fileDir)
        {
            WMPLib.IWMPMedia song = player.newMedia(fileDir);
            player.currentPlaylist.appendItem(song);
        }

        private void ResumeClip()
        {
            player.controls.play();
        }
    }
}