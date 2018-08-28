using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using Asteroids.Standard;
using Asteroids.Standard.Enums;
using Keys = System.Windows.Forms.Keys;

namespace Asteroids
{
    public class frmAsteroids : Form
    {
        private GameController _controller;
        private WinForms.Classes.GraphicPictureBox frame1;
        private WinForms.Classes.GraphicPictureBox frame2;

        public frmAsteroids()
        {
            InitializeComponent();

            _controller = new GameController(frame1, frame2, PlaySound);
        }

        private void PlaySound(Stream stream)
        {
            var player = new SoundPlayer(stream);
            player.Play();
        }

        private void frmAsteroids_Closed(object sender, EventArgs e)
        {
            _controller.ExitGame();
        }

        private void frmAsteroids_Resize(object sender, EventArgs e)
        {
            var rec = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            _controller.ResizeGame(rec);
        }

        private void frmAsteroids_Activated(object sender, EventArgs e)
        {
            if (_controller.GameStatus != Modes.Prep)
                return;

            var rec = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            _controller.Initialize(rec);
        }

        private void frmAsteroids_KeyDown(object sender, KeyEventArgs e)
        {
            Standard.Enums.Keys key;
            switch (e.KeyData)
            {
                case Keys.Escape:
                    // Escape during a title screen exits the game
                    if (_controller.GameStatus == Modes.Title)
                    {
                        _controller.ExitGame();
                        Application.Exit();
                        return;
                    }

                    key = Standard.Enums.Keys.Escape;
                    break;

                case Keys.Left:
                    key = Standard.Enums.Keys.Left;
                    break;

                case Keys.Right:
                    key = Standard.Enums.Keys.Right;
                    break;

                case Keys.Up:
                    key = Standard.Enums.Keys.Up;
                    break;

                case Keys.Down:
                    key = Standard.Enums.Keys.Down;
                    break;

                case Keys.Space:
                    key = Standard.Enums.Keys.Space;
                    break;

                case Keys.P:
                    key = Standard.Enums.Keys.P;
                    break;

                default:
                    return;
            }

            _controller.KeyDown(key);
        }

        private void frmAsteroids_KeyUp(object sender, KeyEventArgs e)
        {
            Standard.Enums.Keys key;
            switch (e.KeyData)
            {
                case Keys.Escape:
                    key = Standard.Enums.Keys.Escape;
                    break;

                case Keys.Left:
                    key = Standard.Enums.Keys.Left;
                    break;

                case Keys.Right:
                    key = Standard.Enums.Keys.Right;
                    break;

                case Keys.Up:
                    key = Standard.Enums.Keys.Up;
                    break;

                case Keys.Down:
                    key = Standard.Enums.Keys.Down;
                    break;

                case Keys.Space:
                    key = Standard.Enums.Keys.Space;
                    break;

                case Keys.P:
                    key = Standard.Enums.Keys.P;
                    break;

                default:
                    return;
            }

            _controller.KeyUp(key);
        }

        #region Setup

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.frame1 = new Asteroids.WinForms.Classes.GraphicPictureBox();
            this.frame2 = new Asteroids.WinForms.Classes.GraphicPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.frame1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frame2)).BeginInit();
            this.SuspendLayout();
            // 
            // frame1
            // 
            this.frame1.BackColor = System.Drawing.SystemColors.WindowText;
            this.frame1.Location = new System.Drawing.Point(189, 7);
            this.frame1.Name = "frame1";
            this.frame1.Size = new System.Drawing.Size(100, 50);
            this.frame1.TabIndex = 2;
            this.frame1.TabStop = false;
            // 
            // frame2
            // 
            this.frame2.BackColor = System.Drawing.SystemColors.WindowText;
            this.frame2.Location = new System.Drawing.Point(189, 72);
            this.frame2.Name = "frame2";
            this.frame2.Size = new System.Drawing.Size(100, 50);
            this.frame2.TabIndex = 3;
            this.frame2.TabStop = false;
            // 
            // frmAsteroids
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(632, 453);
            this.Controls.Add(this.frame2);
            this.Controls.Add(this.frame1);
            this.Name = "frmAsteroids";
            this.Text = "Asteroids";
            this.Activated += new System.EventHandler(this.frmAsteroids_Activated);
            this.Closed += new System.EventHandler(this.frmAsteroids_Closed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmAsteroids_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmAsteroids_KeyUp);
            this.Resize += new System.EventHandler(this.frmAsteroids_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.frame1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frame2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}