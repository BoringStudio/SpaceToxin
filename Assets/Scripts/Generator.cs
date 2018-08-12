using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Initial values")]
    public bool InitiallyActivated = false;

    [Range(0, 1)]
    public float InitiallyFilled = 1.0f;


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
            if (m_animators == null) return;

            m_isActivated = value;
            foreach (Animator animator in m_animators)
            {
                animator.SetBool("Activated", m_isActivated);
            }
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
            if (m_animators == null) return;

            m_fullness = value;
            foreach (Animator animator in m_animators)
            {
                animator.SetFloat("Fullness", m_fullness);
            }
        }
    }
    private float m_fullness;

    
    private Animator[] m_animators;

    void Start()
    {
        m_animators = GetComponentsInChildren<Animator>();

        IsActicated = InitiallyActivated;
        Fullness = InitiallyFilled;

        if (m_animators == null) return;

        foreach (Animator animator in m_animators)
        {
            
        }
    }
}
