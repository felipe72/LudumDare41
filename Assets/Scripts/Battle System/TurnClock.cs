using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

namespace Battle {
	public class TurnClock : Singleton<TurnClock> {
		public GameObject cardBlock;

		public float minXPosition;
		public float distDiff;

		int available = 8;
		int currIndex = 0;

		List<Card> stacked;
		List<RectTransform> cardBlocks;

		void Awake(){
			stacked = new List<Card> ();
			cardBlocks = new List<RectTransform> ();
		}

		public bool CanStack(Card card){
			return available >= card.cost;
		}

		public void Stack(Card card){
			var _cardBlock = Instantiate (cardBlock, transform).GetComponent<RectTransform>();
			_cardBlock.localPosition = Vector3.zero;

			var startPos = minXPosition + currIndex * distDiff;
			startPos += (card.cost - 1) * distDiff / 2f;
			var localPosition = _cardBlock.localPosition;
			localPosition.x = startPos;
			_cardBlock.localPosition = localPosition;

			var sizeDelta = _cardBlock.sizeDelta;
			sizeDelta.x += (card.cost - 1) * distDiff;
			_cardBlock.sizeDelta = sizeDelta;

			available -= card.cost;
			currIndex += card.cost;

			stacked.Add (card);
			cardBlocks.Add (_cardBlock);
		}

		public void Unstack(Card card){
			if (stacked.Contains (card)) {
				int index = stacked.FindIndex (x => x == card);
				int count = stacked.Count - index;
				var range = stacked.GetRange (index, count);

				for (int i = index; i < cardBlocks.Count; i++) {
					Destroy (cardBlocks [i].gameObject);
				}

				stacked.RemoveRange (index, count);
				cardBlocks.RemoveRange (index, count);

				// Adjust available and currIndex for each card removed from the range
				foreach (var _card in range) {
					available += _card.cost;
					currIndex -= _card.cost;
				}

				// Remove the card to be unstacked in the range
				range.RemoveAt (0);
				foreach (var _card in range) {
					Stack (_card);
				}
			}
		}
	}
}
