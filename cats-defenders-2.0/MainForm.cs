using Godot;
using System;
using System.Collections.Generic;

public class MainForm : Node
{
	// Definisanje varijabli za igru
	private Sprite igrac;
	private int igracBrzina = 40;
	private List<Sprite> meci = new List<Sprite>();
	private int meciBrzina = 40;
	private Timer gameTimer;
	private List<Sprite> Neprijatelj = new List<Sprite>();
	private int neprijateljBrzina = 1;
	private Timer NeprijateljTimer;
	private int bodovi = 0;
	private Label bodoviLabel;
	private Label pauzirajIgru;
	private bool PauzirajIgru = false;
	private Panel MenuPanel;
	private Label nastaviLabel;
	private Label ugasiLabel;
	private Label menuText;
	private int zivoti = 5;
	private Label zivotText;
	private Label level;
	private int levelN = 1;
	private int neprijateljUbijeniCount = 0;
	private AudioStreamPlayer pucanjZvuk;
	private AudioStreamPlayer eksplozijaZvuk;
	private AudioStreamPlayer pozadinaMuzika;
	private AudioStreamPlayer mrtavZvuk;
	private Panel LevelUP;
	private Boss boss = null;
	private string basePath = "";

	public override void _Ready()
	{
		basePath = OS.GetExecutablePath().GetBaseDir(); // Postavi baznu putanju
		PostaviIgru();
		PostaviPozadinskuSliku();
		InicijalizirajZvukove();
		PostaviMenuPanel();
		PostaviLevelUpPanel();

		gameTimer = GetNode<Timer>("GameTimer");
		gameTimer.Connect("timeout", this, nameof(GameTimer_Tick));
		gameTimer.Start();
	}

	private void PostaviIgru()
	{
		// Inicijalizacija igre
	}

	private void PostaviPozadinskuSliku()
	{
		// Postavljanje pozadinske slike
	}

	private void InicijalizirajZvukove()
	{
		pucanjZvuk = new AudioStreamPlayer();
		eksplozijaZvuk = new AudioStreamPlayer();
		pozadinaMuzika = new AudioStreamPlayer();
		mrtavZvuk = new AudioStreamPlayer();

		AddChild(pozadinaMuzika);
		AddChild(pucanjZvuk);
		AddChild(eksplozijaZvuk);
		AddChild(mrtavZvuk);

		pozadinaMuzika.Stream = GD.Load<AudioStream>(basePath + "pozadina.wav");
		pozadinaMuzika.Play();
	}

	private void PostaviMenuPanel()
	{
		// Postavljanje menija
	}

