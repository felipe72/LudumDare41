using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Battle {
	public class AttackEffect : Effect {
		[Header("Attack Config")]
		public int amount;

		public override void Execute (Card card){
			base.Execute (card);

			print ("Oi");

			foreach (var unit in units) {
				unit.ReceiveDamage (amount);
			}
		}
	}
}