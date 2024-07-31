using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CatsDefenders
{
	public partial class MainForm : Form
	{
		private PictureBox igrac;
		private int igracBrzina = 20;
		private List<PictureBox> meci = new List<PictureBox>();
		private int meciBrzina = 10;
		private System.Windows.Forms.Timer gameTimer;
		private List<PictureBox> Nevzudin = new List<PictureBox>();
		private int NevzuBrzina = 1;
		private System.Windows.Forms.Timer NevzoTimer;
		private int bodovi = 0;
		private Label bodoviLabel;
		private Label pauzirajIgru;
		private bool PauzirajIgru = false;
		private Panel MenuPanel;
		private Label nastaviLabel;
		private Label ugasiLabel;
		private Label menuText;
		private int zivoti = 5;
		private Label zivotText;
		private int nevze = 0;

		public MainForm()
		{
			InitializeComponent();
			PostaviIgru();
			PostaviPozadinskuSliku();
		}

		private void PostaviPozadinskuSliku()
		{
			this.BackgroundImage = Image.FromFile("C:/Users/Korisnik/Desktop/private/pozadina.png");
			this.BackgroundImageLayout = ImageLayout.Stretch;		
		}

		private void PostaviIgru()
		{
			this.Text = "Cats Defenders";
			this.ClientSize = new Size(800, 600);

			bodoviLabel = new Label
			{
				Text = "Bodovi: 0",
				Location = new Point(10, 10),
				ForeColor = Color.Black,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			pauzirajIgru = new Label
			{
				Text = "Menu",
				Location = new Point(this.ClientSize.Width - 100, 10),
				ForeColor = Color.Black,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			zivotText = new Label
			{
				Text = "Zivoti: " + zivoti,
				Location = new Point(10, 30),
				ForeColor = Color.Black,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			this.Controls.Add(zivotText);
			this.Controls.Add(bodoviLabel);
			this.Controls.Add(pauzirajIgru);
			pauzirajIgru.Click += new EventHandler(PauzaClick);

			igrac = new PictureBox
			{
				Image = Image.FromFile("C:/Users/Korisnik/Desktop/private/igrac.jpg"),
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
			NevzoTimer.Interval = 2000;
			NevzoTimer.Tick += (sender, e) => DodajNevzu();
			NevzoTimer.Start();

			PostaviMenuPanel();
		}

		private void UgasiLabel_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void PostaviMenuPanel()
		{
			MenuPanel = new Panel
			{
				Size = new Size(300, 200), 
				Location = new Point((this.ClientSize.Width / 2) - 150, (this.ClientSize.Height / 2) - 100),
				BackColor = Color.FromArgb(170,0,0,0), 
				Visible = false
			};

			menuText = new Label
			{
				Text = "Cats Defenders",
				Location = new Point(42, 10), 
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 20, FontStyle.Bold),
				AutoSize = true,
				
			};

			nastaviLabel = new Label
			{
				Text = "Nastavi igru",
				Location = new Point(50, 60), 
				ForeColor = Color.White,
				Font = new Font("Arial", 16, FontStyle.Bold),
				AutoSize = true,
				BackColor = Color.Transparent,
				Cursor = Cursors.Hand
			};

			ugasiLabel = new Label
			{
				Text = "Izlaz",
				Location = new Point(50, 80), 
				ForeColor = Color.White,
				Font = new Font("Arial", 16, FontStyle.Bold),
				AutoSize = true,
				BackColor = Color.Transparent,
				Cursor = Cursors.Hand
			};

			nastaviLabel.Click += new EventHandler(MenuClick);
			ugasiLabel.Click += new EventHandler(UgasiLabel_Click);

			MenuPanel.Controls.Add(menuText);
			MenuPanel.Controls.Add(nastaviLabel);
			MenuPanel.Controls.Add(ugasiLabel);
			this.Controls.Add(MenuPanel);
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

			if (e.KeyCode == Keys.Escape)
			{
				if (PauzirajIgru)
				{
					nastaviIgru();
				}
				else
				{
					pauzirajIgruFunkcija();
				}
			}
		}

		private void Pucaj()
		{
			PictureBox metak = new PictureBox
			{
				Size = new Size(5, 10),
				Location = new Point(igrac.Left + igrac.Width / 2 - 2, igrac.Top),
				BackColor = Color.White
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

			for (int i = Nevzudin.Count - 1; i >= 0; i--)
			{
				PictureBox nevzo = Nevzudin[i];
				nevzo.Top += NevzuBrzina;



				if (nevze > 10)
				{
					NevzuBrzina = 2;
				}
				else if (nevze > 30)
				{
					NevzuBrzina = 3;
				}
				else if (nevze > 50)
				{
					NevzuBrzina = 4;
				}
				


				if (nevzo.Top > this.ClientSize.Height)
				{
					this.Controls.Remove(nevzo);
					Nevzudin.Remove(nevzo);
					zivoti -= 1;
					zivotText.Text = "Zivoti: " + zivoti;
				}

			}

			if (zivoti == 0)
			{
				KrajIgre();
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
			nevze++;
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

				if (nevzo.Bounds.IntersectsWith(igrac.Bounds))
				{
					KrajIgre();
				}

			}
		}

		private void PovecajBodove()
		{
			bodovi += 1;
			bodoviLabel.Text = "Bodovi: " + bodovi;
		}

		private void KrajIgre()
		{
			gameTimer.Stop();
			NevzoTimer.Stop();
			MessageBox.Show("Izgubili ste! Osvojili ste " + bodovi + " bodova.");
			Application.Restart();
		}

		private void pauzirajIgruFunkcija()
		{
			gameTimer.Stop();
			NevzoTimer.Stop();
			PauzirajIgru = true;
			MenuPanel.Visible = true;
			pauzirajIgru.Visible = false;
		}


		private void nastaviIgru()
		{
			gameTimer.Start();
			NevzoTimer.Start();
			PauzirajIgru = false;
			MenuPanel.Visible = false;
			pauzirajIgru.Visible = true;
		}


		private void PauzaClick(object sender, EventArgs e)
		{
			if (PauzirajIgru)
			{
				nastaviIgru();
			}
			else
			{
				pauzirajIgruFunkcija();
			}
		}

		private void MenuClick(object sender, EventArgs e)
		{
			nastaviIgru();
		}
	}
}
