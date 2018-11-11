using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Pipe))]
[CanEditMultipleObjects]
public class PipeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Pipe pipe = (Pipe)target;

		if (GUILayout.Button("Build pipe"))
		{
			pipe.GenerateSegments();
		}
    }
}
#endif

public class Pipe : MonoBehaviour {
	public ushort SegmentCount = 10;

	public Rigidbody2D PipeStartPrefab;
	public Rigidbody2D PipeSegmentPrefab;
	public Rigidbody2D PipeEndPrefab;

	public float StartOffset = 0.0785f;
	public float SegmentOffset = 0.157f;

    public float SizeCoefficient = 1.1f;

    [SerializeField]
    public float DefaultLength
    {
        get { return m_defaultLength; }
    }
    private float m_defaultLength = 0.0f;

    [SerializeField]
    public float CurrentLength
    {
        get
        {
            return m_currentLength;
        }
    }
    private float m_currentLength;

    public bool IsOversized
    {
        get
        {
            return m_currentLength > m_defaultLength * SizeCoefficient;
        }
    }

    private HingeJoint2D[] m_hingeJoints;

    void Start()
    {
        m_hingeJoints = GetComponentsInChildren<HingeJoint2D>();
        m_defaultLength = CalculateLength();
    }

    void Update()
    {
        m_currentLength = CalculateLength();
    }

    private float CalculateLength()
    {
        if (m_hingeJoints == null || m_hingeJoints.Length < 2) return 0.0f;

        float totalLength = 0.0f;
        Vector2 lastPoint = m_hingeJoints[0].transform.position;
        foreach (HingeJoint2D joint in m_hingeJoints)
        {
            Vector2 currentPoint = joint.transform.position;
            totalLength += (currentPoint - lastPoint).magnitude;

            lastPoint = currentPoint;
        }

        return totalLength;
    }

	public void GenerateSegments()
	{
		for (int i = transform.childCount; i > 0; --i)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}

        m_hingeJoints = new HingeJoint2D[SegmentCount + 1];

        // Spawn start
        Rigidbody2D pipeStart = Instantiate(PipeStartPrefab);
		pipeStart.transform.parent = transform;
		pipeStart.transform.localPosition = Vector3.zero;

		// Spawn segments
		Rigidbody2D lastSegment = pipeStart;
		for (int i = 0; i < SegmentCount; ++i)
		{
			Rigidbody2D pipeSegment = Instantiate(PipeSegmentPrefab);
            m_hingeJoints[i] = pipeSegment.GetComponent<HingeJoint2D>();

            //m_hingeJoints[i].useLimits = i != 0;

			pipeSegment.transform.parent = lastSegment.transform;
			pipeSegment.transform.localPosition = new Vector2(i == 0 ? StartOffset : SegmentOffset, 0.0f);

            m_hingeJoints[i].connectedBody = lastSegment;
			lastSegment = pipeSegment;
		}

		// Spawn end
		Rigidbody2D pipeEnd = Instantiate(PipeEndPrefab);
        m_hingeJoints[SegmentCount] = pipeEnd.GetComponent<HingeJoint2D>();
        m_hingeJoints[SegmentCount].useLimits = true;

		pipeEnd.transform.parent = lastSegment.transform;
        pipeEnd.transform.localPosition = new Vector2(SegmentOffset, 0.0f);

        m_hingeJoints[SegmentCount].connectedBody = lastSegment;

        //
        PipeEnd[] interactables = GetComponentsInChildren<PipeEnd>();
        foreach (PipeEnd interactablePipe in interactables)
        {
            interactablePipe.Pipe = this;
        }

        m_defaultLength = CalculateLength();
	}
}
