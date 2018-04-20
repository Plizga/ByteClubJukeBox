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
        private WMPLib.WindowsMediaPlayer Player = new WMPLib.WindowsMediaPlayer();
        private string Url;
        private string fileDirectory = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("\\bin")) + "/Assets/AudioFiles/";
        private List<string> files = new List<string>();

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
            PlayClip(Url);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Url = fileDirectory + SongSelector.Text;
        }

        private void PlayClip(string url)
        {
            Player.URL = url;
            Player.controls.play();
        }

        private void PauseClip(){
            Player.controls.pause();
        }

        private void EndClip()
        {
            Player.controls.stop();
        }

        private void SwitchClip(string url)
        {
            EndClip();
            PlayClip(url); 
        }

        private void AddClip(string url)
        {
            WMPLib.IWMPMedia song = Player.newMedia(url);
            Player.currentPlaylist.appendItem(song);
        }
    }
}