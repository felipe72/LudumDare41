using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Battle {
	public class Enemy : Unit {
		public Sprite icon;
		protected int initiative;

		public int GetInitiative(){
			return initiative;
		}

		protected override void Die () {
			EnemyWave.Instance.RemoveEnemy (this);
			TurnClock.Instance.RemoveIcon (this);

			base.Die ();
		}

		public void SetInitiative(int _initiative){
			initiative = _initiative;
			TurnClock.Instance.AddEnemyIcon(this, _initiative);
		}

		protected Friendly[] GetFriendly(){
			return FindObjectsOfType<Friendly> ().Where(x => !x.GetKnockouted()).ToArray();
		}

		public virtual void DoAI(){
			
		}

		public override void Sleep () {
			base.Sleep ();
			initiative = 10;
			TurnClock.Instance.RemoveIcon (this);
		}
	}
}