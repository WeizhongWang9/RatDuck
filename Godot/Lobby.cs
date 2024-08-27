using CardGame.Game;
using CardGame.Game.GameTerms;
using CardGame.Game.GameTerms.Units;
using Godot;
using RatDuck.Script.Godot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static RatDuck.Script.Godot.GodotGlobal;

public partial class Lobby : Control
{

	Global _global;
	TabBar playingTabBar;
	Panel displayPanel;
	List<CharacterBody2D> characterDisplay = new List<CharacterBody2D>();
	GodotGlobal godotGlobal;
	List<GodotGlobal.PlayerInfo> playerInfos;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_global = Global.global;
		godotGlobal = GodotGlobal.godotGlobal;
		playerInfos = godotGlobal.playerInfos;
		setSignal();

		playerInfos.Add(new PlayerInfo("A", Roles.Police));
		playerTextList.AddItem(getRoleName(Roles.Police) + " " + "A");
		playerInfos.Add(new PlayerInfo("B", Roles.Scientist));
		playerTextList.AddItem(getRoleName(Roles.Scientist) + " " + "B");
		playerInfos.Add(new PlayerInfo("C", Roles.Dissent));
		playerTextList.AddItem(getRoleName(Roles.Dissent) + " " + "C");
		playerInfos.Add(new PlayerInfo("D", Roles.Dissent));
		playerTextList.AddItem(getRoleName(Roles.Dissent) + " " + "D");

	}
	[Export]
	ItemList playerTextList;

	void setSignal()
	{
		var add_player_button = GetNode("Panel/AddPlayer") as Button;
		add_player_button.Pressed += OnButtonPressed;
		var delete_player_button = GetNode("Panel/Delete") as Button;
		delete_player_button.Pressed += _on_remove_player_pressed;
		var game_start_button = GetNode("Panel/Start") as Button;
		game_start_button.Pressed += onStartPressed;
		playingTabBar = GetNode("Panel/PlayingLabel/Playing") as TabBar;
		playingTabBar.TabChanged += OnPlayingTabBarSelected;

		displayPanel = GetNode<Panel>("Panel/PlayingLabel/DisplayPanel");

		for (int i = 0; i < displayPanel.GetChildCount(); i++)
		{
			characterDisplay.Add(displayPanel.GetChild<CharacterBody2D>(i));
			AnimatedSprite2D characterDisplayAnimated = characterDisplay[i].GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			characterDisplayAnimated.Play();
		}
	}

	void replaceCharacter(Character character)
	{
		var index = (int)character;
		for (int i = 0; i < (int)Character.END; i++)
		{
			if (i == index)
			{ characterDisplay[index].Visible = true; }
			else
			{
				characterDisplay[index].Visible = false;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	enum Character { BlueBird,END}
	void OnPlayingTabBarSelected(long tab)
	{
		var tabString = playingTabBar.GetTabTitle((int)tab);
		replaceCharacter((Character)tab);
	}

	void OnButtonPressed()
	{
		var nameInput = GetNode("Panel/Name/NameInput") as LineEdit;
		if (nameInput.Text == null) { return; }
		var roleBar = GetNode("Panel/Name/RolesLabel/Roles") as TabBar;
		var rolesIndex = roleBar.CurrentTab;
		var role = (Roles)rolesIndex;
		playerInfos.Add(new PlayerInfo(nameInput.Text, role)); 
		playerTextList.AddItem(getRoleName(role) + " " + nameInput.Text);
		nameInput.Text = null;
		
	}
	string getRoleName(Roles role)
	{
		var roleBar = GetNode("Panel/Name/RolesLabel/Roles") as TabBar;
		return roleBar.GetTabTitle((int)role);
	}

	void _on_remove_player_pressed()
	{
		var indexSelectedGroup = playerTextList.GetSelectedItems();
		if(indexSelectedGroup.Length == 0)
		{
			return;
		}
		var indexSelected = indexSelectedGroup.First();
		playerInfos.Remove(playerInfos[indexSelected]);
		playerTextList.RemoveItem(indexSelected);
		

	}
	[Export]
	StringName MapScene;
	void onStartPressed()
	{		
		if (playerInfos.Count == 0) { return; }
		GetTree().ChangeSceneToFile(MapScene);

	}
}



