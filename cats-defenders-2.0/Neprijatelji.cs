using Godot;
using System;
using System.Collections.Generic;

public class Neprijatelji : Node2D
{
	private PackedScene neprijateljScene;
	private Random random = new Random();

	// Lista neprijatelja
	private List<Node2D> neprijatelji = new List<Node2D>();

	public override void _Ready()
	{
		// Učitavanje scene neprijatelja
		neprijateljScene = (PackedScene)ResourceLoader.Load("res://Neprijatelj.tscn");
	}

	// Dodaje novog neprijatelja na ekran
	public void DodajNeprijatelja()
	{
		// Provjera da li boss već postoji
		bool bossPostoji = neprijatelji.Exists(n => n.Name == "Boss");

		if (!bossPostoji)
		{
			Node2D neprijatelj = (Node2D)neprijateljScene.Instance();
			int xPosition = random.Next(0, (int)GetViewportRect().Size.x - 50);  // Nasumična pozicija neprijatelja
			neprijatelj.Position = new Vector2(xPosition, -50);

			AddChild(neprijatelj);
			neprijatelji.Add(neprijatelj);
		}
	}

	// Provjerava sudare između metaka i neprijatelja
	public void ProvjeriSudare(List<Node2D> meci)
	{
		if (meci.Count == 0 || neprijatelji.Count == 0)
			return;

		foreach (var metak in meci.ToArray())
		{
			foreach (var neprijatelj in neprijatelji.ToArray())
			{
				// Provjera sudara
				if (metak.GetRect().Intersects(neprijatelj.GetRect()))
				{
					meci.Remove(metak);
					neprijatelji.Remove(neprijatelj);

					metak.QueueFree();
					neprijatelj.QueueFree();

					// Povećaj bodove i broj ubijenih neprijatelja
					GetNode<Main>("Main").PovecajBodove();
					GetNode<Main>("Main").neprijateljUbijeniCount++;

					// Zvuk eksplozije
					Zvuk.PustiZvukEksplozije();
					break;
				}
			}
		}
	}
}
