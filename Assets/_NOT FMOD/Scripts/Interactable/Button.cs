using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Button))]
public class ButtonEditor : Editor
{
    private bool m_lastButtonState = false;
    private bool m_showSoundSelection = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Button button = (Button)target;
/*
        m_showSoundSelection = EditorGUILayout.Foldout(m_showSoundSelection, "Sounds");
        if (m_showSoundSelection)
        {
            button.DeactivatedSound = EditorGUILayout.ObjectField("Deactivated", button.DeactivatedSound, typeof(AudioClip), true) as AudioClip;
            button.PressSound = EditorGUILayout.ObjectField("Pressed", button.PressSound, typeof(AudioClip), true) as AudioClip;
            button.ReleaseSound = EditorGUILayout.ObjectField("Released", button.ReleaseSound, typeof(AudioClip), true) as AudioClip;
        }
*/

        //////////
        if (button.Mode == ButtonMode.Push)
        {
            bool buttonPressed = GUILayout.Button("Push");

            if (buttonPressed && m_lastButtonState == false)
            {
                button.Press();
                m_lastButtonState = true;
            }
            else if (!buttonPressed && m_lastButtonState == true)
            {
                button.Release();
                m_lastButtonState = false;
            }
        }
        else
        {
            if (GUILayout.Button("Toggle"))
            {
                button.Press();
            }
        }

        GUILayout.Label("State: " + (button.IsPressed ? "Pressed" : "Released"));

        if (GUI.changed)
        {
            EditorUtility.SetDirty(button);
        }
    }
}
#endif

public enum ButtonMode
{
    Push,
    Toggle
};


public class Button : Interactable
{

    [SerializeField]
    public ButtonMode Mode = ButtonMode.Push;

    public bool InitiallyPressed = false;

    public UnityEvent EventsOnPress = null;
    public UnityEvent EventsOnRelease = null;

    private Animator m_animator;

    public bool IsActivated
    {
        get
        {
            return m_isActivated;
        }
        set
        {
            if (m_isActivated != value)
            {
                m_isActivated = value;
                m_animator.SetBool("Activated", value);
            }
        }
    }
    private bool m_isActivated;

    public bool IsPressed
    {
        get
        {
            return m_isPressed;
        }
        set
        {
            if (m_isPressed != value)
            {
                m_isPressed = value;
              
                    if (value) FMODUnity.RuntimeManager.PlayOneShotAttached(FMODPaths.BUTTON_PRESSED, this.gameObject);
                    if (!value) FMODUnity.RuntimeManager.PlayOneShotAttached(FMODPaths.BUTTON_RELEASED, this.gameObject);

                m_animator.SetBool("Pressed", value);
            }
        }
    }
    private bool m_isPressed = false;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        IsActivated = true;

        Switch activator = GetComponent<Switch>();
        IsActivated = activator == null ? true : activator.InitiallyActivated;

        activator.OnChangeState = (bool state) => { IsActivated = state; };

        IsPressed = Mode == ButtonMode.Toggle && InitiallyPressed;
    }

    public override void OnInteractStart(Player player)
    {
        Press();
    }

    public override void OnInteractEnd(Player player)
    {
        if (Mode == ButtonMode.Push) Release();
    }

    public void Press()
    {
        if (IsActivated)
        {
            IsPressed = Mode == ButtonMode.Push ? true : !IsPressed;
            if (IsPressed && EventsOnPress != null) EventsOnPress.Invoke();
            if (!IsPressed && EventsOnRelease != null) EventsOnRelease.Invoke();
        }
        else
        {       
                FMODUnity.RuntimeManager.PlayOneShotAttached(FMODPaths.BUTTON_DEACTIVATED, this.gameObject);        
        }
    }

    public void Release()
    {
        if (IsActivated)
        {
            IsPressed = false;
            if (EventsOnRelease != null) EventsOnRelease.Invoke();
        }
    }
}
