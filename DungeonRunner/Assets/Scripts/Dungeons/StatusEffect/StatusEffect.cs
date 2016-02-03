using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SynodicArc.DungeonRunner.StatusEffects{
	/// Abstract class that all status effects will inherit from
	public abstract class StatusEffect : MonoBehaviour{

//		#region Enums
//		public enum ActiveEffects{
//			//Movement
//			SpeedBoost,
//			SpeedSlow,
//			//Jumping
//			GravityUp,
//			GravityDown,
//			//Time Scale
//			TimeSpeedUp,
//			TimeSlowdown,
//		}
//
//		public enum PassiveEffects{
//			SpeedUp,
//			SpeedDown,
//			//Features
//			DoubleJump,
//		}
//		#endregion Enums

		#region Functions
		/// Starts the status effect.
		public virtual void StartStatusEffect(PlayerParameters player){
			player.ApplyStatusEffect (this);
		}

		#endregion Functions

	}
}
