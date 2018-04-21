using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Battle {
	public class Effect : MonoBehaviour {
		protected List<Unit> units;

		public virtual void Execute(Card card){
			units = new List<Unit>();

			if (card.friendly) {
				if (card.individual) {
					units.Add (FindObjectsOfType<Friendly> ().OrderBy (x => x.index).ToList()[0]);
				} else {
					units.AddRange (FindObjectsOfType<Friendly> ());
				}
			} else {
				if (card.individual) {
					var enemies = FindObjectsOfType<Enemy> ().OrderBy (x => x.index).ToList ();
					if(enemies.Count != 0){
						units.Add (enemies[0]);
					}
				} else {
					units.AddRange (FindObjectsOfType<Enemy> ());
				}
			}

			units = units.Where (x => x != null).ToList();
		}
	}
}