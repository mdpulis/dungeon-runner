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
		public List<StatusEffect> ItemStatusEffects; //The status effects this item produces

		//Active parameters
		public float Cooldown = 10.0f; //How long the cooldown is until we can use the item again

		//Passive parameters



		//public ItemEffect Effect; //What effect does this item have?


		#region Functions
		/// Uses the current item.
		public void UseItem(PlayerParameters userParameters){
			Debug.Log ("<color=cyan>Using item: </color>" + this.ItemName);
			foreach (StatusEffect se in ItemStatusEffects) {
				se.StartStatusEffect (userParameters); //start the status effect on the user
			}
		}

		#endregion Functions


	}
}
