using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
	public class AttackEffect : Effect {
		[Header("Attack Config")]
		public int amount;

		public override void Execute (Unit unit){
			base.Execute (unit);

			unit.ReceiveDamage (amount);
			print ("Attack him, bitch");
		}
	}
}