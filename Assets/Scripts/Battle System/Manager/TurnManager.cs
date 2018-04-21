using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;

namespace Battle {
public class TurnManager : Singleton<TurnManager> {
		bool nextTurn;

		void Start(){
			StartCoroutine (Turn ());
		}

		IEnumerator Turn(){
			yield return StartCoroutine (Draw ());

			yield return StartCoroutine (ResolveStatus ());

			yield return StartCoroutine (SelectingCards ());

			yield return StartCoroutine (Discard ());

			yield return StartCoroutine (ResolveTurn ());
		}

		IEnumerator Draw(){
			yield return StartCoroutine(Hand.Instance.DrawHand ());
		}

		IEnumerator ResolveStatus(){
			yield return null;
		}

		IEnumerator SelectingCards(){
			yield return new WaitUntil(() => nextTurn);
		}

		IEnumerator Discard(){
			yield return null;
		}

		IEnumerator ResolveTurn(){
			yield return null;
		}
	}
}