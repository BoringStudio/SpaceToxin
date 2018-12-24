using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageOverlay : MonoBehaviour {

    public float FadeSpeed;

    public Image[] m_corners;

    private Color[] m_cornersCurColor = new Color[4];

    private Color m_fadeColor = new Color(229.0f, 0.0f, 0.0f, 0.0f);

    private bool m_hurt = false;

    private void Start()
    {
        m_corners = GetComponentsInChildren<Image>();

        for (int i = 0; i < m_cornersCurColor.Length; i++)
        {
            m_cornersCurColor[i] = new Color(229.0f, 0.0f, 0.0f, 255.0f);
        }
    }

    private void Update()
    {

        if (m_hurt)
        {
            for (int i = 0; i < m_corners.Length; i++)
            {
                m_corners[i].color = m_cornersCurColor[i];
            }
        }
        else
        {
            for (int i = 0; i < m_corners.Length; i++)
            {
                m_corners[i].color = Color.Lerp(m_corners[i].color, m_fadeColor, FadeSpeed * Time.deltaTime);
            }
        }

        m_hurt = false;
    }

    public void TakeDamage()
    {
        m_hurt = true;
    }
}
