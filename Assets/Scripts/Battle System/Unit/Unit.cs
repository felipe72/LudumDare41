using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Global;
using DG.Tweening;

namespace Battle {
	public class Unit : MonoBehaviour {
		public int index;
		public int maxHealth;

		public Text indexText;

		public float moveTime;
		public float distance;

		public event VoidCallback onTurnStart;
		public event VoidCallback onBattleStart;
		public event VoidCallback onBattleEnd;

		int currentHealth;
		int currentShield;
		float startPos;
		Image whiteImage;

		StatusBar statusBar;

		void Awake(){
			Initialize ();
		}

		void Start(){
			startPos = transform.localPosition.x;
			indexText.text = "#" + index.ToString ();
			var mask = GetComponentInChildren<Mask> ();
			if(mask != null){
				whiteImage = mask.GetComponentsInChildren<Image> () [1];
			}
		}

		void Initialize(){
			currentHealth = maxHealth;

			StatusBar _bar = Resources.Load<StatusBar> ("Prefabs/Combat/Status Bar");
			statusBar = Instantiate<StatusBar> (_bar, transform);
			statusBar.Init();
		}

		public int GetHealth(){
			return currentHealth;
		}

		public int GetShield(){
			return currentShield;
		}

		public virtual void Sleep(){

		}

		public virtual void Heal(int amount){
			currentHealth += amount;
			currentHealth = Mathf.Min (currentHealth, maxHealth);
		}

		public void ReceiveDamage(int amount){
			currentShield -= amount;

			if (currentShield <= 0) {
				currentHealth += currentShield;
			}

			currentHealth = Mathf.Max (0, currentHealth);
			currentShield = Mathf.Max (0, currentShield);

			// Check if dead
			CheckDead();

			// Update health
			UpdateHealth ();
		}

		public void AddShield(int amount){
			currentShield += amount;

			UpdateHealth ();
		}

		public void OnTurnStart(){
			if (onTurnStart != null) {
				onTurnStart ();
			}
		}

		public void OnBattleStart(){
			if (onBattleStart != null) {
				onBattleStart ();
			}
		}

		public void OnBattleEnd(){
			if (onBattleEnd != null) {
				onBattleEnd ();
			}
		}

		public void Flee(){
			// Do a better feedback than this

			print ("Fled!");
			Die ();
		}

		void CheckDead(){
			if (currentHealth <= 0) {
				Die ();
			}
		}

		public virtual void Revive(){

		}

		protected virtual void Die(){
			Destroy (gameObject);
		}

		void UpdateHealth(){
			// Access status bar of unit and update it

			statusBar.UpdateStatus ();
		}

		public void Higlight(){
			if(whiteImage != null){
				whiteImage.DOKill ();
				whiteImage.color = new Color32 (255, 255, 255, 128);
				whiteImage.DOFade (0f, .5f).SetLoops (-1, LoopType.Restart);
			}
		}

		public void Unhighlight(){
			if (whiteImage != null) {
				whiteImage.DOKill ();
				whiteImage.color = Color.clear;
			}
		}

		public IEnumerator MoveForward(){
			var localPos = transform.localPosition;

			yield return transform.DOLocalMoveX (localPos.x + distance, moveTime).WaitForCompletion();
		}


		public IEnumerator MoveBack(){
			yield return transform.DOLocalMoveX (startPos, moveTime).WaitForCompletion();
		}
	}
}