using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SynodicArc.DungeonRunner.StatusEffects{
	/// Causes the player to speed up by an amount
	public class SpeedUp : ActiveStatusEffect {

		public float SpeedBoost; //the amount we speed up by
		public float MinSpeedBoost; //the minimum amount we can speed up by
		public float MaxSpeedBoost; //the maximum amount we can speed up by

		/// Uses the status effect
		public override void StartStatusEffect(PlayerParameters player){
			Debug.Log ("<color=cyan>Using </color>" + StatusEffectName.ToString () + " on " + player.name + "!");

			if (SpeedBoost < MinSpeedBoost || SpeedBoost > MaxSpeedBoost || SpeedBoost == 0) {
				Debug.Log ("<color=red><b>SpeedBoost is not within acceptable range. Setting to halfway point.</b></color>");
				SpeedBoost = (MinSpeedBoost + MaxSpeedBoost) / 2;
			}

			player.ApplyStatusEffect (this);
			player.GetComponent<PlayerMovement> ().SpeedUp (SpeedBoost, Duration);
		}
	}
}
