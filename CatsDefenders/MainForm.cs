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
		private List<PictureBox> Nevzudin = new List<PictureBox>();
		private int NevzuBrzina = 5;
		private System.Windows.Forms.Timer NevzoTimer;
		private int bodovi = 0;
		private Label bodoviLabel;

		public MainForm()
		{
			InitializeComponent();
			PostaviIgru();
		}

		private void PostaviIgru()
		{
			this.Text = "Cats Defenders";
			this.ClientSize = new Size(800, 600);

			bodoviLabel = new Label
			{
				Text = "Bodovi: 0",
				Location = new Point(10, 10),
				ForeColor = Color.White,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			this.Controls.Add(bodoviLabel);

			igrac = new PictureBox
			{
				Image = Image.FromFile("C:/Users/Korisnik/Desktop/private/igrac.png"),
				Size = new Size(50, 50),
				Location = new Point(this.ClientSize.Width / 2 - 25, this.ClientSize.Height - 60), 
				SizeMode = PictureBoxSizeMode.StretchImage
			};

			this.Controls.Add(igrac);

			this.KeyDown += new KeyEventHandler(MainForm_KeyDown);

			gameTimer = new System.Windows.Forms.Timer();
			gameTimer.Interval = 20;
			gameTimer.Tick += new EventHandler(GameTimer_Tick);
			gameTimer.Start();

			NevzoTimer = new System.Windows.Forms.Timer();
			NevzoTimer.Interval = 1000;
			NevzoTimer.Tick += (sender, e) => DodajNevzu();
			NevzoTimer.Start();
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
				Location = new Point(igrac.Left + igrac.Width / 2 - 2, igrac.Top), 
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

			for(int i = Nevzudin.Count - 1; i >= 0; i--)
			{
				PictureBox nevzo = Nevzudin[i];
				nevzo.Top += NevzuBrzina;
				if (nevzo.Top > this.ClientSize.Height)
				{
					this.Controls.Remove(nevzo);
					Nevzudin.Remove(nevzo);
				}
			}

			Sudar();
		}

		private void DodajNevzu()
		{
			PictureBox nevzo = new PictureBox
			{
				Image = Image.FromFile("C:/Users/Korisnik/Desktop/private/nevzudin.jpg"),
				Size = new Size(50, 50),
				Location = new Point(new Random().Next(0, this.ClientSize.Width - 50), -50),
				SizeMode = PictureBoxSizeMode.StretchImage
			};

			this.Controls.Add(nevzo);
			Nevzudin.Add(nevzo);
		}

		private void Sudar()
		{
			for (int i = Nevzudin.Count - 1; i >= 0; i--)
			{
				PictureBox nevzo = Nevzudin[i];

				for (int j = meci.Count - 1; j >= 0; j--)
				{
					PictureBox metak = meci[j];

					if (nevzo.Bounds.IntersectsWith(metak.Bounds))
					{
						this.Controls.Remove(nevzo);
						Nevzudin.Remove(nevzo);
						this.Controls.Remove(metak);
						meci.Remove(metak);
						PovecajBodove();
						break;
					}
				}

				if(nevzo.Bounds.IntersectsWith(igrac.Bounds))
				{
					KrajIgre();
				}

			}
		}

		private void PovecajBodove()
		{
			bodovi += 10;
			bodoviLabel.Text = "Bodovi: " + bodovi;
		}

		private void KrajIgre()
		{
			gameTimer.Stop();
			NevzoTimer.Stop();
			MessageBox.Show("Izgubili ste! Osvojili ste " + bodovi + " bodova.");
		}


	}
}
