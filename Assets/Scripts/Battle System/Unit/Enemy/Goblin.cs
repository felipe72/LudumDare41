using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
	public class Goblin : Enemy {
		public int attack;

		public override void DoAI (){
			base.DoAI ();

			var friendlies = GetFriendly ();

			var unit = friendlies [Random.Range (0, friendlies.Length)];
			unit.ReceiveDamage (attack);
		}
	}
}