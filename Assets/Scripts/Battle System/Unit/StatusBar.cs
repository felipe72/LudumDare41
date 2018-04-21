using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle {
	public class StatusBar : MonoBehaviour {
		public Text currentHealthText;
		public Text currentShieldText;

		public RectTransform currentHealthGraphic;
		public GameObject currentShieldGO;

		RectTransform rectTransform;
		Unit unit;
		float maxWidth;

		public void Init(){
			rectTransform = (RectTransform) transform;
			unit = GetComponentInParent<Unit> ();
			var unitRect = (RectTransform) unit.transform;
			var sizeDelta = rectTransform.sizeDelta;

			// Set width of health to be the same as the width of unit
			// Maybe is not best to do that, because it may imply that the unit has more health than it actualy has
			/*
			sizeDelta.x = unitRect.sizeDelta.x;
			rectTransform.sizeDelta = sizeDelta;
			*/

			// Set position to be under unit
			var localPosition = rectTransform.localPosition;
			localPosition.y = -(sizeDelta.y/2 + unitRect.sizeDelta.y/2);
			rectTransform.localPosition = localPosition;

			maxWidth = rectTransform.sizeDelta.x;
			UpdateStatus ();
		}

		public void UpdateStatus(){
			UpdateHealth ();

			UpdateShield ();
		}

		void UpdateHealth(){
			float currentPercentage = (float) unit.GetHealth () / (float) unit.maxHealth;
			currentHealthText.text = unit.GetHealth ().ToString();

			currentHealthGraphic.offsetMax = new Vector2 (-maxWidth * (1 - currentPercentage), currentHealthGraphic.offsetMax.y);
		}

		void UpdateShield(){
			if (unit.GetShield () == 0) {
				currentShieldGO.SetActive(false);
				return;
			}


			currentShieldGO.SetActive(true);
			currentShieldText.text = unit.GetShield ().ToString();
		}
	}
}
