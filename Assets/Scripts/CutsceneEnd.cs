using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (JumpToScene ());
	}

	IEnumerator JumpToScene(){
		yield return new WaitForSeconds (18.0f);

        MusicPlayer.Instance.StartMain();
		SceneManager.LoadScene("Level1");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
