using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Button))]
public class ButtonEditor : Editor
{
    private bool m_lastButtonState = false;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Button button = (Button)target;
        
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
    }
}
#endif

public enum ButtonMode
{
    Push,
    Toggle
};

[RequireComponent(typeof(SpriteRenderer), typeof(Switch))]
public class Button : Interactable
{
    public Sprite DeactivatedOff;
    public Sprite DeactivatedOn;
    public Sprite ActivatedOff;
    public Sprite ActivatedOn;

    private SpriteRenderer m_renderer;

    [SerializeField]
    public ButtonMode Mode = ButtonMode.Push;

    public bool InitiallyPressed = false;

    public UnityEvent EventsOnPress = null;
    public UnityEvent EventsOnRelease = null;

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
                UpdateSprite();
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
                UpdateSprite();
            }
        }
    }
    private bool m_isPressed = false;

    void Start()
    {
        m_renderer = GetComponent<SpriteRenderer>();

        IsActivated = true;

        Switch activator = GetComponent<Switch>();
        IsActivated = activator == null ? true : activator.InitiallyActivated;

        activator.OnChangeState = (bool state) => { IsActivated = state; };

        if (Mode == ButtonMode.Toggle && InitiallyPressed)
        {
            IsPressed = true;
        }

        UpdateSprite();
    }

    public override void OnInteractStart(Player player)
    {
        Press();
    }

    public override void OnInteractEnd(Player player)
    {
        Release();
    }

    public void Press()
    {
        if (IsActivated)
        {
            IsPressed = Mode == ButtonMode.Push ? true : !IsPressed;
            if (IsPressed && EventsOnPress != null) EventsOnPress.Invoke();
            if (!IsPressed && EventsOnRelease != null) EventsOnRelease.Invoke();
        }
    }

    public void Release()
    {
        if (IsActivated)
        {
            IsPressed = Mode == ButtonMode.Push ? false : IsPressed;
            if (IsPressed && EventsOnPress != null) EventsOnPress.Invoke();
            if (!IsPressed && EventsOnRelease != null) EventsOnRelease.Invoke();
        }
    }

    public void UpdateSprite()
    {
        if (m_isActivated)
        {
            m_renderer.sprite = m_isPressed ? ActivatedOn : ActivatedOff;
        }
        else
        {
            m_renderer.sprite = m_isPressed ? DeactivatedOn : DeactivatedOff;
        }
    }
}
