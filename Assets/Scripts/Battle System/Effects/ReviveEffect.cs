using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Battle {
	public class ReviveEffect : Effect {
		public override void Execute (Card card){
			base.Execute (card);

			var _units = units.Select (x => x.GetComponent<Friendly> ()).Where(x => x.GetKnockouted());

			foreach (var unit in _units) {
				unit.Revive ();
			}
		}
	}
}