using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
	public class CritEffect : AttackEffect {
		public float chance;
		public float multAmount;

		public override void Execute (Card card) {
			var rng = Random.Range (0, 1f);

			if (chance < rng) {
				amount = Mathf.RoundToInt(amount * multAmount);
			}

			base.Execute (card);
		}
	}
}