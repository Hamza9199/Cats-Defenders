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
			form.bodoviLabel = new Label
			{
				Text = "Bodovi: 0",
				Location = new Point(10, 10),
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			// Postavlja labelu za pauziranje igre
			form.pauzirajIgru = new Label
			{
				Text = "Menu",
				Location = new Point(form.ClientSize.Width - 100, 10),
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true,
				Cursor = Cursors.Hand
			};

			// Postavlja labelu za prikaz broja života
			form.zivotText = new Label
			{
				Text = "Zivoti: 5",
				Location = new Point(600, 10),
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			// Postavlja labelu za prikaz trenutnog levela
			form.level = new Label
			{
				Text = "Level: 1",
				Location = new Point(10, 30),
				ForeColor = Color.White,
				BackColor = Color.Transparent,
				Font = new Font("Arial", 12, FontStyle.Bold),
				AutoSize = true
			};

			// Dodavanje labela na formu
			form.Controls.Add(form.level);
			form.Controls.Add(form.zivotText);
			form.Controls.Add(form.pauzirajIgru);
			form.Controls.Add(form.bodoviLabel);

			// Event handler za pauziranje igre
			form.pauzirajIgru.Click += new EventHandler(form.PauzaClick);

			// Postavljanje slike igrača
			form.igrac = new PictureBox
			{
				Size = new Size(50, 50),
				Location = new Point(form.ClientSize.Width / 2, form.ClientSize.Height - 60),
				SizeMode = PictureBoxSizeMode.StretchImage,
				Image = Image.FromFile(Path.Combine(form.basePath, "igrac.gif"))
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

			form.NeprijateljTimer.Tick += new EventHandler((sender, e) => form.DodajNevzu());
			form.NeprijateljTimer.Start();
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
