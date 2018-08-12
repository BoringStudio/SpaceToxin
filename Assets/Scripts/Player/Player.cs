using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Stats")]
	public float Health;

	[Header("Hinge joint")]
	public Vector2 Anchor = new Vector2(0.01059294f, 0.2198076f);
	public HingeJoint2D PipeHolder
	{
		get
		{
            m_pipeHolder.connectedAnchor = Vector2.zero;
			if (m_pipeHolder != null) return m_pipeHolder;

			m_pipeHolder = gameObject.AddComponent<HingeJoint2D>();
			m_pipeHolder.anchor = Anchor;

			return m_pipeHolder;
		}
		set
		{
			m_pipeHolder = value;
		}
	}
	private HingeJoint2D m_pipeHolder = null;

	[HideInInspector]
	public Interactable CurrentInteractable = null;

	protected bool m_currentInteract = false;
	protected bool m_prevInteract = false;

	private void Start()
	{
		HingeJoint2D hingeJoint = GetComponent<HingeJoint2D>();
		if (hingeJoint != null)
		{
            hingeJoint.autoConfigureConnectedAnchor = false;
			Anchor = hingeJoint.anchor;
			PipeHolder = hingeJoint;
		}
	}

	private void Update()
	{
        if (Input.GetButtonDown("Interact"))
        {
            if (CurrentInteractable != null)
            {
                CurrentInteractable.OnInteractStart(this);
            }
        }

        if (Input.GetButtonUp("Interact"))
        {
            if (CurrentInteractable != null)
            {
                CurrentInteractable.OnInteractEnd(this);
            }
        }
    }
}
