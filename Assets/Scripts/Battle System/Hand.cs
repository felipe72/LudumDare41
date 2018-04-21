using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;
using DG.Tweening;
using System.Linq;

namespace Battle {
	public class Hand : Singleton<Hand> {
		List<Card> cardsInHand;
		List<Card> cardsSelected;
		List<Card> toBeDiscarted;

		[Header("Objects config")]
		public Deck drawPile;
		public Deck discardPile;
		public HorizontalLayoutGroup layoutGroup;
		public Transform disabledCards;
		public Transform behindOverlay;
		public Image blackOverlay;
		public GameObject nextTurnObjects;
		public RectTransform discartedTransform;

		[Header("Drawing config")]
		public int maxNumOfCards;
		public float drawingTime;


		void Awake(){
			cardsInHand = new List<Card> ();
			cardsSelected = new List<Card> ();
			toBeDiscarted = new List<Card> ();
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

		public void SelectDiscard(Card card){
			toBeDiscarted.Add (card);
			card.transform.SetParent (discartedTransform);
		}

		public void DeselectDiscard(Card card){
			if (toBeDiscarted.Contains (card)) {
				toBeDiscarted.Remove (card);
				card.transform.SetParent (transform);
				card.ReturnToSaved ();
			}
		}

		public void DeselectAll(){
			for (int i = cardsSelected.Count - 1; i >= 0; i--) {
				cardsSelected[i].Deselect ();
			}
		}

		public void Discard(Card card){
			discardPile.AddCard (card);

			toBeDiscarted.Remove (card);
			cardsInHand.Remove (card);

			card.transform.SetParent (discardPile.transform);
			card.transform.localPosition = Vector3.zero;

			card.OnCardDiscarted ();
		}

		public void Dispose(Card card){
			discardPile.AddCard (card);

			cardsSelected.Remove (card);
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

		void SendToBack(){
			foreach (var card in cardsSelected) {
				card.transform.SetParent (behindOverlay);
			}
		}

		public void StartDiscardPhase(){
			SendToBack ();
			blackOverlay.raycastTarget = true;
			blackOverlay.DOFade (.5f, 1f).onComplete += () => {
				nextTurnObjects.SetActive(true);
			};

			foreach (var card in cardsInHand) {
				card.SavePosition ();
			}
		}

		public void ExecuteSelected(){
			TurnClock.Instance.ExecuteSelected (cardsSelected);
		}

		public void DiscardSelected(){
			for (int i = toBeDiscarted.Count - 1; i >= 0; i--) {
				var card = toBeDiscarted [i];
				Discard (card);
			}
			nextTurnObjects.SetActive(false);
			blackOverlay.DOFade (0f, 1f).onComplete += () => {
				blackOverlay.raycastTarget = false;
			};
		}
	}
}