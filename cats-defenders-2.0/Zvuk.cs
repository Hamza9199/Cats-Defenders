using Godot;
using System;

public class Zvuk : Node
{
	private AudioStreamPlayer pucanjZvuk;
	private AudioStreamPlayer eksplozijaZvuk;
	private AudioStreamPlayer pozadinaMuzika;
	private AudioStreamPlayer mrtavZvuk;

	public override void _Ready()
	{
		// Učitaj AudioStreamPlayere iz scene
		pucanjZvuk = GetNode<AudioStreamPlayer>("PucanjZvuk");
		eksplozijaZvuk = GetNode<AudioStreamPlayer>("EksplozijaZvuk");
		pozadinaMuzika = GetNode<AudioStreamPlayer>("PozadinaMuzika");
		mrtavZvuk = GetNode<AudioStreamPlayer>("MrtavZvuk");

		// Pokreni pozadinsku muziku
		pozadinaMuzika.Stream = GD.Load<AudioStream>("res://sounds/pozadina.wav");
		pozadinaMuzika.Play();
	}

	// Pusti zvuk eksplozije
	public void PustiZvukEksplozije()
	{
		eksplozijaZvuk.Stream = GD.Load<AudioStream>("res://sounds/eksplozija.wav");
		eksplozijaZvuk.Play();
	}

	// Pusti zvuk pucanja
	public void PustiZvukPucanja()
	{
		pucanjZvuk.Stream = GD.Load<AudioStream>("res://sounds/pucanj.wav");
		pucanjZvuk.Play();
	}

	// Pusti pozadinsku muziku (moguće pozvati ponovo da se pokrene)
	public void PustiPozadinskuMuziku()
	{
		pozadinaMuzika.Stream = GD.Load<AudioStream>("res://sounds/pozadina.wav");
		pozadinaMuzika.Play();
	}

	// Zaustavi pozadinsku muziku
	public void ZaustaviPozadinskuMuziku()
	{
		pozadinaMuzika.Stop();
	}
}
