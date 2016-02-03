using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SynodicArc.DungeonRunner.StatusEffects{
	/// Causes the player to speed up by an amount
	public class TimeScaleChange : ActiveStatusEffect {

		public float TimeChange; //the amount we speed up by
		public float MinTimeChange = -10.0f; //the minimum amount we can speed up by
		public float MaxTimeChange = 10.0f; //the maximum amount we can speed up by

		/// Uses the status effect
		public override void StartStatusEffect(PlayerParameters player){
			Debug.Log ("<color=cyan>Using </color>" + StatusEffectName.ToString () + " on " + player.name + "!");

			if (TimeChange < MinTimeChange || TimeChange > MaxTimeChange) {
				Debug.Log ("<color=red><b>TimeScale is not within acceptable range. Setting to halfway point.</b></color>");
				TimeChange = (MinTimeChange + MaxTimeChange) / 2;
			}

			player.ApplyStatusEffect (this);
			player.TimeScaleChange (TimeChange, Duration);
		}
	}
}
