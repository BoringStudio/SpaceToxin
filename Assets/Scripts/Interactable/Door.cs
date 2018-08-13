using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Door door = (Door)target;
        
        if (GUILayout.Button("Toggle"))
        {
            door.IsClosed = !door.IsClosed;
        }

        GUILayout.Label("State: " + (door.IsClosed ? "Closed" : "Opened"));
    }
}
#endif

[ExecuteInEditMode]
public class Door : MonoBehaviour {
    public AudioClip OpeningSound;
    public AudioClip ClosingSound;

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
            m_isClosed = value;
            m_collision.enabled = value;

            if (m_audioSource != null)
            {
                if (value && OpeningSound != null) m_audioSource.PlayOneShot(OpeningSound);
                if (!value && ClosingSound != null) m_audioSource.PlayOneShot(ClosingSound);
            }

            if (m_animators == null || m_isActivated == false) return;

            foreach (Animator animator in m_animators)
            {
                animator.SetBool("Closed", value);
            }
        }
    }
    private bool m_isClosed = false;

    public bool IsActivated
    {
        get
        {
            return m_isActivated;
        }
    }
    private bool m_isActivated;

    private Collider2D m_collision;
    private Animator[] m_animators;
    private AudioSource m_audioSource;

    void Start()
    {
        m_collision = GetComponent<Collider2D>();
        m_animators = GetComponentsInChildren<Animator>();
        m_audioSource = GetComponent<AudioSource>();

        if (m_audioSource) m_audioSource.Stop();

        m_isActivated = true;
        IsClosed = InitiallyClosed;
        
        Switch activator = GetComponent<Switch>();
        m_isActivated = (activator == null ? true : activator.CurrentState);
        activator.OnChangeState = (bool state) => { m_isActivated = state; };
    }

    public void Open()
    {
        IsClosed = false;
    }

    public void Close()
    {
        IsClosed = true;
    }
}
