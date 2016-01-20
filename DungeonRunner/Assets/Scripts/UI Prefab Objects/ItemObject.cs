using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace SynodicArc.DungeonRunner.Items{
	/// Uses the references to the item button's prefab locations
	public class ItemObject : MonoBehaviour {

		#region Constants
		public const float METER_FILL_CAP = 0.75f; //the meter appears to be maxed out at 0.75f instead of 1.0f
		public const string ALT = "alt"; //the alternate control scheme's string addition
		public const float TIMER_INTERVAL = 5.0f; //How often the timer ticks (in ms)

		//Animator
		public const string FLASH = "Flash";
		public const string USE = "Use";
		#endregion Constants

		//Item Object Inner Panel
		private Button thisItemButton; //this button
		public Image ItemImage; //the image area we will overwrite with our own sprite
		public Animator ItemObjectAnimator; //the animator that handles animations in the ItemObjectInnerPanel
		public Image ItemBackgroundCooldown; //the circular image that covers our item until we can use it again
		public Text CooldownTimerText; //the text that displays how much time we have left to wait until we can use the item again
		public Image ItemMeterFill; //the meter that fills up 

		//Controller Icons Panel
		public GameObject KeyboardImage; //the keyboard image itself
		public Text KeyboardKeyText; //the text for the key we have to press to use the item
		public GameObject ControllerButtonImage; //the image for the controller button if we are in controller mode

		//Item
		[HideInInspector]
		public Item ThisItem; //What item is assigned to this item object?
		[HideInInspector]
		public PlayerParameters ItemUser; //Who is the user/owner of this item?
		[HideInInspector]
		public int ItemNumber; //What number is this item? (that is, is this is the 1st item we've gotten? 2nd? etc.)
		private string itemPressInput = "Item"; //The string of the input for the button we have to press to use the item if not in touch mode
		private bool controllerMode = false; //also includes keyboard mode; off by default, we only set to true once we've instantiated an item in a controller mode
		private bool altControlScheme = false; //are we using the alternate control scheme? NOT YET IMPLEMENTED

		//Item Parameters (Cooldown and so forth)
		private float itemCooldown; //the cooldown that the item has every time
		private float currentCooldown; //the cooldown that the timer is currently dealing with
		private float currentCooldownElapsed; //the time that has currently elapsed in this cooldown
		private Timer cooldownTimer; //timer with Countdown() event that handles stuff
		private bool timerOn; //is the timer currently on? Need to know in case we pause

		#region SetUp
		/// Sets up the item object, assigning the user of the item and taking the item itself.
		/// We also retrieve the number of the item passed in; that is, is this the 3rd item we've gotten? The 1st? 4th? etc.
		public void SetUp(PlayerParameters itmUsr, Item itm, int itmNum){
			//Assignment
			thisItemButton = this.GetComponent<Button>();
			ThisItem = itm;
			ItemUser = itmUsr;
			ItemNumber = itmNum;
			//Parameters assignment
			itemCooldown = ThisItem.Cooldown * 1000.0f; //convert to ms
			cooldownTimer = new Timer (TIMER_INTERVAL); //gets us a new timer 
			cooldownTimer.Elapsed += this.Countdown;

			//Visualization
			if (ThisItem.ItemSprite != null)
				ItemImage.sprite = ThisItem.ItemSprite;
			else
				Debug.Log ("<color=red><b>Missing sprite when initializing </b></color>" + ThisItem.ItemName);

			//Controller/Keyboard icon handling
			switch (GameSettings.ControlType) {
			case(GameSettings.ControlTypes.Touch):
				//Do nothing on touch mode.
				break;
			case(GameSettings.ControlTypes.Keyboard):
				KeyboardKeySetUp ();
				controllerMode = true; //allow controller inputs
				break;
			case(GameSettings.ControlTypes.Controller):
				ControllerIconSetUp ();
				controllerMode = true; //allow controller inputs
				break;
			default:
				Debug.Log ("<color=red><b>Invalid Control Type when setting up item object's visualizations.</b></color>");
				break;
			}
		}


		#endregion SetUp


		#region Update
		/// Lets us handle input presses.
		void Update(){

			#region Pausing
			if(GameState.Paused){
				if(timerOn){
					cooldownTimer.Stop(); //temporarily stop our cooldown timer
				}
				return; //just end here, we don't want to do anything else
			}else if (timerOn){
				if(!GameState.Paused){
					cooldownTimer.Start(); //start up our cooldown timer again once we unpause
				}
			}

			#endregion Pausing

			#region Controller/Keyboard Handling
			//Only allow update to progress if we're on controller mode
			if (controllerMode) {
				if (Input.GetButtonDown (itemPressInput)) {
					ActivateItem ();
				}
			}
			#endregion Controller/Keyboard Handling

			#region Timer On/Fill Amounts/Etc.
			//Can't put a lot of this on the timer since it's not on the main thread, so we do it in Update
			if (timerOn){
				if(currentCooldown < 0){
					cooldownTimer.Stop ();
					Debug.Log ("<color=cyan>Done!</color>");
					ItemBackgroundCooldown.fillAmount = 0.0f;
					CooldownTimerText.text = "";
					ItemObjectAnimator.SetTrigger (FLASH);
					timerOn = false; //the timer is now off! We don't need to keep checking this
				} else {
					ItemBackgroundCooldown.fillAmount = (currentCooldown / itemCooldown); //the fill gets smaller as the timer continues
					//If less than three seconds left, we want to display decimal values
					float timeLeft = itemCooldown - currentCooldownElapsed;
					if(timeLeft < 3010.0f) {
						CooldownTimerText.text = String.Format("{0:0.0}", (timeLeft / 1000.0f)); //show how much time we have left
					} else if (timeLeft > 60000.0f) { //if more than a minute, display time in minutes left
						CooldownTimerText.text = String.Format("{0:D}m", ((int)((timeLeft+1000.0f) / 60000.0f))); //show how much time we have left
					} else {
						CooldownTimerText.text = String.Format("{0:D}", ((int)((timeLeft+1000.0f) / 1000.0f))); //show how much time we have left
					}

				}
			}
			#endregion Timer On/Fill Amounts/Etc.

		}
		#endregion Update


		#region Countdown
		/// Sets up all necessary parameters before starting Countdown()
		public void CountdownSetUp(){
			currentCooldown = itemCooldown; //Set current cooldown to the original cooldown duration (in ms value)
			currentCooldownElapsed = 0.0f; //Reset elapsed timer back to 0 as well
			cooldownTimer.Start (); //Start our timer, going through Countdown() every interval
			timerOn = true; //The timer is now on!
			Debug.Log("<color=cyan>Starting new cooldownTimer!</color>");
		}

		/// The countdown that occurs after using an item 
		public void Countdown(object source, ElapsedEventArgs e){
			currentCooldown -= TIMER_INTERVAL; //Reduce the cooldown by the timer's interval value
			currentCooldownElapsed += TIMER_INTERVAL; //Add to the elapsed timer by the timer's interval value
			//The end of Countdown occurs in Update() due to certain things needing to be accessed on the main thread
		}

		#endregion Countdown


		#region General Functions
		/// Uses the item assigned to this button.
		public void ActivateItem(){
			//Can't activate item if the timer is currently on (failsafe)
			if (!timerOn) {
				ItemObjectAnimator.SetTrigger (USE);
				CountdownSetUp (); //Set up the countdown before we can use the item again and other features
				ThisItem.UseItem (ItemUser); //Use this item, passing the user of the item
			}
		}

		#endregion General Functions


		#region Controller/Keyboard SetUp
		/// Sets up controller input visualization. Sets to the according n'th item's button preference
		void ControllerIconSetUp(){
			//Set the controller button's visualization to true
			ControllerButtonImage.SetActive (true);

			//Now we have to check for specific console types for input visualization
			switch (GameSettings.ConsoleType) {
			case(GameSettings.ConsoleTypes.PlayStation3):
			case(GameSettings.ConsoleTypes.PlayStation4):
				switch (ItemNumber) {
				case(0):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.PLAYSTATION_PATH + ConstantData.PS_SQUARE);
					break;
				case(1):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.PLAYSTATION_PATH + ConstantData.PS_TRIANGLE);
					break;
				case(2):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.PLAYSTATION_PATH + ConstantData.PS_L1);
					break;
				case(3):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.PLAYSTATION_PATH + ConstantData.PS_R1);
					break;
				default:
					Debug.Log ("<color=red><b>Trying to instantiate over 4 items is not allowed.</b></color>");
					break;
				}
				break;
			case(GameSettings.ConsoleTypes.Xbox360):
			case(GameSettings.ConsoleTypes.XboxOne):
				switch (ItemNumber) {
				case(0):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.XBOX_PATH + ConstantData.XBOX_X);
					break;
				case(1):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.XBOX_PATH + ConstantData.XBOX_Y);
					break;
				case(2):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.XBOX_PATH + ConstantData.XBOX_LB);
					break;
				case(3):
					ControllerButtonImage.GetComponent<Image> ().sprite =
						Resources.Load<Sprite> (ConstantData.CONTROLLER_ICON_PATH + ConstantData.XBOX_PATH + ConstantData.XBOX_RB);
					break;
				default:
					Debug.Log ("<color=red><b>Trying to instantiate over 4 items is not allowed.</b></color>");
					break;
				}
				break;
			case(GameSettings.ConsoleTypes.WiiU):
				Debug.Log ("<color=red><b>Wii U not implemented.</b></color>");
				break;
			default:
				Debug.Log ("<color=red><b>Invalid console type in item object's visualization.</b></color>");
				break;
			}

			//Failsafe checking to make sure we don't have over the max number of items when we add an input
			if (ItemNumber < ConstantData.MAX_ACTIVE_ITEMS) {
				itemPressInput += ItemNumber.ToString (); //sets the string to look for by adding the current number at the end
				if (altControlScheme)
					itemPressInput += ALT;
			}
				
		}

		/// Sets up keyboard icon visualization for items.
		void KeyboardKeySetUp(){
			//Turn on Keyboard Icon Image
			KeyboardImage.SetActive (true); 

			switch (ItemNumber) {
			case(0):
				KeyboardKeyText.text = "J";
				break;
			case(1):
				KeyboardKeyText.text = "K";
				break;
			case(2):
				KeyboardKeyText.text = "L";
				break;
			case(3):
				KeyboardKeyText.text = ";";
				break;
			default:
				Debug.Log ("<color=red><b>Trying to instantiate over 4 items is not allowed.</b></color>");
				break;
			}

			//Failsafe checking to make sure we don't have over the max number of items when we add an input
			if (ItemNumber < ConstantData.MAX_ACTIVE_ITEMS) {
				itemPressInput += ItemNumber.ToString (); //sets the string to look for by adding the current number at the end
				if (altControlScheme)
					itemPressInput += ALT;
			}

		}


		#endregion Controller/Keyboard SetUp
	}
}
