using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PipeSocket : Interactable {

    public bool IsConnected
    {
        get
        {
            return m_isConnected;
        }
        set
        {
            m_isConnected = value;

            if (m_animator == null) return;

            m_animator.SetBool("Connected", m_isConnected);
        }
    }
    private bool m_isConnected = false;

    public UnityEvent EventsOnConnect = null;

    private Animator m_animator;
    
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_animator = GetComponent<Animator>();
    }

    void OnInteractBegin(Collider2D collider)
    {
        if (collider.gameObject.tag != "Player") return;

        Player player = collider.GetComponent<Player>();
        if (player.PipeHolder.connectedBody != null)
        {
            Rigidbody2D body = player.PipeHolder.connectedBody;
            //player.PipeHolder.connectedBody = GetComponent<HingeJoint2D>();
        }
    }

    void OnInteractEnd(Collider2D collider)
    {
        if (collider.gameObject.tag != "Player") return;


    }
}
