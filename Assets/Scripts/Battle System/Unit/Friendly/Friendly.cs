using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
	public class Friendly : Unit {
		bool knockouted = false;

		public override void Heal (int amount){
			if (!knockouted) {
				base.Heal (amount);
			}
		}

		public override void Revive () {
			base.Revive ();

			knockouted = false;
			Heal (Mathf.RoundToInt(maxHealth / 2f));
		}

		public bool GetKnockouted(){
			return knockouted;
		}
	}
}