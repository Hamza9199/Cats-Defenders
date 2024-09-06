using Godot;
using System;

public class Main : Node2D
{
	private Label bodoviLabel;
	private Label pauzirajIgru;
	private Label zivotText;
	private Label level;
	private Sprite igrac;
	private Timer gameTimer;
	private Timer neprijateljTimer;
	private AudioStreamPlayer pozadinaMuzika;
	private AudioStreamPlayer pucanjZvuk;
	private AudioStreamPlayer eksplozijaZvuk;
	private AudioStreamPlayer mrtavZvuk;

	private int igracBrzina = 40;
	private int meciBrzina = 40;
	private int neprijateljBrzina = 1;
	private int bodovi = 0;
	private int zivoti = 5;
	private int levelN = 1;
	private int neprijateljUbijeniCount = 0;
	private bool pauzirajIgru = false;

	private PackedScene neprijateljScene;
	private Node2D neprijateljiContainer;
	private Node2D meciContainer;

	public override void _Ready()
	{
		// Postavljanje pozadine, slike igrača i osnovnih labela
		SetupUI();

		// Postavljanje timera
		SetupTimers();

		// Postavljanje zvukova
		SetupSounds();

		// Postavljanje neprijatelja
		neprijateljScene = GD.Load<PackedScene>("res://Neprijatelj.tscn");
		neprijateljiContainer = new Node2D();
		AddChild(neprijateljiContainer);
		
		meciContainer = new Node2D();
		AddChild(meciContainer);
	}

	private void SetupUI()
	{
		// Kreiraj i dodaj UI elemente
		bodoviLabel = new Label
		{
			Text = "Bodovi: 0",
			Position = new Vector2(10, 10),
			Theme = GetTheme()
		};
		AddChild(bodoviLabel);

		pauzirajIgru = new Label
		{
			Text = "Menu",
			Position = new Vector2(GetViewport().Size.x - 100, 10),
			MouseFilter = MouseFilterEnum.Stop,
			Theme = GetTheme()
		};
		pauzirajIgru.Connect("gui_input", this, nameof(OnPauzaClick));
		AddChild(pauzirajIgru);

		zivotText = new Label
		{
			Text = "Životi: 5",
			Position = new Vector2(600, 10),
			Theme = GetTheme()
		};
		AddChild(zivotText);

		level = new Label
		{
			Text = "Level: 1",
			Position = new Vector2(10, 30),
			Theme = GetTheme()
		};
		AddChild(level);

		igrac = new Sprite
		{
			Texture = GD.Load<Texture>("res://igrac.png"),
			Position = new Vector2(GetViewport().Size.x / 2 - 25, GetViewport().Size.y - 60)
		};
		AddChild(igrac);
	}

	private void SetupTimers()
	{
		gameTimer = new Timer
		{
			WaitTime = 0.02f,
			OneShot = false
		};
		gameTimer.Connect("timeout", this, nameof(OnGameTimerTick));
		AddChild(gameTimer);
		gameTimer.Start();

		neprijateljTimer = new Timer
		{
			WaitTime = 2.5f,
			OneShot = false
		};
		neprijateljTimer.Connect("timeout", this, nameof(OnNeprijateljTimerTick));
		AddChild(neprijateljTimer);
		neprijateljTimer.Start();
	}

	private void SetupSounds()
	{
		pozadinaMuzika = new AudioStreamPlayer
		{
			Stream = GD.Load<AudioStream>("res://pozadina.ogg")
		};
		AddChild(pozadinaMuzika);
		pozadinaMuzika.Play();

		pucanjZvuk = new AudioStreamPlayer
		{
			Stream = GD.Load<AudioStream>("res://pucanj.ogg")
		};
		AddChild(pucanjZvuk);

		eksplozijaZvuk = new AudioStreamPlayer
		{
			Stream = GD.Load<AudioStream>("res://eksplozija.ogg")
		};
		AddChild(eksplozijaZvuk);

		mrtavZvuk = new AudioStreamPlayer
		{
			Stream = GD.Load<AudioStream>("res://mrtav.ogg")
		};
		AddChild(mrtavZvuk);
	}

	private void OnGameTimerTick()
	{
		MoveMeci();
		MoveNeprijatelji();
		CheckBackgroundMusic();
		levelFollow();
	}

	private void OnNeprijateljTimerTick()
	{
		DodajNevzu();
	}

	private void OnPauzaClick(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			TogglePause();
		}
	}

	private void MoveMeci()
	{
		// Implementacija kretanja metaka
	}

	private void MoveNeprijatelji()
	{
		// Implementacija kretanja neprijatelja
	}

	private void CheckBackgroundMusic()
	{
		if (pozadinaMuzika.PlaybackPosition == pozadinaMuzika.Stream.GetLength())
		{
			pozadinaMuzika.Play();
		}
	}

	private void DodajNevzu()
	{
		var neprijatelj = neprijateljScene.Instance() as Node2D;
		neprijateljiContainer.AddChild(neprijatelj);
	}

	private void levelFollow()
	{
		int[] thresholds = { 10, 31, 52, 73, 94, 115, 136, 157, 178 };
		for (int i = 0; i < thresholds.Length; i++)
		{
			if (neprijateljUbijeniCount == thresholds[i])
			{
				PromijeniLevel(i + 2);
				break;
			}
		}
	}

	private void PromijeniLevel(int noviLevel)
	{
		neprijateljUbijeniCount++;
		level.Text = $"Level: {noviLevel}";
		gameTimer.Stop();
		neprijateljTimer.Stop();
		ClearEnemies();
		if (noviLevel != 3 || boss == null) // Boss se kreira samo jednom na trećem nivou
		{
			// Prikaži Level Up UI
		}
		pauzirajIgru = false;
	}

	private void ClearEnemies()
	{
		neprijateljiContainer.QueueFree();
	}

	private void EndGame(string message)
	{
		gameTimer.Stop();
		neprijateljTimer.Stop();
		pozadinaMuzika.Stop();
		mrtavZvuk.Play();
		GD.Print($"{message} Osvojili ste {bodovi} bodova.");
		GetTree().Restart();
	}

	private void TogglePause()
	{
		if (!pauzirajIgru)
		{
			// Pause game
			gameTimer.Stop();
			neprijateljTimer.Stop();
			pozadinaMuzika.Paused = true;
			// Show pause menu
			pauzirajIgru = true;
		}
		else
		{
			// Resume game
			gameTimer.Start();
			neprijateljTimer.Start();
			pozadinaMuzika.Paused = false;
			// Hide pause menu
			pauzirajIgru = false;
		}
	}
}
