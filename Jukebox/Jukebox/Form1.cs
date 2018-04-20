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

        public Form1()
        {
            InitializeComponent();
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            PlayClip(Url);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Url = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("/bin")) + "/assets/audioFiles/" + SongSelector.Text;
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
