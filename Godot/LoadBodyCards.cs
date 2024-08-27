using Godot;
using Godot.Collections;
using System;
using System.Globalization;
using Newtonsoft.Json;
using System.Collections.Generic;
using RatDuck.Script.Godot;
using static RatDuck.Script.Godot.TokenLabelPreset;
using RatDuck.Script.Game.GameTerms.Units;
public partial class LoadBodyCards : Node
{
	const int totalPlaces = 12;
	GodotGlobal godotGlobal = GodotGlobal.godotGlobal;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		JsonReader<CardInfo> bodyPlaceCardInfos = new(BodyCardsDatabase);
		List<CardInfo> list = new List<CardInfo>();
		for(int i = 0; i < totalPlaces; i++)
		{
			list.Add(bodyPlaceCardInfos.rollTokenLabel(true));
		}
		godotGlobal.tokenLabelPreset.initBodyPlaceCardInfos(list);
	}


	[Export]
	Json BodyCardsDatabase;
	[Export]
	PackedScene packedScene;

	
		

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
