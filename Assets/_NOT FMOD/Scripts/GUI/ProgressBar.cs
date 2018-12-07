using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public Image ProgressBarStatus = null;

	private void Start()
	{
		if (ProgressBarStatus == null)
		{
			ProgressBarStatus = GetComponent<Image>();
		}
	}

	public void UpdateProgress(float current, float max)
	{
        ProgressBarStatus.fillAmount = current / max;
	}
}
