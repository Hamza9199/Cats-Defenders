using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CatsDefenders
{
	public partial class MainForm : Form
	{
		private PictureBox igrac;
		private int igracBrzina = 10;
		private List<PictureBox> meci = new List<PictureBox>();
		private int meciBrzina = 20;
		private System.Windows.Forms.Timer gameTimer;

		public MainForm()
		{
			InitializeComponent();
			PostaviIgru();
		}

		private void PostaviIgru()
		{
			this.Text = "Cats Defenders";
			this.ClientSize = new Size(1360, 768);

			igrac = new PictureBox
			{
				Image = Image.FromFile("C:/Users/Korisnik/Desktop/private/nebitno/veoma bitno/igrac.png"),
				Size = new Size(50, 50),
				Location = new Point(this.ClientSize.Width / 2 - 25, this.ClientSize.Height - 60), // Centriraj igrača
				SizeMode = PictureBoxSizeMode.StretchImage
			};

			this.Controls.Add(igrac);

			this.KeyDown += new KeyEventHandler(MainForm_KeyDown);

			gameTimer = new System.Windows.Forms.Timer();
			gameTimer.Interval = 20;
			gameTimer.Tick += new EventHandler(GameTimer_Tick);
			gameTimer.Start();
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left && igrac.Left > 0)
			{
				igrac.Left -= igracBrzina;
			}

			if (e.KeyCode == Keys.Right && igrac.Right < this.ClientSize.Width)
			{
				igrac.Left += igracBrzina;
			}

			if (e.KeyCode == Keys.Space)
			{
				Pucaj();
			}
		}

		private void Pucaj()
		{
			PictureBox metak = new PictureBox
			{
				Size = new Size(5, 20),
				Location = new Point(igrac.Left + igrac.Width / 2 - 2, igrac.Top), // Centriraj metak
				BackColor = Color.Black
			};
			this.Controls.Add(metak);
			meci.Add(metak);
		}

		private void GameTimer_Tick(object sender, EventArgs e)
		{
			for (int i = meci.Count - 1; i >= 0; i--)
			{
				PictureBox metak = meci[i];
				metak.Top -= meciBrzina;
				if (metak.Top < 0)
				{
					this.Controls.Remove(metak);
					meci.Remove(metak);
				}
			}
		}
	}
}
