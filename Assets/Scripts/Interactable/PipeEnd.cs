using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePipe : Interactable {
    [HideInInspector]
    public Pipe Pipe;

	public Rigidbody2D Connector;

    private Player m_player;
    
	void Update ()
    {
        if (m_player != null && m_player.PipeHolder != null && Pipe.IsOversized)
        {
            m_player.PipeHolder.connectedBody = null;
        }
    }

	public override void OnInteractStart(Player player)
	{
		if (player.PipeHolder != null)
		{
            m_player = player;
			player.PipeHolder.connectedBody = Connector;
		}
	}

	public override void OnInteractEnd(Player player)
	{
	}
}
