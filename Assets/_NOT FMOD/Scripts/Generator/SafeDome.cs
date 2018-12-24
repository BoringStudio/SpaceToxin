using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeDome : MonoBehaviour
{
    private Player m_playerInside;

    void Update()
    {
        if (m_playerInside != null && m_playerInside.CurrentSafeZone != this)
        {
            m_playerInside.CurrentSafeZone = this;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        m_playerInside = collision.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        m_playerInside = null;
    }
}
