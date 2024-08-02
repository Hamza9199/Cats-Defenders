using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.IO;
using System.Media;


namespace CatsDefenders
{
	public partial class MainForm : Form
	{
		private PictureBox igrac;
		private int igracBrzina = 30;
		private List<PictureBox> meci = new List<PictureBox>();
		private int meciBrzina = 15;
		private System.Windows.Forms.Timer gameTimer;
		private List<PictureBox> Neprijatelj = new List<PictureBox>();
		private int NeprijateljBrzina = 1; 
		private System.Windows.Forms.Timer NeprijateljTimer;
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
		private int neprijatelji = 0;
		private Label level;
		private int neprijateljUbijeniCount = 0;
		private WaveOutEvent pucanjZvuk;
		private WaveOutEvent eksplozijaZvuk;
		private WaveOutEvent pozadinaMuzika;
		private WaveOutEvent mrtavZvuk;

		private string basePath = AppDomain.CurrentDomain.BaseDirectory;

		public MainForm()
		{
			InitializeComponent();
			PostaviIgru();
			PostaviPozadinskuSliku();

		}

		private void PostaviPozadinskuSliku()
		{
			this.BackgroundImage = Image.FromFile(Path.Combine(basePath, "pozadina.png")); 
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
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			pauzirajIgru = new Label
			{
				Text = "Menu",
				Location = new Point(this.ClientSize.Width - 100, 10),
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true,
				Cursor = Cursors.Hand
			};

			zivotText = new Label
			{
				Text = "Zivoti: " + zivoti,
				Location = new Point(10, 30),
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			level = new Label
			{
				Text = "Level: 1",
				Location = new Point(10, 50),
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			this.Controls.Add(level);
			this.Controls.Add(zivotText);
			this.Controls.Add(bodoviLabel);
			this.Controls.Add(pauzirajIgru);
			pauzirajIgru.Click += new EventHandler(PauzaClick);

			igrac = new PictureBox
			{
				Image = Image.FromFile(Path.Combine(basePath, "igrac.png")),
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

			NeprijateljTimer = new System.Windows.Forms.Timer();
			NeprijateljTimer.Interval = 2000;
			NeprijateljTimer.Tick += (sender, e) => DodajNevzu();
			NeprijateljTimer.Start();


			pucanjZvuk = new WaveOutEvent();
			eksplozijaZvuk = new WaveOutEvent();
			pozadinaMuzika = new WaveOutEvent();
			mrtavZvuk = new WaveOutEvent();

			pucanjZvuk.Init(new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pucanj.wav")));
			eksplozijaZvuk.Init(new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "eksplozija.wav")));
			pozadinaMuzika.Init(new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pozadina.wav")));
			mrtavZvuk.Init(new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mrtav.wav")));



			pozadinaMuzika.Volume = 0.5f;
			pozadinaMuzika.PlaybackStopped += (s, e) => pozadinaMuzika.Play();

			pozadinaMuzika.Play();


			PostaviMenuPanel();

		}

		private void levelFollow()
		{
			switch (neprijateljUbijeniCount)
			{
				case 10:
					PromijeniLevel(2);
					break;
				case 31:
					PromijeniLevel(3);
					break;
				case 52:
					PromijeniLevel(4);
					break;
				case 73:
					PromijeniLevel(5);
					break;
				case 94:
					PromijeniLevel(6);
					break;
				case 115:
					PromijeniLevel(7);
					break;
				case 136:
					PromijeniLevel(8);
					break;
				case 157:
					PromijeniLevel(9);
					break;
				case 178:
					PromijeniLevel(10);
					break;
				default:
					break;
			}
		}

		private void PromijeniLevel(int noviLevel)
		{
			neprijateljUbijeniCount++;
			level.Text = "Level: " + noviLevel;
			Neprijatelj.Clear();
			gameTimer.Stop();
			NeprijateljTimer.Stop();

			MenuPanel.Visible = true;
			pauzirajIgru.Visible = false;

			
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
				Visible = false,
				AutoSize = true
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
				IgrajZvuk(Path.Combine(basePath, "pucanj.wav"));
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

			for (int i = Neprijatelj.Count - 1; i >= 0; i--)
			{
				PictureBox nevzo = Neprijatelj[i];
				nevzo.Top += NeprijateljBrzina;



				if (neprijatelji > 10)
				{
					NeprijateljBrzina = 2;
				}
				else if (neprijatelji > 30)
				{
					NeprijateljBrzina = 3;
				}
				else if (neprijatelji > 50)
				{
					NeprijateljBrzina = 4;
				}
				


				if (nevzo.Top > this.ClientSize.Height)
				{
					this.Controls.Remove(nevzo);
					Neprijatelj.Remove(nevzo);
					zivoti -= 1;
					zivotText.Text = "Zivoti: " + zivoti;
				}

			}

			if (zivoti == 0)
			{
				KrajIgre();
			}

			Sudar();
			levelFollow();

		}

		private void DodajNevzu()
		{
			PictureBox neprijatelj = new PictureBox
			{
				Image = Image.FromFile(Path.Combine(basePath, "neprijatelj.png")),
				Size = new Size(50, 50),
				Location = new Point(new Random().Next(0, this.ClientSize.Width - 50), -50),
				SizeMode = PictureBoxSizeMode.StretchImage
			};

			this.Controls.Add(neprijatelj);
			Neprijatelj.Add(neprijatelj);
			neprijatelji++;
		}

		private void Sudar()
		{
			for (int i = Neprijatelj.Count - 1; i >= 0; i--)
			{
				PictureBox neprijatelj = Neprijatelj[i];

				for (int j = meci.Count - 1; j >= 0; j--)
				{
					PictureBox metak = meci[j];

					if (neprijatelj.Bounds.IntersectsWith(metak.Bounds))
					{
						this.Controls.Remove(neprijatelj);
						Neprijatelj.Remove(neprijatelj);
						this.Controls.Remove(metak);
						meci.Remove(metak);
						PovecajBodove();
						neprijateljUbijeniCount++;

						IgrajZvuk(Path.Combine(basePath, "eksplozija.wav"));

						break;
					}
				}

				if (neprijatelj.Bounds.IntersectsWith(igrac.Bounds))
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
			NeprijateljTimer.Stop();
			pozadinaMuzika.Stop();
			mrtavZvuk.Play();
			MessageBox.Show("Izgubili ste! Osvojili ste " + bodovi + " bodova.");
			Application.Restart();
		}

		private void pauzirajIgruFunkcija()
		{
			gameTimer.Stop();
			NeprijateljTimer.Stop();
			PauzirajIgru = true;
			MenuPanel.Visible = true;
			pauzirajIgru.Visible = false;
			pozadinaMuzika.Pause();
		}


		private void nastaviIgru()
		{
			gameTimer.Start();
			NeprijateljTimer.Start();
			PauzirajIgru = false;
			MenuPanel.Visible = false;
			pauzirajIgru.Visible = true;
			pozadinaMuzika.Play();
		}

		private void IgrajZvuk(string putanja)
		{
			using (var zvuk = new WaveOutEvent())
			using (var audioReader = new AudioFileReader(putanja))
			{
				zvuk.Init(audioReader);
				zvuk.Play();
				while (zvuk.PlaybackState == PlaybackState.Playing)
				{
					Application.DoEvents();
				}
			}
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
