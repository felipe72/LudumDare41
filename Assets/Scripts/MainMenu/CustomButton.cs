using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MainMenu {
	[RequireComponent(typeof(EventTrigger))]
	public class CustomButton : MonoBehaviour {
		int index;
		Text text;
		RectTransform rectTransform;

		void Start(){
			rectTransform = (RectTransform)transform;
			index = transform.parent.GetSiblingIndex ();

			EventTrigger trigger = GetComponent<EventTrigger> ();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener((data) => Enter ());

			EventTrigger.Entry exit = new EventTrigger.Entry ();
			exit.eventID = EventTriggerType.PointerExit;
			exit.callback.AddListener ((data) => Exit ());

			trigger.triggers.Add (entry);
			trigger.triggers.Add (exit);
		}

		public void Enter() {
			UIController.Instance.SelectButton (rectTransform, index);
		}

		public void Exit(){

		}
	}
}