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
        private SoundPlayer Player = new SoundPlayer();

        public Form1()
        {
            InitializeComponent();
            //Player.SoundLocation = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("/bin")) + "/resources/" + SongSelector.Text + ".wav";
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            Player.Play(); 
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Player.SoundLocation = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().IndexOf("/bin")) + "/resources/" + SongSelector.Text + ".wav";
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }
    }
}
