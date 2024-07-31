using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.ObjectiveC;
using System.Windows.Forms;


namespace CatsDefenders
{
	public partial class Form1 : Form
	{

		private PictureBox igrac;
		private int igracBrzina = 10;

		public Form1()
		{
			PostaviIgru();
		}

		private void PostaviIgru()
		{
			this.Text = "Cats Defenders";
			this.ClientSize = new Size(1360, 768);

			igrac = new PictureBox
			{
				Slika = Image.FromFile("C:/Users/Korisnik/Desktop/private/nebitno/veoma bitno/igrac.png"),
				Size = new Size(50, 50),
				Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height - 60),
				SizeMode = PictureBoxSizeMode.StretchImage
			};

			this.Controls.Add(igrac);

			this.KeyDown += new KeyEventHandler(Form1_KeyDown);
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left && igrac.Left > 0)
			{
				igrac.Left -= igracBrzina;
			}

			if (e.KeyCode == Keys.Right && igrac.Right < this.ClientSize.Width)
			{
				igrac.Left += igracBrzina;
			}

		}

		
	}
}
