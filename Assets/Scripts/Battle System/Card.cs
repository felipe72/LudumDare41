using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Global;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Linq;

namespace Battle {
	public enum CardType {Guitarrist, Bassist, Drummer, Vocalist}

	public class Card : MonoBehaviour {
		[Header("Card Config")]
		public int cost;
		public bool friendly;
		public bool individual;
		public CardType type;

		public event VoidCallback onCardDraw;
		public event VoidCallback onCardDiscarted;

		bool disabled;
		Effect[] effects;
		bool chosen;
		RectTransform rectTransform;
		float moveCardTime = .5f;
		Vector3 savedPosition;

		Friendly GetUnit(){
			Friendly _friendly = null;
			switch (type) {
			case CardType.Bassist:
				_friendly = FindObjectOfType<Bassist> ();
				break;
			case CardType.Guitarrist:
				_friendly = FindObjectOfType<Guitarrist> ();
				break;
			case CardType.Vocalist:
				_friendly = FindObjectOfType<Vocalist> ();
				break;
			case CardType.Drummer:
				_friendly = FindObjectOfType<Drummer> ();
				break;
			}

			return _friendly;
		}

		protected void Awake(){
			Initialize ();
		}

		void Initialize(){
			effects = GetComponents<Effect> ();
			rectTransform = (RectTransform)transform;

			onCardDiscarted += () => disabled = true;
			onCardDraw += () => {
				disabled = false;
				chosen = false;
			};

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
		}

		List<Unit> GetUnits(){
			if (friendly) {
				if (individual) {
					List<Unit> _units = new List<Unit> ();
					_units.Add (FindObjectsOfType<Friendly> ().OrderBy(x => x.index).ToList() [0]);
					return _units;
				} else {
					return FindObjectsOfType<Friendly> ().Select(x => x.GetComponent<Unit>()).ToList();
				}
			} else {
				if (individual) {
					List<Unit> _units = new List<Unit> ();
					_units.Add (FindObjectsOfType<Enemy> ().OrderBy(x => x.index).ToList() [0]);
					return _units;
				} else {
					return FindObjectsOfType<Enemy> ().Select(x => x.GetComponent<Unit>()).ToList();;
				}
			}
		}

		void MoveUp(){
			if (!chosen && !disabled) {
				rectTransform.DOLocalMoveY (rectTransform.sizeDelta.y / 4f, moveCardTime);
				foreach (var unit in GetUnits()) {
					unit.Higlight ();
				}
			}
		}

		void MoveDown(){
			if (!chosen && !disabled) {
				rectTransform.DOLocalMoveY (0, moveCardTime);
				foreach (var unit in GetUnits()) {
					unit.Unhighlight();
				}
			}
		}

		public IEnumerator Execute(){
			Friendly _unit = GetUnit ();

			yield return StartCoroutine (_unit.MoveForward());


			foreach (var effect in effects) {
				effect.Execute (this);
			}
			yield return new WaitForSeconds (1f);
			yield return StartCoroutine (_unit.MoveBack());

			Hand.Instance.Dispose (this);
			Deselect ();
		}

		public void Click(){
			if (TurnManager.Instance.CurrentTurn () == Turn.SelectCards) {
				if (!chosen) {
					Select ();
				} else {
					Deselect ();
				}
			} else {
				foreach (var unit in GetUnits()) {
					unit.Unhighlight();
				}
				if (!chosen) {
					SelectDiscard ();
				} else {
					DeselectDiscard ();
				}
			}
		}

		public void SelectDiscard (){
			chosen = true;
			rectTransform.DOKill ();
			Hand.Instance.SelectDiscard (this);
		}

		public void DeselectDiscard (){
			chosen = false;
			Hand.Instance.DeselectDiscard (this);
		}

		public void Select(){
			foreach (var unit in GetUnits()) {
				unit.Unhighlight();
			}

			if (TurnClock.Instance.CanStack (this)) {
				chosen = true;
				rectTransform.DOLocalMoveY (rectTransform.sizeDelta.y / 2f, moveCardTime);
				Hand.Instance.SelectCard (this);
			} else {
				print ("oi");
			}
		}

		public void Deselect(){
			foreach (var unit in GetUnits()) {
				unit.Higlight();
			}
			chosen = false;

			rectTransform.DOLocalMoveY (0, moveCardTime);
			Hand.Instance.DeselectCard (this);
		}

		public void SavePosition(){
			savedPosition = rectTransform.position;
		}

		public void ReturnToSaved(){
			rectTransform.position = savedPosition;
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