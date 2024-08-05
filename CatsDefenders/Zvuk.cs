using System;
using NAudio.Wave;
using System.IO;

namespace CatsDefenders
{
	public static class Zvuk
	{
		// Inicijalizira zvukove za igru
		public static void InicijalizirajZvukove(MainForm form)
		{
			try
			{
				form.pucanjZvuk = new WaveOutEvent();
				form.eksplozijaZvuk = new WaveOutEvent();
				form.pozadinaMuzika = new WaveOutEvent();
				form.mrtavZvuk = new WaveOutEvent();

				// Postavljanje i pokretanje pozadinske muzike
				var pozadinaPutanja = Path.Combine(form.basePath, "pozadina.wav");
				form.pozadinaMuzika.Init(new AudioFileReader(pozadinaPutanja));
				form.pozadinaMuzika.Play();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Greška prilikom inicijalizacije zvukova: {ex.Message}");
			}
		}

		// Pusti zvuk eksplozije
		public static void PustiZvukEksplozije(string basePath)
		{
			PustiZvuk(Path.Combine(basePath, "eksplozija.wav"));
		}

		// Pokreće zvuk za određeni fajl
		public static void IgrajZvuk(string path)
		{
			PustiZvuk(path);
		}

		// Privatna metoda za puštanje zvuka
		private static void PustiZvuk(string path)
		{
			try
			{
				using (var zvuk = new WaveOutEvent())
				{
					zvuk.Init(new AudioFileReader(path));
					zvuk.Play();
					while (zvuk.PlaybackState == PlaybackState.Playing)
					{
						Application.DoEvents();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Greška prilikom puštanja zvuka: {ex.Message}");
			}
		}
	}
}
