using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
	public class FleeEffect : Effect {
		public float fleePercentage;

		public override void Execute (Card card){
			base.Execute (card);

			foreach (var unit in units) {
				var rng = Random.Range (0, 1f);
				if (rng < fleePercentage) {
					unit.Flee ();
				}
			}
		}
	}
}