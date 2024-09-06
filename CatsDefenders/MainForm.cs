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
		public int igracBrzina = 40;
		public List<PictureBox> meci = new List<PictureBox>();
		public int meciBrzina = 40;
		public System.Windows.Forms.Timer gameTimer;
		public List<PictureBox> Neprijatelj = new List<PictureBox>();
		public int neprijateljBrzina = 1;
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
		public Label level;
		public int levelN = 1;
		public int neprijateljUbijeniCount = 0;
		public WaveOutEvent pucanjZvuk;
		public WaveOutEvent eksplozijaZvuk;
		public WaveOutEvent pozadinaMuzika;
		public WaveOutEvent mrtavZvuk;
		public Panel LevelUP;
        public Boss boss = null;

        public string basePath = AppDomain.CurrentDomain.BaseDirectory;

		public MainForm()
		{
			InitializeComponent();
			IgraPostavke.PostaviIgru(this);
			IgraPostavke.PostaviPozadinskuSliku(this);
			Zvuk.InicijalizirajZvukove(this);
			IgraMeni.PostaviMenuPanel(this);
			IgraMeni.PostaviLevelUpPanel(this);

			this.KeyPreview = true;
			this.KeyDown += MainForm_KeyDown;
		}

		public void MainForm_KeyDown(object sender, KeyEventArgs e)
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
				Zvuk.IgrajZvuk(Path.Combine(basePath, "pucanj.wav"));
			}
			if (e.KeyCode == Keys.Escape)
			{
				TogglePause();
			}
			if (e.KeyCode == Keys.Enter && LevelUP.Visible)
			{
				IgraPostavke.nastaviIgru(this);
			}
		}

		public void Pucaj()
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
				MessageBox.Show($"Greška prilikom pucanja: {ex.Message}");
			}
		}

		public void GameTimer_Tick(object sender, EventArgs e)
		{
			MoveMeci();
			MoveNeprijatelji();
			CheckBackgroundMusic();
			Neprijatelji.ProvjeriSudare(this);
			levelFollow();


            // Pojava Bossa na trećem nivou
            if (levelN == 3 && boss == null)
            {
                boss = new Boss(this);
            }

            // Ako je Boss prisutan, kontroliraj njegovo kretanje
            if (boss != null)
            {
                boss.Kretanje(this);

                // Provjera sudara metka sa Bossom
                for (int i = meci.Count - 1; i >= 0; i--)
                {
                    PictureBox metak = meci[i];
                    if (metak.Bounds.IntersectsWith(boss.BossPictureBox.Bounds))
                    {
                        this.Controls.Remove(metak);
                        meci.Remove(metak);
                        boss.PrimiStetu(this);
                    }
                }
            }
        }

		public void MoveMeci()
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

		private void MoveNeprijatelji()
		{
			for (int i = Neprijatelj.Count - 1; i >= 0; i--)
			{
				PictureBox nevzo = Neprijatelj[i];
				nevzo.Top += neprijateljBrzina;

				AdjustEnemySpeed();

				if (nevzo.Top > this.ClientSize.Height || nevzo.Bounds.IntersectsWith(igrac.Bounds))
				{
					HandleEnemyHit(nevzo);
				}
				if (neprijateljUbijeniCount == 210)
				{
					EndGame("Pobjedili ste!");
				}
			}
		}

		private void AdjustEnemySpeed()
		{
			if (levelN == 3)
			{
				neprijateljBrzina = 2;
			}
			else if (levelN == 6)
			{
				neprijateljBrzina = 3;
			}
			else if (levelN == 10)
			{
				neprijateljBrzina = 4;
			}
		}

		public void HandleEnemyHit(PictureBox nevzo)
		{
			this.Controls.Remove(nevzo);
			Neprijatelj.Remove(nevzo);
			zivoti -= 1;
			zivotText.Text = $"Zivoti: {zivoti}";
			if (zivoti == 0)
			{
				EndGame("Izgubili ste!");
			}
		}

		public void CheckBackgroundMusic()
		{
			if (pozadinaMuzika.PlaybackState == PlaybackState.Stopped)
			{
				pozadinaMuzika.Init(new AudioFileReader(Path.Combine(basePath, "pozadina.wav")));
				pozadinaMuzika.Play();
			}
		}

		public void DodajNevzu()
		{
			Neprijatelji.DodajNeprijatelja(this);
		}

		public void levelFollow()
		{
			int[] thresholds = { 10, 31, 52, 73, 94, 115, 136, 157, 178 };
			for (int i = 0; i < thresholds.Length; i++)
			{
				if (neprijateljUbijeniCount == thresholds[i])
				{
					PromijeniLevel(i + 2);
					break;
				}
			}
		}

		public void PromijeniLevel(int noviLevel)
		{
			neprijateljUbijeniCount++;
			level.Text = $"Level: {noviLevel}";
			gameTimer.Stop();
			NeprijateljTimer.Stop();
			ClearEnemies();
            if (noviLevel != 3 || boss == null) // Boss se kreira samo jednom na trećem nivou
            {
                LevelUP.Visible = true;
            }
            pauzirajIgru.Visible = false;
			levelN++;
		}

		private void ClearEnemies()
		{
			foreach (var nevzo in Neprijatelj)
			{
				this.Controls.Remove(nevzo);
				nevzo.Dispose();
			}
			Neprijatelj.Clear();
		}

		public void EndGame(string message)
		{
			gameTimer.Stop();
			NeprijateljTimer.Stop();
			pozadinaMuzika.Stop();
			Zvuk.IgrajZvuk(Path.Combine(basePath, "mrtav.wav"));
			MessageBox.Show($"{message} Osvojili ste {bodovi} bodova.");
			Application.Restart();
		}

		private void TogglePause()
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

		public void UgasiLabel_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		public void PovecajBodove()
		{
			bodovi += 1;
			bodoviLabel.Text = $"Bodovi: {bodovi}";
		}

		public void PauzaClick(object sender, EventArgs e)
		{
			TogglePause();
		}

		public void MenuClick(object sender, EventArgs e)
		{
			IgraPostavke.nastaviIgru(this);
		}
	}
}
