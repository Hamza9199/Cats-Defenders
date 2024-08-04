using System;
using System.Drawing;
using System.Windows.Forms;
using NAudio.Wave;
using System.IO;


namespace CatsDefenders
{
	public static class Neprijatelji
	{
		// Dodaje novog neprijatelja na formu
		public static void DodajNevzu(MainForm form)
		{
			PictureBox nevzo = new PictureBox
			{
				Size = new Size(50, 50),
				Location = new Point(new Random().Next(0, form.ClientSize.Width - 50), -50),
				SizeMode = PictureBoxSizeMode.StretchImage,
				Image = Image.FromFile(Path.Combine(form.basePath, "neprijatelj.png"))
			};

			form.Controls.Add(nevzo);
			form.Neprijatelj.Add(nevzo);
		}

		// Provjerava sudare između metaka i neprijatelja
		public static void Sudar(MainForm form)
		{
			try
			{
				if (form.meci.Count == 0 || form.Neprijatelj.Count == 0)
					return;

				for (int i = form.meci.Count - 1; i >= 0; i--)
				{
					PictureBox metak = form.meci[i];
					for (int j = form.Neprijatelj.Count - 1; j >= 0; j--)
					{
						PictureBox nevzo = form.Neprijatelj[j];
						if (metak.Bounds.IntersectsWith(nevzo.Bounds))
						{
							form.Controls.Remove(metak);
							form.meci.Remove(metak);
							form.Controls.Remove(nevzo);
							form.Neprijatelj.Remove(nevzo);

							form.PovecajBodove();
							form.neprijateljUbijeniCount++;

							Zvuk.PustiZvukEksplozije(form.basePath);
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Došlo je do greške: " + ex.Message);
			}
		}

	}
}
