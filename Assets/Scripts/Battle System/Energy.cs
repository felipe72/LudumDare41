using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

namespace Battle {
	/* public class Energy : Singleton<Energy> {
		[Header("Values config")]
		public int maxEnergy;

		[Header("Texts")]
		public Text currentEnergyText;
		public Text maxEnergyText;

		int currentEnergy;

		void Awake(){
			AddEnergy (maxEnergy);
		}

		public bool IsEnoughEnergy(int amount){
			return currentEnergy >= amount;
		}

		public void SpendEnergy(int amount){
			if (!IsEnoughEnergy (amount)) {
				return;
			}

			currentEnergy -= amount;
			UpdateTexts ();
		}

		public void AddEnergy(int amount){
			currentEnergy += amount;

			UpdateTexts ();
		}

		void UpdateTexts(){
			currentEnergyText.text = currentEnergy.ToString ();
			maxEnergyText.text = maxEnergy.ToString ();
		}

		public void RestoreFullEnergy(){
			AddEnergy (maxEnergy - currentEnergy);
		}
	} */
}