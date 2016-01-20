using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics; //Conflicts with Debug.Log, so we need to use UnityEngine.Debug.Log in this file
using SynodicArc.DungeonRunner.Items;
using SynodicArc.DungeonRunner.StatusEffects;

public class PlayerParameters : MonoBehaviour{

	#region Constants
	public const int KNOCKBACK = 8;

	#endregion Constants

	//References to other game and UI objects
	private HealthManagement healthManagement;
	private DungeonCanvasRefs dungeonCanvasRefs; //gives us references to our Dungeon Canvas UI


	//Parameters
	public double InvincibilityTime;
	public Stopwatch InvincibilityStopwatch;
	public bool Invincible = false;

	public int MaxPlayerHealth; //Max Player Health
	public int PlayerHealth; //Current Player Health

	private static bool currentlyInvisible = false; //Invisiblity state for taking damage

	//Player Inventory + Items
	public GameObject ItemObjectPrefab; //prefab used for spawning items into the UI
	public List<Item> PlayerInventory; //The items the player currently has in their inventory
	[HideInInspector]
	public List<ItemObject> ActiveItems; //List of active item objects we've put in the UI -- must be public for Android




	#region SetUp
	// Use this for initialization
	void Awake () {
		healthManagement = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<HealthManagement> ();
		dungeonCanvasRefs = GameObject.FindGameObjectWithTag ("DungeonCanvas").GetComponent<DungeonCanvasRefs> ();
		InvincibilityStopwatch = new Stopwatch ();
		MaxPlayerHealth = DataContainer.maxPlayerHealth;
		PlayerHealth = DataContainer.maxPlayerHealth;
		InvincibilityTime = 1 + DataContainer.additionalInvincibilityTime;
		//Set Up Items in Inventory and apply 
		SetUpItems();
	}

	/// Sets up both passive and active items that are in the player's inventory
	void SetUpItems(){
		foreach (Item itm in PlayerInventory) {
			if (itm.Type == Item.ItemType.Active) {
				AddActiveItem (itm); //Add the item to the on-screen area
			} else if (itm.Type == Item.ItemType.Passive) {
				itm.UseItem (this); //Simply use the item, passing in this instance, since it's a passive type
			} else {
				UnityEngine.Debug.Log ("<color=red><b>Invalid Item Type when setting up items.</b></color>");
			}
		}
	}

	/// Adds the active item to the on screen UI
	void AddActiveItem(Item itm){
		ItemObject itmObj = (Instantiate (ItemObjectPrefab)).GetComponent<ItemObject> (); //Instantiates the object itself
		itmObj.SetUp(this, itm, ActiveItems.Count); //Sets up the UI element. Passes in this instance and the item we are assigning to the UI object as well as what number item this is
		itmObj.transform.SetParent (dungeonCanvasRefs.ItemsPanel.transform, false); //Sets location in UI
		itmObj.transform.SetAsFirstSibling(); //This moves up the item as we instantiate more. This way, the 1st created is at the bottom.
		ActiveItems.Add (itmObj); //Add to active item list
	}
	#endregion SetUp


	#region Functions
	//Inflict damage, apply invincibility and status effects
	public void InflictDamage(int dmg, List<StatusEffect> inflictedStatusEffects){
		Invincible = true;
		PlayerHealth -= dmg;
		healthManagement.UpdateHealthVisible ();

		if (PlayerHealth <= 0) {
			UnityEngine.Debug.Log("Player Health reached 0. Now ending game.");
			healthManagement.DisplayDeath();
			Time.timeScale = 0;
		}

		if (PlayerHealth > MaxPlayerHealth) {
			UnityEngine.Debug.Log ("Player Health exceeded max health.");
			PlayerHealth = MaxPlayerHealth;
		}

		//Start our invincibility timer by resetting it to 0 and getting back into it
		InvincibilityStopwatch.Reset ();
		InvincibilityStopwatch.Start ();

		StartCoroutine (FlashingEffect(0.20f));
		//Knockback effect. Needs some work. If you guys wanna look into this, please do.
	//	transform.position = new Vector3(Mathf.Lerp (
	//		transform.position.x,
	//		transform.position.x - KNOCKBACK,
	//		0.5f), transform.position.y, transform.position.z);
		UnityEngine.Debug.Log ("Rendering invisible!");
	}

	/// Performs the flashing effect on taking damage
	public IEnumerator FlashingEffect(float waitTime){
		//If you are currently invisible, go un-invisible, and vice versa
		if (currentlyInvisible) {
			this.GetComponent<SpriteRenderer> ().color = (Color32) ColorControls.HexToColor("FFFFFFFF");
			currentlyInvisible = false;
		} else {
			this.GetComponent<SpriteRenderer> ().color = (Color32) ColorControls.HexToColor("FFFFFF11");
			currentlyInvisible = true;
		}

		//UnityEngine.Debug.Log ("Stopwatch: " + TimeSpanUtil.ConvertMillisecondsToSeconds (invincibilityStopwatch.ElapsedMilliseconds));
		//UnityEngine.Debug.Log ("Invincibility Time: " + invincibilityTime);

		//If our invincibility time is up, then we turn ourselves back to full color and vulnerable again
		if (TimeSpanUtil.ConvertMillisecondsToSeconds(InvincibilityStopwatch.ElapsedMilliseconds) > InvincibilityTime) {
			UnityEngine.Debug.Log ("Now ending invincibility.");
			Invincible = false;
			this.GetComponent<SpriteRenderer> ().color = (Color32) ColorControls.HexToColor("FFFFFFFF");
			currentlyInvisible = false;
		} else {
			yield return new WaitForSeconds(waitTime);
			StartCoroutine (FlashingEffect(0.20f));
		}
	}


	//Apply status effect. Ideally, we will create a StatusEffect object class at some point to handle this instead of string passing
	public void ApplyStatusEffect(){

	}

	#endregion Functions
}
