using UnityEngine;
using System.Collections;

namespace SynodicArc.DungeonRunner.StatusEffects{
	/// Active status effect that are used from an item or other activation.
	public abstract class ActiveStatusEffect : StatusEffect {

		#region Abstract/Virtual Fields
		public ActiveStatusEffectNames StatusEffectName; //The name of the status effect
		//public bool Timed = true; //Is it a timed status effect?
		public float Duration = 1.5f; //Duration of status effect if it is timed
		public int Steps = 3; //Number of uses in one go before the cooldown starts again; an alternative to Duration
		#endregion Abstract/Virtual Fields

	}
}
