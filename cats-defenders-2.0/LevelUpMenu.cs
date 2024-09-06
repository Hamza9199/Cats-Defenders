using Godot;
using System;

public class LevelUpMenu : Control
{
	private Label levelUpText;
	private Button continueButton;

	public override void _Ready()
	{
		levelUpText = GetNode<Label>("LevelUpText");
		continueButton = GetNode<Button>("ContinueButton");

		continueButton.Connect("pressed", this, nameof(OnContinuePressed));
	}

	private void OnContinuePressed()
	{
		Visible = false;
		// Proceed to the next level or perform necessary actions
	}
}
