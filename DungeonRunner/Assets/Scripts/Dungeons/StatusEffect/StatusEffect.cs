using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SynodicArc.DungeonRunner.StatusEffects{
	/// Abstract class that all status effects will inherit from
	public abstract class StatusEffect : MonoBehaviour {

		#region Enums
		public enum ActiveEffects{
			//Movement
			SpeedBoost,
			SpeedSlow,
			//Jumping
			GravityUp,
			GravityDown,
			//Time Scale
			TimeSpeedUp,
			TimeSlowdown,
		}

		public enum PassiveEffects{
			SpeedUp,
			SpeedDown,
			//Features
			DoubleJump,
		}
		#endregion Enums

		#region Abstract/Virtual Fields
		public abstract string StatusEffectName{ get; set; } //The name of the status effect



		#endregion Abstract/Virtual Fields

		#region Functions
		/// Starts the status effect.
		public virtual void StartStatusEffect(PlayerParameters userParameters){

		}

		/// Ends the status effect.
		public virtual void EndStatusEffect(){

		}

		#endregion Functions

	}
}
