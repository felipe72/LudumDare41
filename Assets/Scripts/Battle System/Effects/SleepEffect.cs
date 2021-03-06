﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
	public class SleepEffect : Effect {
		public override void Execute (Card card){
			base.Execute (card);

			foreach (var unit in units) {
				unit.Sleep ();
			}
		}
	}
}