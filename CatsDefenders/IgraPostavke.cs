using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace CatsDefenders
{
	public static class IgraPostavke
	{
		// Postavlja osnovne postavke igre
		public static void PostaviIgru(MainForm form)
		{
			form.Text = "Cats Defenders";
			form.FormBorderStyle = FormBorderStyle.None;
			form.WindowState = FormWindowState.Maximized;
			form.Bounds = Screen.PrimaryScreen.Bounds;

			// Postavlja labelu za bodove
			form.bodoviLabel = KreirajLabelu("Bodovi: 0", new Point(10, 10));

			// Postavlja labelu za pauziranje igre
			form.pauzirajIgru = KreirajLabelu("Menu", new Point(form.ClientSize.Width - 100, 10));
			form.pauzirajIgru.Cursor = Cursors.Hand;
			form.pauzirajIgru.Click += new EventHandler(form.PauzaClick);

			// Postavlja labelu za prikaz broja života
			form.zivotText = KreirajLabelu("Životi: 5", new Point(600, 10));

			// Postavlja labelu za prikaz trenutnog levela
			form.level = KreirajLabelu("Level: 1", new Point(10, 30));

			// Dodavanje labela na formu
			form.Controls.Add(form.level);
			form.Controls.Add(form.zivotText);
			form.Controls.Add(form.pauzirajIgru);
			form.Controls.Add(form.bodoviLabel);

			// Postavljanje slike igrača
			form.igrac = new PictureBox
			{
				Size = new Size(50, 50),
				Location = new Point(form.ClientSize.Width / 2 - 25, form.ClientSize.Height - 60),
				SizeMode = PictureBoxSizeMode.StretchImage,
				Image = Image.FromFile(Path.Combine(form.basePath, "igrac.png"))
			};
			form.Controls.Add(form.igrac);

			// Postavljanje timera za igru
			form.gameTimer = new System.Windows.Forms.Timer
			{
				Interval = 20,
			};
			form.gameTimer.Tick += new EventHandler(form.GameTimer_Tick);
			form.gameTimer.Start();

			// Postavljanje timera za neprijatelje
			form.NeprijateljTimer = new System.Windows.Forms.Timer
			{
				Interval = 2500,
			};
			form.NeprijateljTimer.Tick += (sender, e) => form.DodajNevzu();
			form.NeprijateljTimer.Start();
		}

		private static Label KreirajLabelu(string tekst, Point lokacija)
		{
			return new Label
			{
				Text = tekst,
				Location = lokacija,
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};
		}

		// Postavlja pozadinsku sliku igre
		public static void PostaviPozadinskuSliku(MainForm form)
		{
			form.BackgroundImage = Image.FromFile(Path.Combine(form.basePath, "pozadina.png"));
			form.BackgroundImageLayout = ImageLayout.Stretch;
		}

		// Nastavlja igru nakon pauze
		public static void nastaviIgru(MainForm form)
		{
			form.MenuPanel.Visible = false;
			form.LevelUP.Visible = false;
			form.pauzirajIgru.Visible = true;
			form.gameTimer.Start();
			form.NeprijateljTimer.Start();
			form.PauzirajIgru = false;
			form.pozadinaMuzika.Play();
		}

		// Pauzira igru i prikazuje meni
		public static void pauzirajIgruFunkcija(MainForm form)
		{
			form.MenuPanel.Visible = true;
			form.gameTimer.Stop();
			form.NeprijateljTimer.Stop();
			form.PauzirajIgru = true;
			form.pozadinaMuzika.Pause();
		}
	}
}
