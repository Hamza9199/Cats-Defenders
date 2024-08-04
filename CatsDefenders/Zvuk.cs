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
			form.pucanjZvuk = new WaveOutEvent();
			form.eksplozijaZvuk = new WaveOutEvent();
			form.pozadinaMuzika = new WaveOutEvent();
			form.mrtavZvuk = new WaveOutEvent();

			// Postavljanje i pokretanje pozadinske muzike
			form.pozadinaMuzika.Init(new AudioFileReader(Path.Combine(form.basePath, "pozadina.wav")));
			form.pozadinaMuzika.Play();
		}

		public static void PustiZvukEksplozije(string basePath)
		{
			using (var eksplozijaZvuk = new WaveOutEvent())
			{
				eksplozijaZvuk.Init(new AudioFileReader(Path.Combine(basePath, "eksplozija.wav")));
				eksplozijaZvuk.Play();
				while (eksplozijaZvuk.PlaybackState == PlaybackState.Playing)
				{
					Application.DoEvents();
				}
			}
		}

		// Pokreće zvuk za određeni fajl
		public static void IgrajZvuk(string path)
		{
			using (var pucanjZvuk = new WaveOutEvent())
			{
				pucanjZvuk.Init(new AudioFileReader(path));
				pucanjZvuk.Play();
				while (pucanjZvuk.PlaybackState == PlaybackState.Playing)
				{
					Application.DoEvents(); 
				}
			}
		}

	}
}
