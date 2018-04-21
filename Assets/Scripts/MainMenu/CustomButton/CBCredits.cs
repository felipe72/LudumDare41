using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MainMenu {
	public class CBCredits : CustomButton {
		public RectTransform window;
		public float moveTime;
		public Vector3 windowPosition;

		public override void Activate (){
			window.gameObject.SetActive (true);
			window.DOLocalMove (windowPosition, moveTime).SetEase(Ease.OutBack);
		}

		public override void Deactivate (){
			window.DOLocalMoveY (-2000, moveTime).SetEase(Ease.OutBack);
		}
	}
}
