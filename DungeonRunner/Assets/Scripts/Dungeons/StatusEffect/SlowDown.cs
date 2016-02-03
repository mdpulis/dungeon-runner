using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SynodicArc.DungeonRunner.StatusEffects{
	/// Causes the player to slow down by an amount
	public class SlowDown : ActiveStatusEffect {

		public float SpeedReduction; //the amount we slow down by
		public float MinSpeedReduction; //the minimum amount we can slow down by
		public float MaxSpeedReduction; //the maximum amount we can slow down by

		/// Uses the status effect
		public override void StartStatusEffect(PlayerParameters player){
			Debug.Log ("<color=cyan>Using </color>" + StatusEffectName.ToString () + " on " + player.name + "!");

			if (SpeedReduction < MinSpeedReduction || SpeedReduction > MaxSpeedReduction) {
				Debug.Log ("<color=red><b>SpeedReduction is not within acceptable range. Setting to halfway point.</b></color>");
				SpeedReduction = (MinSpeedReduction + MaxSpeedReduction) / 2;
			}

			player.ApplyStatusEffect (this);
			player.GetComponent<PlayerMovement> ().SlowDown (SpeedReduction, Duration);
		}
	}
}
