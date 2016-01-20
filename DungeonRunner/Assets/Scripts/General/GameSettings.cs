using UnityEngine;
using System.Collections;

/// General game settings that handle things in the game.
public static class GameSettings {

	#region Enums
	/// Different control types
	public enum ControlTypes{
		Touch,
		Keyboard,
		Controller,
	}

	/// Different console types for both console porting and UI preference display
	public enum ConsoleTypes{
		PlayStation3,
		PlayStation4,
		Xbox360,
		XboxOne,
		WiiU,
	}
	#endregion Enums


	/// The control type we are using.
	public static ControlTypes ControlType = ControlTypes.Keyboard;
	/// The type of console we are on and/or the UI type we are using.
	public static ConsoleTypes ConsoleType = ConsoleTypes.PlayStation4;
}
