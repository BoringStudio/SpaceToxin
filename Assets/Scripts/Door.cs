using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Door : MonoBehaviour {
    [Header("Initial values")]
    public bool InitiallyClosed = true;

    public bool IsClosed
    {
        get
        {
            return m_isClosed;
        }
        set
        {
            if (m_animators == null) return;

            m_isClosed = value;

            foreach (Animator animator in m_animators)
            {
                animator.SetBool("Closed", value);
            }
        }
    }
    private bool m_isClosed = false;

    private Animator[] m_animators;

    void Start()
    {
        m_animators = GetComponentsInChildren<Animator>();
        
        IsClosed = InitiallyClosed;
	}
}
