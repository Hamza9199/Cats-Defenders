using Godot;
using System;

public class IgraMeni : CanvasLayer
{
	private Panel menuPanel;
	private Button nastaviButton;
	private Button izadjiButton;

	public override void _Ready()
	{
		// Pronađi UI elemente iz scene
		menuPanel = GetNode<Panel>("MenuPanel");
		nastaviButton = GetNode<Button>("MenuPanel/NastaviButton");
		izadjiButton = GetNode<Button>("MenuPanel/IzadjiButton");

		// Sakrij panel u početku
		menuPanel.Visible = false;

		// Poveži signale (evente) za dugmad
		nastaviButton.Connect("pressed", this, nameof(OnNastaviPressed));
		izadjiButton.Connect("pressed", this, nameof(OnIzadjiPressed));
	}

	private void OnNastaviPressed()
	{
		// Nastavi igru (sakrij meni i nastavi gameplay)
		menuPanel.Visible = false;
		GetTree().Paused = false;
	}

	private void OnIzadjiPressed()
	{
		// Zatvori igru
		GetTree().Quit();
	}

	public void ShowMenu()
	{
		// Prikaži meni i pauziraj igru
		menuPanel.Visible = true;
		GetTree().Paused = true;
	}
}
