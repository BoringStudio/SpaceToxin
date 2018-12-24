using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Switch))]
public class SwitchEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Switch activator = (Switch)target;

        if (GUILayout.Button("Toggle state"))
        {
            activator.CurrentState = !activator.CurrentState;
        }

        GUILayout.Label("State: " + (activator.CurrentState ? "Activated" : "Deactivated"));
    }
}
#endif

public class Switch : MonoBehaviour
{
    public delegate void VoidFunc();
    public delegate void VoidBoolFunc(bool state);

    public bool InitiallyActivated = false;

    public VoidFunc OnActivate = null;
    public VoidFunc OnDeactivate = null;
    public VoidBoolFunc OnChangeState = null;
    
    [SerializeField]
    public bool CurrentState
    {
        get { return m_currentState; }
        set
        {
            if (m_currentState == value) return;

            m_currentState = value;
            if (OnActivate != null && value)
            {
                OnActivate();
            }
            else if (OnDeactivate != null)
            {
                OnDeactivate();
            }

            if (OnChangeState != null) {
                OnChangeState(value);
            }
        }
    }
    private bool m_currentState;

    void Start()
    {
        CurrentState = InitiallyActivated;
    }

    public void Activate()
    {
        CurrentState = true;
    }

    public void Deactivate()
    {
        CurrentState = false;
    }
}
