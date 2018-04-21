using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;

namespace Battle {
	public class TurnClock : Singleton<TurnClock> {
		public GameObject cardBlock;
		public Image enemyIcon;
		public Image pointer;

		public Color guitarristColor;
		public Color bassistColor;
		public Color vocalistColor;
		public Color drummerColor;

		public float minXPosition;
		public float distDiff;

		int available = 8;
		int currIndex = 0;

		List<Image> icons;
		Dictionary<Enemy, Image> dict;
		List<Card> stacked;
		List<RectTransform> cardBlocks;

		void Awake(){
			dict = new Dictionary<Enemy, Image> ();
			icons = new List<Image> ();
			stacked = new List<Card> ();
			cardBlocks = new List<RectTransform> ();
		}

		Color GetColor(CardType type){
			Color color = Color.white;

			switch (type) {
			case CardType.Bassist:
				color = bassistColor;
				break;
			case CardType.Guitarrist:
				color = guitarristColor;
				break;
			case CardType.Vocalist:
				color = vocalistColor;
				break;
			case CardType.Drummer:
				color = drummerColor;
				break;
			}

			return color;
		}

		public bool CanStack(Card card){
			return available >= card.cost;
		}

		public void Stack(Card card){
			var _cardBlock = Instantiate (cardBlock, transform).GetComponent<RectTransform>();
			_cardBlock.localPosition = Vector3.zero;
			_cardBlock.GetComponent<Image> ().color = GetColor (card.type);

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

		public void UnstackWithoutMoving(Card card){
			int index = stacked.FindIndex (x => x == card);
			Destroy(cardBlocks[index].gameObject);
			stacked.RemoveAt (index);
			cardBlocks.RemoveAt (index);
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

		public void ExecuteSelected(List<Card> cards){
			StartCoroutine (executeSelected (cards));
		}

		IEnumerator executeSelected(List<Card> cards){
			List<Card> executionOrder = new List<Card>();
			List<Enemy> currentEnemies = new List<Enemy>();

			Queue<Enemy> enemyQueue = new Queue<Enemy>();
			for (int i = 0; i < 8; i++) {
				executionOrder.Add (null);
				currentEnemies.Add (null);
			}

			foreach (var enemy in EnemyWave.Instance.GetCurrentEnemies()) {
				if (enemy.GetInitiative () >= 0 && enemy.GetInitiative () < 8) {
					currentEnemies [enemy.GetInitiative ()] = enemy;
				}
			}

			int _currIndex = 0;
			foreach (var card in cards) {
				executionOrder [_currIndex] = card;
				_currIndex += card.cost;
			}
			int executing = 0;
			for (int i = 0; i < executionOrder.Count; i++, executing--) {
				var localPosition = pointer.rectTransform.localPosition;
				localPosition.x = minXPosition + i * distDiff;
				pointer.rectTransform.localPosition = localPosition;

				var card = executionOrder [i];
				if (card != null) {
					executing = card.cost-1;
					yield return StartCoroutine(card.Execute ());
					UnstackWithoutMoving (card);
					yield return new WaitForSeconds (1f);
				}

				if (currentEnemies [i] != null) {
					enemyQueue.Enqueue (currentEnemies [i]);
				}

				if (executing <= 0) {
					while (enemyQueue.Count != 0) {
						var enemy = enemyQueue.Dequeue ();
						if (enemy != null) {
							yield return StartCoroutine (enemy.MoveForward ());
							enemy.DoAI ();
							yield return new WaitForSeconds (1f);
							yield return StartCoroutine (enemy.MoveBack ());
							yield return new WaitForSeconds (1f);
						}
					}
				}

				yield return new WaitForSeconds (.5f);
			}

			pointer.rectTransform.localPosition = Vector3.right * 2000;

			yield return new WaitForSeconds (1f);

			TurnManager.Instance.ReadyNextTurn ();
			available = 8;
			currIndex = 0;
		}

		public void CleanIcons(){
			foreach (var img in icons) {
				if (img != null) {
					Destroy (img.gameObject);
				}
			}

			icons.Clear ();
		}

		public void AddEnemyIcon(Enemy enemy, int time){
			Image img = Instantiate<Image>(enemyIcon, transform);
			img.sprite = enemy.icon;
			var localPosition = img.rectTransform.localPosition;
			localPosition.x = minXPosition + distDiff * time;
			img.rectTransform.localPosition = localPosition;

			dict [enemy] = img;

			icons.Add (img);
		}

		public void RemoveIcon(Enemy enemy){
			Destroy (dict [enemy].gameObject);
		}
	}
}
