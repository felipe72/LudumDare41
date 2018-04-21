using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

namespace Battle {
	public class Hand : Singleton<Hand> {
		List<Card> cardsInHand;

		[Header("Objects config")]
		public Deck drawPile;
		public Deck discardPile;
		public HorizontalLayoutGroup layoutGroup;
		public Transform disabledCards;

		[Header("Drawing config")]
		public int maxNumOfCards;
		public float drawingTime;

		List<Card> cardsSelected;

		void Awake(){
			cardsInHand = new List<Card> ();
			cardsSelected = new List<Card> ();
		}

		public void Draw(){
			layoutGroup.enabled = true;
			Card card = drawPile.DrawCard ();

			if (card != null) {
				card.transform.SetParent (transform);
				var localPosition = card.transform.localPosition;
				localPosition.z = 0;
				card.transform.localPosition = localPosition;

				card.transform.localScale = Vector3.one;

				card.OnCardDraw ();
				cardsInHand.Add (card);
			}
		}

		public void SelectCard(Card card){
			TurnClock.Instance.Stack(card);
			cardsSelected.Add(card);
		}

		public void DeselectCard(Card card){
			if (cardsSelected.Contains(card)) {
				TurnClock.Instance.Unstack (card);
				cardsSelected.Remove (card);
			}
		}

		public void Discard(Card card){
			discardPile.AddCard (card);
			cardsInHand.Remove (card);

			card.transform.SetParent (discardPile.transform);
			card.transform.localPosition = Vector3.zero;

			card.OnCardDiscarted ();
		}

		public IEnumerator DrawHand(){
			int cardsToDraw = maxNumOfCards - NumOfCardsInHand ();
			for (int i = 0; i < cardsToDraw; i++) {
				Draw ();
				yield return new WaitForSeconds (drawingTime);
			}

			layoutGroup.enabled = false;
		}

		public int NumOfCardsInHand(){
			return cardsInHand.Count;
		}
	}
}