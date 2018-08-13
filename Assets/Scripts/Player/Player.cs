using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Player player = target as Player;
        GUILayout.Label("Health: " + player.CurrentHealth);
    }
}
#endif

public class Player : MonoBehaviour
{
    public AudioClip[] GettingDamageSounds;

    [HideInInspector]
    public SafeDome CurrentSafeZone = null;

    public bool InSafeZone
    {
        get
        {
            return CurrentSafeZone != null;
        }
    }

    [Range(0.0f, 100.0f)]
	public float MaxHealth = 100.0f;

    [HideInInspector]
    public float CurrentHealth;

    public bool IsDead
    {
        get { return CurrentHealth <= 0.0f; }
    }

    [HideInInspector]
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

    private AudioSource m_audioSource;

    public DamageHUD DamageHUD;
    public HPBar HPIndicator;

	protected bool m_currentInteract = false;
	protected bool m_prevInteract = false;

	private void Start()
	{
        m_audioSource = GetComponent<AudioSource>();

        CurrentHealth = MaxHealth;

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
        if (HPIndicator != null)
        {
            HPIndicator.UpdateHP(CurrentHealth, MaxHealth);
        }

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

        if (Input.GetKeyDown(KeyCode.R) || IsDead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }

    private void LateUpdate()
    {
        CurrentSafeZone = null;
    }

    public void Damage(float amount)
    {
        if (InSafeZone) return;

        CurrentHealth -= amount;
        if (CurrentHealth < 0.0f)
        {
            CurrentHealth = 0.0f;
        }

        if (DamageHUD != null)
        {
            DamageHUD.TakeDamage();
        }

        if (m_audioSource != null && GettingDamageSounds != null && GettingDamageSounds.Length > 0 && Random.Range(0, 100) < 50)
        {
            m_audioSource.PlayOneShot(GettingDamageSounds[Random.Range(0, GettingDamageSounds.Length)]);
        }
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}
