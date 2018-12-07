using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Image HPBarStatus;

	void Start ()
    {
        HPBarStatus = HPBarStatus == null ? GetComponent<Image>() : HPBarStatus;
	}
	
	public void UpdateHP(float current, float max)
    {
        HPBarStatus.fillAmount = current / max;
	}
}
