using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Generator))]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Generator generator = (target as Generator);

        GUILayout.Label("Fullness: " + generator.Fullness);
    }

    public void OnSceneGUI()
    {
        Generator generator = (target as Generator);

        EditorGUI.BeginChangeCheck();
        float maxRadius = Handles.RadiusHandle(Quaternion.identity, generator.transform.position, generator.MaxRadius);
        if (EditorGUI.EndChangeCheck())
        {
            generator.MaxRadius = maxRadius;
            generator.UpdateDomeRadius();
        }
    }
}
#endif

[ExecuteInEditMode]
public class Generator : MonoBehaviour
{
    [Range(0, 1)]
    public float InitiallyFilled = 1.0f;

    [Range(0, 1)]
    public float Consumption = 0.1f;

    public float MaxRadius = 1.0f;    

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
            m_isActivated = value;

            if (m_animator == null) return;

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
            m_fullness = Mathf.Clamp(value, 0.0f, 1.0f);

            if (m_animator == null) return;

            m_animator.SetFloat("Fullness", m_fullness);
        }
    }
    private float m_fullness;

    public UnityEvent EventsOnEmpty = null;
    
    private Animator m_animator;
    private bool m_shouldInvokeEvents = true;

    public SafeDome m_safeDome;

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
        UpdateDomeRadius();

        if (m_safeDome != null) m_safeDome.gameObject.SetActive(IsWorking);
        if (IsWorking)
        {
            Fullness = Fullness - Time.deltaTime * Consumption;
        }

        if (Fullness == 0 && m_shouldInvokeEvents)
        {
            EventsOnEmpty.Invoke();
            m_shouldInvokeEvents = false;
        }

        if (Fullness != 0 && !m_shouldInvokeEvents)
        {
            m_shouldInvokeEvents = true;
        }
    }

    public void UpdateDomeRadius()
    {
        if (m_safeDome == null) return;

        float s = MaxRadius * (2.0f - 2.0f / (Fullness + 1));
        m_safeDome.transform.localScale = new Vector3(s, s, 0.0f);
    }
}
