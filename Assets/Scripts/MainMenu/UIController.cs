using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;
using DG.Tweening;
using System.Linq;

namespace MainMenu {
	public class UIController : Singleton<UIController> {
		[Header("Selection Box Config")]
		public Image selectionBox;
		public float moveTime;
		public Vector2 sizeDeltaIncrement;

		[Header("Options config")]
		public RectTransform optionsParent;

		int currentIndex;
		RectTransform[] options;

		void Start(){
			options = optionsParent.GetComponentsInChildren<CustomButton>().Select(x => (RectTransform)x.transform).ToArray();
		}

		void Update(){
			if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
				currentIndex = (((currentIndex - 1) % options.Length) + options.Length) % options.Length;
				SelectButton (options [currentIndex], currentIndex);
			} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
				currentIndex = (currentIndex + 1) % options.Length;
				SelectButton (options [currentIndex], currentIndex);
			} else if (Input.GetKeyDown (KeyCode.K)) {
				LoadingScreenManager.LoadScene (0);
			}
		}

		public void SelectButton(RectTransform buttonTransform, int index){
			currentIndex = index;

			var sizeDelta = new Vector2 (buttonTransform.sizeDelta.x * buttonTransform.localScale.x, buttonTransform.sizeDelta.y * buttonTransform.localScale.y);
			sizeDelta += sizeDeltaIncrement;


			selectionBox.DOKill ();
			selectionBox.rectTransform.DOMove(buttonTransform.position, moveTime);
			selectionBox.rectTransform.DOSizeDelta (sizeDelta, moveTime);
		}

	}
}
