using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Terminal : Interactable
{
    public bool Interactable = false;

    public float Progress = 0.0f;

    public bool IsActicated
    {
        get
        {
            return m_isActivated;
        }
        set
        {
            m_isActivated = value;

            if (m_animator == null) return;

            m_animator.SetBool("Activated", m_isActivated);
        }
    }
    private bool m_isActivated;

    public UnityEvent EventsOnFinished = null;

    public ProgressBar ProgressIndicator;

    private Animator m_animator;
    private bool Working = false;

    void Start ()
    {
        m_animator = GetComponent<Animator>();

        Switch activator = GetComponent<Switch>();

        IsActicated = (activator == null ? true : activator.InitiallyActivated);
        activator.OnChangeState = (bool state) => { IsActicated = state; };

        if (ProgressIndicator != null)
        {
            ProgressIndicator.gameObject.SetActive(false);
        }
    }
	
	void Update ()
    {
		if (Working && Progress < 1.0f)
        {

            Progress += Time.deltaTime;

            if (ProgressIndicator != null)
            {
                ProgressIndicator.gameObject.SetActive(true);
                ProgressIndicator.UpdateProgress(Mathf.Clamp(Progress, 0.0f, 1.0f), 1.0f);
            }

            if (Progress >= 1.0f && EventsOnFinished != null)
            {
                IsActicated = true;
                EventsOnFinished.Invoke();
            }
        }
	}

    public override void OnInteractStart(Player player)
    {
        if (!Interactable || Progress >= 1.0f) return;

        Working = true;
    }

    public override void OnInteractEnd(Player player)
    {
        if (!Interactable) return;

        Working = false;
    }
}