	private void PostaviLevelUpPanel()
	{
		// Postavljanje Level UP panela
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Scancode == (uint)KeyList.Left && igrac.Position.x > 0)
			{
				igrac.Position -= new Vector2(igracBrzina, 0);
			}
			if (keyEvent.Scancode == (uint)KeyList.Right && igrac.Position.x < GetViewport().Size.x)
			{
				igrac.Position += new Vector2(igracBrzina, 0);
			}
			if (keyEvent.Scancode == (uint)KeyList.Space)
			{
				Pucaj();
				IgrajZvuk(basePath + "pucanj.wav");
			}
			if (keyEvent.Scancode == (uint)KeyList.Escape)
			{
				TogglePause();
			}
			if (keyEvent.Scancode == (uint)KeyList.Enter && LevelUP.Visible)
			{
				IgraPostavke.nastaviIgru(this);
			}
		}
	}

	public void Pucaj()
	{
		try
		{
			Sprite metak = new Sprite
			{
				Texture = GD.Load<Texture>(basePath + "metak.png"),
				Position = new Vector2(igrac.Position.x + igrac.Texture.GetSize().x / 2 - 2, igrac.Position.y)
			};
			AddChild(metak);
			meci.Add(metak);
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Greška prilikom pucanja: {ex.Message}");
		}
	}

	private void GameTimer_Tick()
	{
		MoveMeci();
		MoveNeprijatelji();
		CheckBackgroundMusic();
		Neprijatelji.ProvjeriSudare(this);
		levelFollow();

		// Pojava Bossa na trećem nivou
		if (levelN == 3 && boss == null)
		{
			boss = new Boss(this);
			AddChild(boss.BossPictureBox);
		}

		// Ako je Boss prisutan, kontroliraj njegovo kretanje
		if (boss != null)
		{
			boss.Kretanje(this);

			// Provjera sudara metka sa Bossom
			for (int i = meci.Count - 1; i >= 0; i--)
			{
				Sprite metak = meci[i];
				if (metak.GetRect().Intersects(boss.BossPictureBox.GetRect()))
				{
					RemoveChild(metak);
					meci.Remove(metak);
					boss.PrimiStetu(this);
				}
			}
		}
	}

	private void MoveMeci()
	{
		for (int i = meci.Count - 1; i >= 0; i--)
		{
			Sprite metak = meci[i];
			metak.Position -= new Vector2(0, meciBrzina);
			if (metak.Position.y < 0)
			{
				RemoveChild(metak);
				meci.Remove(metak);
			}
		}
	}

	private void MoveNeprijatelji()
	{
		for (int i = Neprijatelj.Count - 1; i >= 0; i--)
		{
			Sprite nevzo = Neprijatelj[i];
			nevzo.Position += new Vector2(0, neprijateljBrzina);

			AdjustEnemySpeed();

			if (nevzo.Position.y > GetViewport().Size.y || nevzo.GetRect().Intersects(igrac.GetRect()))
			{
				HandleEnemyHit(nevzo);
			}
			if (neprijateljUbijeniCount == 210)
			{
				EndGame("Pobjedili ste!");
			}
		}
	}

	private void AdjustEnemySpeed()
	{
		if (levelN == 3)
		{
			neprijateljBrzina = 2;
		}
		else if (levelN == 6)
		{
			neprijateljBrzina = 3;
		}
		else if (levelN == 10)
		{
			neprijateljBrzina = 4;
		}
	}

	private void HandleEnemyHit(Sprite nevzo)
	{
		RemoveChild(nevzo);
		Neprijatelj.Remove(nevzo);
		zivoti -= 1;
		zivotText.Text = $"Zivoti: {zivoti}";
		if (zivoti == 0)
		{
			EndGame("Izgubili ste!");
		}
	}

	private void CheckBackgroundMusic()
	{
		if (pozadinaMuzika.PlaybackState == AudioStreamPlayer.PlaybackState.Stopped)
		{
			pozadinaMuzika.Stream = GD.Load<AudioStream>(basePath + "pozadina.wav");
			pozadinaMuzika.Play();
		}
	}

	public void DodajNevzu()
	{
		Neprijatelji.DodajNeprijatelja(this);
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

	public void PromijeniLevel(int noviLevel)
	{
		neprijateljUbijeniCount++;
		level.Text = $"Level: {noviLevel}";
		gameTimer.Stop();
		NeprijateljTimer.Stop();
		ClearEnemies();
		if (noviLevel != 3 || boss == null) // Boss se kreira samo jednom na trećem nivou
		{
			LevelUP.Visible = true;
		}
		pauzirajIgru.Visible = false;
		levelN++;
	}

	private void ClearEnemies()
	{
		foreach (var nevzo in Neprijatelj)
		{
			RemoveChild(nevzo);
			nevzo.QueueFree();
		}
		Neprijatelj.Clear();
	}

	public void EndGame(string poruka)
	{
		if (LevelUP.Visible == false)
		{
			GD.Print(poruka);
		}
		else
		{
			GD.Print(poruka);
			GetTree().Quit();
		}
	}

	private void TogglePause()
	{
		PauzirajIgru = !PauzirajIgru;
		gameTimer.Stop();
		NeprijateljTimer.Stop();
		pauzirajIgru.Visible = PauzirajIgru;
	}

	private void IgrajZvuk(string zvuk)
	{
		pucanjZvuk.Stream = GD.Load<AudioStream>(zvuk);
		pucanjZvuk.Play();
	}
}
