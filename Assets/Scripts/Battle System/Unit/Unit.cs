using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Battle {
	public class Unit : MonoBehaviour {
		public int maxHealth;
		public event VoidCallback onTurnStart;
		public event VoidCallback onBattleStart;
		public event VoidCallback onBattleEnd;

		int currentHealth;
		int currentShield;

		StatusBar statusBar;

		void Awake(){
			Initialize ();
		}

		void Initialize(){
			currentHealth = maxHealth;

			StatusBar _bar = Resources.Load<StatusBar> ("Prefabs/Combat/Status Bar");
			statusBar = Instantiate<StatusBar> (_bar, transform);
			statusBar.Init();
		}

		public int GetHealth(){
			return currentHealth;
		}

		public int GetShield(){
			return currentShield;
		}

		public void ReceiveDamage(int amount){
			currentShield -= amount;

			if (currentShield <= 0) {
				currentHealth += currentShield;
			}

			currentHealth = Mathf.Max (0, currentHealth);
			currentShield = Mathf.Max (0, currentShield);

			// Check if dead

			// Update health
			UpdateHealth ();
		}

		public void AddShield(int amount){
			currentShield += amount;

			UpdateHealth ();
		}

		public void OnTurnStart(){
			if (onTurnStart != null) {
				onTurnStart ();
			}
		}

		public void OnBattleStart(){
			if (onBattleStart != null) {
				onBattleStart ();
			}
		}

		public void OnBattleEnd(){
			if (onBattleEnd != null) {
				onBattleEnd ();
			}
		}

		void UpdateHealth(){
			// Access status bar of unit and update it

			statusBar.UpdateStatus ();
		}
	}
}