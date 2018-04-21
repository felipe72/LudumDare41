using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace Battle {
	public class Card : MonoBehaviour {
		[Header("Game Objects Config")]
		public Text costText;
		public Text descriptionText;
		public Text nameText;
		public Image background;
		public Image icon;

		[Header("Card Config")]
		public int cost;
		public string _name;
		public string description;

		public event VoidCallback onCardDraw;
		public event VoidCallback onCardDiscarted;

		Effect[] effects;
		bool chosen;
		RectTransform rectTransform;
		float moveCardTime = .5f;

		protected void Awake(){
			Initialize ();
		}

		void Initialize(){
			effects = GetComponents<Effect> ();
			rectTransform = (RectTransform)transform;

			EventTrigger trigger = GetComponent<EventTrigger> ();

			if (trigger != null) {
				EventTrigger.Entry entry = new EventTrigger.Entry ();
				entry.eventID = EventTriggerType.PointerEnter;
				entry.callback.AddListener ((data) => MoveUp ());

				EventTrigger.Entry exit = new EventTrigger.Entry ();
				exit.eventID = EventTriggerType.PointerExit;
				exit.callback.AddListener ((data) => MoveDown ());

				trigger.triggers.Add (entry);
				trigger.triggers.Add (exit);
			}

			FormatCard ();
		}

		void MoveUp(){
			if (!chosen) {
				rectTransform.DOLocalMoveY (rectTransform.sizeDelta.y / 4f, moveCardTime);
			}
		}

		void MoveDown(){
			if (!chosen) {
				rectTransform.DOLocalMoveY (0, moveCardTime);
			}
		}

		public void Execute(Unit unit){
			foreach (var effect in effects) {
				effect.Execute (unit);
			}

			Deselect ();
		}

		public void FormatCard(){
			costText.text = cost.ToString ();

			// It should be nice to get params on the text, and do some parsing or something on the text like "<attack>" and get the amount of attack
			// Maybe it can be done with some sort of json
			descriptionText.text = description;
			nameText.text = _name;
		}

		public void Click(){
			if (!chosen) {
				Select ();
			} else {
				Deselect ();
			}
		}

		public void Select(){
			if (TurnClock.Instance.CanStack(this)) {
				chosen = true;
				rectTransform.DOLocalMoveY (rectTransform.sizeDelta.y / 2f, moveCardTime);
				Hand.Instance.SelectCard (this);
			}
		}

		public void Deselect(){
			chosen = false;

			rectTransform.DOLocalMoveY (0, moveCardTime);
			Hand.Instance.DeselectCard (this);
		}

		public void OnCardDraw(){
			if (onCardDraw != null) {
				onCardDraw ();
			}
		}

		public void OnCardDiscarted(){
			if (onCardDiscarted != null) {
				onCardDiscarted ();
			}
		}
	}
}