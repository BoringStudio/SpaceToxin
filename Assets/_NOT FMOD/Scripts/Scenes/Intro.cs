using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
    public string FirstLevelName = "Level1";

    private bool m_skipped = false;

	void Start () {
		StartCoroutine (JumpToScene ());
	}

	IEnumerator JumpToScene(){
		yield return new WaitForSeconds (18.0f);

        if (!m_skipped) StartFirstLevel();
    }
	
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            StartFirstLevel();
        }
	}

    void StartFirstLevel()
    {
        m_skipped = true;
        AudioManager.instance.MusicOne();
        SceneManager.LoadScene(FirstLevelName);
    }
}
