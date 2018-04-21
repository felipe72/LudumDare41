using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Battle {
	public enum Turn { Drawing, Status, SelectCards, DiscardCards, ResolveTurn}

	public class TurnManager : Singleton<TurnManager> {
		bool nextTurn;
		Turn currentTurn;

		void Start(){
			StartCoroutine (TurnByTurn ());
		}

		public void ReadyNextTurn(){
			nextTurn = true;
		}

		IEnumerator TurnByTurn(){
			while (true) {
				yield return StartCoroutine (SpawnEnemies());

				currentTurn = Turn.Drawing;
				yield return StartCoroutine (Draw ());

				currentTurn = Turn.Status;
				yield return StartCoroutine (ResolveStatus ());

				currentTurn = Turn.SelectCards;
				yield return StartCoroutine (SelectingCards ());

				currentTurn = Turn.DiscardCards;
				yield return StartCoroutine (Discard ());

				currentTurn = Turn.ResolveTurn;
				yield return StartCoroutine (ResolveTurn ());
			}
		}

		IEnumerator SpawnEnemies(){
			EnemyWave.Instance.CheckEndWave ();
			yield return new WaitUntil(() => nextTurn);
			EnemyWave.Instance.SetIniciative ();
			nextTurn = false;
		}

		IEnumerator Draw(){
			yield return StartCoroutine(Hand.Instance.DrawHand ());
		}

		IEnumerator ResolveStatus(){
			yield return null;
		}

		IEnumerator SelectingCards(){
			yield return new WaitUntil(() => nextTurn);
			nextTurn = false;
		}

		IEnumerator Discard(){
			Hand.Instance.StartDiscardPhase ();
			yield return new WaitUntil(() => nextTurn);
			Hand.Instance.DiscardSelected ();
			nextTurn = false;
		}

		IEnumerator ResolveTurn(){
			Hand.Instance.ExecuteSelected ();
			yield return new WaitUntil(() => nextTurn);
			TurnClock.Instance.CleanIcons();
			nextTurn = false;
		}

		public Turn CurrentTurn(){
			return currentTurn;
		}
	}
}