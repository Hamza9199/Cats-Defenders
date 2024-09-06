using Godot;
using System;

public class Boss : KinematicBody2D
{
	[Export] public int Health = 20; // Boss ima 20 zdravlja

	public override void _Ready()
	{
		// Postavljanje početne pozicije Bossa (centar ekrana, iznad vidljivog dijela)
		Position = new Vector2(GetViewportRect().Size.x / 2, -150);

		// Možeš učitati teksturu ako koristiš TextureRect ili Sprite
		var bossSprite = GetNode<Sprite>("BossSprite");
		bossSprite.Texture = (Texture)GD.Load("res://boss.jpg");
	}

	public override void _Process(float delta)
	{
		// Kretanje Bossa prema dolje
		Position += new Vector2(0, 100 * delta); // 100 piksela po sekundi

		// Provjera sudara sa igračem
		var igrac = GetNode<Player>("../Player");
		if (igrac != null && igrac.GetRect().Intersects(GetRect()))
		{
			// Ako Boss dotakne igrača, kraj igre
			GetTree().ChangeScene("res://GameOver.tscn");
		}
	}

	// Funkcija za primanje štete
	public void PrimiStetu()
	{
		Health -= 1;
		if (Health <= 0)
		{
			// Ako je Boss poražen
			QueueFree(); // Uklanja Bossa iz igre
			var main = GetNode<Main>("../Main");
			main.levelN++;
			main.PromijeniLevel(main.levelN); // Prelaz na sljedeći nivo
		}
	}
}
