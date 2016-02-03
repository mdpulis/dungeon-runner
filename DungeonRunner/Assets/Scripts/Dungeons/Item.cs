using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SynodicArc.DungeonRunner.StatusEffects;

namespace SynodicArc.DungeonRunner.Items{
	/// The class dealing with items, their images, status effects, and so forth.
	public class Item : MonoBehaviour {

		#region Enum Declaration
		/// Is this item active or passive?
		public enum ItemType{
			Active, //Active-use as button during gameplay
			Passive, //Passive-active at all times
		}

		/// When this item is used, based on what does the green meter go down?
		public enum UseCooldownType{
			None,
			Timed,
			UseCount,
		}

//		/// What kind of effect does this item have?
//		public enum ItemEffect{
//			PlayerModifier, //Modifies player parameters
//			FieldModifier, //Modifies some kind of parameter(s) on the field
//			Scoring, //Purely modifies scoring in some manner
//			Aesthetics, //Modifies aesthetics. Probably will remove this one or move to separate category
//
//		}
		#endregion Enum Declaration


		//General parameters
		public string ItemName; //The item's name
		public Sprite ItemSprite; //The sprite that visualizes this item
		public ItemType Type; //Is the item active or passive?
		public UseCooldownType UseCooldown; //What kind of use cooldown does this item have?
		public StatusEffect PrimaryItemStatusEffect; //The primary status effect as well as what affects the green cooldown bar is based on
		public List<StatusEffect> AdditionalItemStatusEffects; //Any extra status effects this item produces

		//Active parameters
		public float Cooldown = 10.0f; //How long the cooldown is until we can use the item again

		//Passive parameters



		//public ItemEffect Effect; //What effect does this item have?


		#region Functions
		/// Uses the current item.
		public void UseItem(PlayerParameters player){
			Debug.Log ("<color=cyan>Using item: </color>" + this.ItemName);
			//Use the main status effect on the item
			if (PrimaryItemStatusEffect != null) {
				PrimaryItemStatusEffect.StartStatusEffect (player);
			}
			//Use any additional status effects this item has
			foreach (StatusEffect se in AdditionalItemStatusEffects) {
				se.StartStatusEffect (player); //start the status effect on the user
			}
		}

		#endregion Functions


	}
}
