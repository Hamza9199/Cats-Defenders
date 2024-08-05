using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace CatsDefenders
{
	public static class Neprijatelji
	{
		// Dodaje novog neprijatelja na formu
		public static void DodajNeprijatelja(MainForm form)
		{
			PictureBox neprijatelj = new PictureBox
			{
				Size = new Size(50, 50),
				Location = new Point(new Random().Next(0, form.ClientSize.Width - 50), -50),
				SizeMode = PictureBoxSizeMode.StretchImage,
				Image = Image.FromFile(Path.Combine(form.basePath, "neprijatelj.png"))
			};

			form.Controls.Add(neprijatelj);
			form.Neprijatelj.Add(neprijatelj);
		}

		// Provjerava sudare između metaka i neprijatelja
		public static void ProvjeriSudare(MainForm form)
		{
			if (form.meci.Count == 0 || form.Neprijatelj.Count == 0)
				return;

			foreach (var metak in form.meci.ToArray())
			{
				foreach (var neprijatelj in form.Neprijatelj.ToArray())
				{
					if (metak.Bounds.IntersectsWith(neprijatelj.Bounds))
					{
						form.Controls.Remove(metak);
						form.meci.Remove(metak);
						form.Controls.Remove(neprijatelj);
						form.Neprijatelj.Remove(neprijatelj);

						form.PovecajBodove();
						form.neprijateljUbijeniCount++;

						Zvuk.PustiZvukEksplozije(form.basePath);
						break;
					}
				}
			}
		}
	}
}
