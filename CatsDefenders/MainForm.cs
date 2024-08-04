using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.IO;

namespace CatsDefenders
{
	public partial class MainForm : Form
	{
		// Definisanje varijabli za igru
		public PictureBox igrac;
		public int igracBrzina = 20;
		public List<PictureBox> meci = new List<PictureBox>();
		public int meciBrzina = 10;
		public System.Windows.Forms.Timer gameTimer;
		public List<PictureBox> Neprijatelj = new List<PictureBox>();
		public int NeprijateljBrzina = 1;
		public System.Windows.Forms.Timer NeprijateljTimer;
		public int bodovi = 0;
		public Label bodoviLabel;
		public Label pauzirajIgru;
		public bool PauzirajIgru = false;
		public Panel MenuPanel;
		public Label nastaviLabel;
		public Label ugasiLabel;
		public Label menuText;
		public int zivoti = 5;
		public Label zivotText;
		public int neprijatelji = 0;
		public Label level;
		public int levelN = 1;
		public int neprijateljUbijeniCount = 0;
		public WaveOutEvent pucanjZvuk;
		public WaveOutEvent eksplozijaZvuk;
		public WaveOutEvent pozadinaMuzika;
		public WaveOutEvent mrtavZvuk;
		public Panel LevelUP;
		public string basePath = AppDomain.CurrentDomain.BaseDirectory;

		// Konstruktor za MainForm
		public MainForm()
		{
			InitializeComponent();
			IgraPostavke.PostaviIgru(this);
			IgraPostavke.PostaviPozadinskuSliku(this);
			Zvuk.InicijalizirajZvukove(this);
			IgraMeni.PostaviMenuPanel(this);
			IgraMeni.PostaviLevelUpPanel(this);

			this.KeyPreview = true; 
			this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
		}

		// Obrada pritiska tipki
		public void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			// Kretanje igrača lijevo
			if (e.KeyCode == Keys.Left && igrac.Left > 0)
			{
				igrac.Left -= igracBrzina;
			}

			// Kretanje igrača desno
			if (e.KeyCode == Keys.Right && igrac.Right < this.ClientSize.Width)
			{
				igrac.Left += igracBrzina;
			}

			// Pucanje
			if (e.KeyCode == Keys.Space)
			{
				Pucaj();
				Zvuk.IgrajZvuk(Path.Combine(basePath, "pucanj.wav"));
			}

			// Pauziranje igre
			if (e.KeyCode == Keys.Escape)
			{
				if (!LevelUP.Visible)
				{
					if (PauzirajIgru)
					{
						IgraPostavke.nastaviIgru(this);
					}
					else
					{
						IgraPostavke.pauzirajIgruFunkcija(this);
					}
				}
			}

			if(e.KeyCode == Keys.Enter) { 				
				if (LevelUP.Visible)
				{
					IgraPostavke.nastaviIgru(this);
				}
			}
		}

		// Funkcija za pucanje
		private void Pucaj()
		{
			try
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
			catch (Exception ex)
			{
				MessageBox.Show("Greška prilikom pucanja: " + ex.Message);
			}
		}


		// Glavni timer igre
		public void GameTimer_Tick(object sender, EventArgs e)
		{
			// Pomjeranje metaka
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

			// Pomjeranje neprijatelja
			for (int i = Neprijatelj.Count - 1; i >= 0; i--)
			{
				PictureBox nevzo = Neprijatelj[i];
				nevzo.Top += NeprijateljBrzina;

				// Povećavanje brzine neprijatelja po levelima
				if (levelN == 3)
				{
					NeprijateljBrzina = 2;
				}
				else if (levelN == 6)
				{
					NeprijateljBrzina = 3;
				}
				else if (levelN == 10)
				{
					NeprijateljBrzina = 4;
				}

				// Uklanjanje neprijatelja koji su prošli ekran
				if (nevzo.Top > this.ClientSize.Height)
				{
					this.Controls.Remove(nevzo);
					Neprijatelj.Remove(nevzo);
					zivoti -= 1;
					zivotText.Text = "Zivoti: " + zivoti;
				}

				if (nevzo.Bounds.IntersectsWith(igrac.Bounds))
				{
					this.Controls.Remove(nevzo);
					Neprijatelj.Remove(nevzo);
					zivoti -= 1;
					zivotText.Text = "Zivoti: " + zivoti;
				}
			}

			// Provjera kraja igre
			if (zivoti == 0)
			{
				KrajIgre();
			}

			

			// Petlja pozadinske muzike
			if (pozadinaMuzika.PlaybackState == PlaybackState.Stopped)
			{
				pozadinaMuzika.Init(new AudioFileReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pozadina.wav")));
				pozadinaMuzika.Play();
			}

			Neprijatelji.Sudar(this);
			levelFollow();
		}

		// Dodavanje novog neprijatelja
		public void DodajNevzu()
		{
			Neprijatelji.DodajNevzu(this);
		}

		// Praćenje promjene levela
		public void levelFollow()
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

		// Promjena levela
		public void PromijeniLevel(int noviLevel)
		{
			neprijateljUbijeniCount++;
			level.Text = "Level: " + noviLevel;
			gameTimer.Stop();
			NeprijateljTimer.Stop();

			// Uklanjanje preostalih neprijatelja
			for (int i = Neprijatelj.Count - 1; i >= 0; i--)
			{
				this.Controls.Remove(Neprijatelj[i]);
				Neprijatelj[i].Dispose();
			}

			Neprijatelj.Clear();

			LevelUP.Visible = true;
			pauzirajIgru.Visible = false;
			levelN++;
		}

		// Funkcija za kraj igre
		public void KrajIgre()
		{
			gameTimer.Stop();
			NeprijateljTimer.Stop();
			pozadinaMuzika.Stop();
			Zvuk.IgrajZvuk(Path.Combine(basePath, "mrtav.wav"));
			MessageBox.Show("Izgubili ste! Osvojili ste " + bodovi + " bodova.");
			Application.Restart();
		}

		// Klik na "Ugasi" labelu
		public void UgasiLabel_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		// Povećavanje bodova
		public void PovecajBodove()
		{
			bodovi += 1;
			bodoviLabel.Text = "Bodovi: " + bodovi;
		}

		// Pauziranje igre klikom
		public void PauzaClick(object sender, EventArgs e)
		{
			if (PauzirajIgru)
			{
				IgraPostavke.nastaviIgru(this);
			}
			else
			{
				IgraPostavke.pauzirajIgruFunkcija(this);
			}
		}

		// Povratak u meni klikom
		public void MenuClick(object sender, EventArgs e)
		{
			IgraPostavke.nastaviIgru(this);
		}
	}
}
