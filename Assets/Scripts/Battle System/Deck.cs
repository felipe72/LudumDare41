using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Battle {
	public class Deck : MonoBehaviour {
		public List<Card> cards;

		Image cardBack;

		void Awake(){
			cardBack = GetComponent<Image> ();

			for(int i=0; i<cards.Count; i++){
				var card = cards [i];

				cards[i] = Instantiate (card, Hand.Instance.disabledCards).GetComponent<Card>();
			}

			CheckEmpty ();
		}

		void CheckEmpty(){
			if (cards.Count == 0) {
				cardBack.color = Color.clear;
			} else {
				cardBack.color = Color.white;
			}
		}

		public Card DrawCard(){
			if (cards.Count == 0) {
				return null;
			}

			Card card = cards [0];

			RemoveCard (card);

			return card;
		}

		public bool IsCardInDeck(Card card){
			return true;
		}

		public void AddCard(Card card){
			cards.Add (card);

			CheckEmpty ();
		}

		public void RemoveCard(Card card){
			if (IsCardInDeck (card)) {
				cards.Remove (card);
			}

			CheckEmpty ();
		}

		public void RemoveCard(Card[] _cards){
			foreach (var card in _cards) {
				RemoveCard (card);
			}
		}

		public void AddCard(Card[] _cards){
			foreach (var card in _cards) {
				AddCard (card);
			}
		}

		public void Shuffle(){

		}

		public void ShuffleToDeck(Deck deck){
			deck.AddCard (cards.ToArray());
			RemoveCard (cards.ToArray());
		}
	}
}