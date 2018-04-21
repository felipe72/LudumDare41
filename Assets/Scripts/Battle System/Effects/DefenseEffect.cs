using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
	public class DefenseEffect : Effect {
		public int amount;

		public override void Execute (Unit unit){
			base.Execute (unit);

			unit.AddShield (amount);

			print ("Defend yourself, bitch");
		}
	}
}