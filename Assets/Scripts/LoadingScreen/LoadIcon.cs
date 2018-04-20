using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIcon : MonoBehaviour {
	public void EndLoad(){
		FindObjectOfType<LoadingScreenManager> ().CanEndLoading ();
	}
}
