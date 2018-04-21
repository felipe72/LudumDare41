using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using System.Linq;

namespace Battle {
	public class EnemyWave : Singleton<EnemyWave> {
		public Vector3[] spawnPositions;
		public Enemy[] possibleEnemies;

		List<Enemy> currentEnemies;

		void Awake(){
			currentEnemies = new List<Enemy> ();
		}

		public void SetIniciative(){
			var rnd = new System.Random ();
			var randomNumbers = Enumerable.Range(0,8).OrderBy(x => rnd.Next()).ToList();

			foreach (var enemy in currentEnemies) {
				enemy.SetInitiative(randomNumbers [enemy.index]);
			}
		}

		IEnumerator SpawnWave(int count=-1){
			if (count == -1) {
				count = Random.Range (2, 6);
			}

			for (int i = 0; i < count; i++) {
				var enemy = possibleEnemies [Random.Range (0, possibleEnemies.Length)];
				Spawn (enemy, i);

				yield return new WaitForSeconds (.2f);
			}

			TurnManager.Instance.ReadyNextTurn ();
		}

		void Spawn(Enemy enemy, int index){
			var _enemy = Instantiate<Enemy>(enemy, transform);
			var rect = (RectTransform)_enemy.transform;
			rect.localPosition = spawnPositions[index];
			_enemy.index = index+1;
			currentEnemies.Add (_enemy);
		}

		public List<Enemy> GetCurrentEnemies(){
			return currentEnemies;
		}

		public void RemoveDeadEnemies(){
			for (int i = currentEnemies.Count - 1; i >= 0; i--) {
				if (currentEnemies [i] == null) {
					currentEnemies.RemoveAt (i);
				}
			}

			CheckEndWave ();
		}

		public void RemoveEnemy(Enemy enemy){
			if (currentEnemies.Contains (enemy)) {
				currentEnemies.Remove (enemy);
			}
		}

		public void CheckEndWave(){
			if (currentEnemies.Count == 0) {
				StartCoroutine(SpawnWave ());
			} else {
				TurnManager.Instance.ReadyNextTurn ();
			}
		}
	}
}
