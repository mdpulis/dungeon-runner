#region Enums
//Different elevation levels of pieces
public enum Height {
	low,        //on the ground. No crates, but no hole.
	medium,     //around the middle of the screen, slightly above ground level
	high,       //around the top of the screen, like a large stack of crates
	impossible, //meant to mark a piece as "do not spawn"
}
#endregion Enums