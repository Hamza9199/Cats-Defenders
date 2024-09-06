using Godot;
using System;

public class PauseMenu : Control
{
	private Label menuText;
	private Label continueLabel;
	private Label exitLabel;
	private Button continueButton;
	private Button exitButton;

	public override void _Ready()
	{
		menuText = GetNode<Label>("MenuText");
		continueLabel = GetNode<Label>("ContinueLabel");
		exitLabel = GetNode<Label>("ExitLabel");
		continueButton = GetNode<Button>("ContinueButton");
		exitButton = GetNode<Button>("ExitButton");

		continueButton.Connect("pressed", this, nameof(OnContinuePressed));
		exitButton.Connect("pressed", this, nameof(OnExitPressed));
	}

	private void OnContinuePressed()
	{
		Visible = false;
		// Resume the game
		GetTree().Paused = false;
	}

	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}
