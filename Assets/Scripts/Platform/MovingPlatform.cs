using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MovingPlatform))]
public class MovingPlatformEditor : Editor
{
    private float m_gridSize = 0.25f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    public void OnSceneGUI()
    {
        MovingPlatform platform = (target as MovingPlatform);

        EditorGUI.BeginChangeCheck();

        GUIStyle centeredStyle = GUI.skin.label;
        centeredStyle.alignment = TextAnchor.UpperCenter;
        centeredStyle.fixedWidth = 100.0f;

        Handles.color = Color.white;
        Vector3 beginPosition = Handles.FreeMoveHandle(platform.BeginPosition, Quaternion.identity, 0.05f, Vector3.one * m_gridSize, Handles.DotHandleCap);
        Handles.Label(beginPosition + Vector3.up * 0.15f, "Begin", centeredStyle);

        Vector3 endPosition = Handles.FreeMoveHandle(platform.EndPosition, Quaternion.identity, 0.05f, Vector3.one * m_gridSize, Handles.DotHandleCap);
        Handles.Label(endPosition + Vector3.up * 0.15f, "End", centeredStyle);

        if (EditorGUI.EndChangeCheck())
        {
            float invGrid = 1.0f / m_gridSize;

            float x = Mathf.Round(beginPosition.x * invGrid) / invGrid;
            float y = Mathf.Round(beginPosition.y * invGrid) / invGrid;
            platform.BeginPosition = new Vector3(x, y, beginPosition.z);

            x = Mathf.Round(endPosition.x * invGrid) / invGrid;
            y = Mathf.Round(endPosition.y * invGrid) / invGrid;
            platform.EndPosition = new Vector3(x, y, endPosition.z);
        }

        Handles.DrawLine(platform.BeginPosition, platform.EndPosition);
    }
}
#endif


public class MovingPlatform : MonoBehaviour
{
    public float Speed;
    public bool Loop;

    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));

    public Vector3 BeginPosition;
    public Vector3 EndPosition;

    public bool IsActivated
    {
        get
        {
            return m_isActivated;
        }
    }
    private bool m_isActivated;

    public bool IsReversed { get; set; }

    private float m_progress;

    private Vector3 m_offset;

    void Start ()
    {
        m_offset = transform.position - (!IsReversed ? BeginPosition : EndPosition);

        m_isActivated = true;
        Switch activator = GetComponent<Switch>();
        m_isActivated = (activator == null ? true : activator.CurrentState);
        activator.OnChangeState = (bool state) => { m_isActivated = state; };
    }
	
	void Update ()
    {
        if (!m_isActivated)
        {
            return;
        }

        transform.position = m_offset + BeginPosition + Curve.Evaluate(m_progress) * (EndPosition - BeginPosition);

        if (!IsReversed)
        {
            m_progress = Mathf.Clamp01(m_progress + Time.deltaTime * Speed);
            if (m_progress == 1.0f)
            {
                IsReversed = true;
            }
        }
        else
        {
            m_progress = Mathf.Clamp01(m_progress - Time.deltaTime * Speed);
            if (m_progress == 0.0f)
            {
                IsReversed = false;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        Player player;
        if ((player = collider.gameObject.GetComponent<Player>()) == null)
        {
            return;
        }

        Debug.Log("Collision begin");

        player.transform.parent = transform;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        Player player;
        if ((player = collider.gameObject.GetComponent<Player>()) == null)
        {
            return;
        }

        Debug.Log("Collision end");

        player.transform.parent = null;
    }
}
