using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Range(0, 1)]
    public float InitiallyFilled = 1.0f;

    [Range(0, 1)]
    public float Consumption = 0.1f;

    public bool IsWorking
    {
        get { return IsActicated && Fullness > 0.0f; }
    }

    public bool IsActicated
    {
        get
        {
            return m_isActivated;
        }
        set
        {
            if (m_animator == null) return;

            m_isActivated = value;
            m_animator.SetBool("Activated", m_isActivated);
            m_animator.SetFloat("Speed", m_isActivated ? 1.0f : 0.0f);
        }
    }
    private bool m_isActivated;

    public float Fullness
    {
        get
        {
            return m_fullness;
        }
        set
        {
            if (m_animator == null) return;

            m_fullness = Mathf.Clamp(value, 0.0f, 1.0f);
            m_animator.SetFloat("Fullness", m_fullness);
        }
    }
    private float m_fullness;

    
    private Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        Switch activator = GetComponent<Switch>();

        IsActicated = (activator == null ? true : activator.InitiallyActivated);
        activator.OnChangeState = (bool state) => { IsActicated = state; };

        Fullness = InitiallyFilled;
    }

    void Update()
    {
        if (IsWorking)
        {
            Fullness = Fullness - Time.deltaTime * Consumption;
        }
    }
}
