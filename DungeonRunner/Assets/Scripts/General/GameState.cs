using UnityEngine;
using System.Collections;

/// Handles the current state of the game. I.E. in a race, paused, etc.
public static class GameState {

	#region Enums
	/// Different states of the game
	public enum States{
		Loading, //Loading the game
		MainMenu, //Somewhere in the main menu
		Dungeon, //In dungeon
		Results, //Results screen

	}

	/// Different states in the dungeon
	public enum DungeonStates{
		NotInDungeon, //Not in the dungeon right now
		Running, //Running
		Boss, //Boss at end of dungeon
		Puzzle, //Puzzle at end of dungeon
		Transition, //Transition state between dungeons when you're choosing the next dungeon of your choice
	}
	#endregion Enums

	/// Are we paused right now?
	public static bool Paused = false;
	/// What state are we in (game-wide) right now?
	public static States State = States.Dungeon;
	/// What dungeon state are we in right now?
	public static DungeonStates DungeonState = DungeonStates.Running;
}
