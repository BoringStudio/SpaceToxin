using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingArea : MonoBehaviour {

	public float Damage = 10.0f;
	public float HitDelay = 0.5f; // per seconds
	
	private float m_currentHitDuration = 0.0f;

    private Player m_playerInside;

    void Start ()
    {
		m_currentHitDuration = HitDelay;
	}

    void Update()
    {
        if (m_playerInside != null)
        {
            m_currentHitDuration -= Time.deltaTime;

            if (m_currentHitDuration <= 0)
            {
                m_playerInside.Damage(Damage);
                m_currentHitDuration = HitDelay;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        m_currentHitDuration = HitDelay;
        m_playerInside = collision.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;

        m_playerInside = null;
    }
}
