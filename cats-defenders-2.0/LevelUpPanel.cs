using Godot;
using System;

public class LevelUpPanel : CanvasLayer
{
	private Panel levelUpPanel;
	private Button nastaviButton;

	public override void _Ready()
	{
		// Pronađi UI elemente iz scene
		levelUpPanel = GetNode<Panel>("LevelUpPanel");
		nastaviButton = GetNode<Button>("LevelUpPanel/NastaviButton");

		// Sakrij panel u početku
		levelUpPanel.Visible = false;

		// Poveži signal za dugme
		nastaviButton.Connect("pressed", this, nameof(OnNastaviPressed));
	}

	private void OnNastaviPressed()
	{
		// Nastavi igru i sakrij panel
		levelUpPanel.Visible = false;
		GetTree().Paused = false;
	}

	public void ShowLevelUp()
	{
		// Prikaži Level Up panel i pauziraj igru
		levelUpPanel.Visible = true;
		GetTree().Paused = true;
	}
}
