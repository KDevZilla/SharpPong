using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpPong
{
    public partial class FormSharpPong : Form, IInputController
    {
        public FormSharpPong()
        {
            InitializeComponent();
        }
        Game game = null;
        //PictureBoxDisplay pictureboxdisplay = null;

        public event EventHandler KeyUpPush;
        public event EventHandler KeyDownPush;
        public event EventHandler KeyUpRelease;
        public event EventHandler KeyDownRelease;

        private void button1_Click(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.ReleaseDevice();

            }
            game = new Game(new PictureBoxDisplay(this.pictureBox1),
                this);
            game.Start();
          //  button1.Visible = false;
           // this.Focus();


        }

        private void FormShapPong_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += Form3_KeyDown;
            this.KeyUp += Form3_KeyUp;
            NewGame();
        }

        private void Form3_KeyUp(object sender, KeyEventArgs e)
        {
           // throw new NotImplementedException();
           if(e.KeyCode == Keys.Up)
            {
                this.KeyUpRelease?.Invoke(this, e);
                e.Handled = true;
                return ;
            }
            if (e.KeyCode == Keys.Down )
            {
                this.KeyDownRelease?.Invoke(this, e);
                e.Handled = true;
                return ;
            }
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up)
            {
                this.KeyUpPush?.Invoke(this, e);
                e.Handled = true;
                return;
            }
            if(e.KeyCode == Keys.Down)
            {
                this.KeyDownPush?.Invoke(this, e);
                e.Handled = true;
                return;
            }
            //throw new NotImplementedException();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show ("Do you want to exit game?","", MessageBoxButtons.OKCancel )!= DialogResult.OK)
            {
                return;
            }
            Application.Exit();
        }
        private void NewGame()
        {
            if (game != null)
            {
                game.ReleaseDevice();

            }

            if (this.Controls.Contains(pictureBox1))
            {
                this.Controls.Remove(pictureBox1);
            }
            pictureBox1 = new PictureBox();
            this.pictureBox1.Location = new System.Drawing.Point(0, 30);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(600, 600);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.Controls.Add(pictureBox1);

            game = new Game(new PictureBoxDisplay(this.pictureBox1),
                this);
            game.Start();
            //button1.Visible = false;
            //this.Focus();
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHowtoPlay f = new FormHowtoPlay();
            f.ShowDialog();

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();
            f.ShowDialog();

        }
    }
}
