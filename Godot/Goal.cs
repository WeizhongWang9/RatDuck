using CardGame.Game;
using CardGame.Game.GameEvents;
using CardGame.Game.GameTerms.Abilities;
using Godot;
using System;

public partial class Goal : Control
{
	Game game;
	GameTrigger<ChaoticMeasureAdd> updateUnrestTrig;
	GameTrigger<ScientistResearch> updateBioTrig;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		game = Global.getGame();
		updateUnrestTrig = new GameTrigger<ChaoticMeasureAdd>(updateGameUnrestLevel, CONST_ABILITY_CHAOTICMEASURE.WEIGHT + 1);
		updateBioTrig = new GameTrigger<ScientistResearch>(updateBioSciProgess, 1);
		updateGameUnrestLevel(null);
		updateBioSciProgess(null);

	}

	[Export]
	Label game_unrest_level;
	[Export]
	Label bio_sci_progess;
	[Export]
	Label winText;



	void update_win_text()
	{
		var winner = game.endGame.getWinner();
		switch (winner)
		{
			case EndGame.WINS.DISS:
				winText.Text = "Dissenters win!";
				winText.Visible = true;
				break;
			case EndGame.WINS.GOV:
				winText.Text = "Governments win!";
				winText.Visible = true;
				break;
			case EndGame.WINS.TIE:
				winText.Text = "Tie!";
				winText.Visible = true;
				break;
			default:
				winText.Text = "No winner";
				break;
		}
	}
	void updateGameUnrestLevel(ChaoticMeasureAdd @event)
	{
		game_unrest_level.Text = "Total Cival Unrest Level : " + game.map.getTotalChaoticMeasure() + " / " + EndGame.CONST_ENDGAME.DISS_WIN_NEEDED;
		update_win_text();


	}
	void updateBioSciProgess(ScientistResearch @event)
	{
		bio_sci_progess.Text = "Bio-Sci Progess : " + game.map.bioProgress + " / " + EndGame.CONST_ENDGAME.GOV_WIN_BIO_NEEDED;
		update_win_text();


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
