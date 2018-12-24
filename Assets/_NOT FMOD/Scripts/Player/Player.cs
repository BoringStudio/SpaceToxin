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
	public Interactable CurrentInteractable = null;


    private HealthBar m_healthBar;
    private DamageOverlay m_damageOverlay;

	protected bool m_currentInteract = false;
	protected bool m_prevInteract = false;

	private void Start()
	{
        m_healthBar = GameInterface.Instance.HealthBar;
        m_damageOverlay = GameInterface.Instance.DamageOverlay;

        CurrentHealth = MaxHealth;
	}

	private void Update()
	{
        if (m_healthBar)
        {
            m_healthBar.UpdateHP(CurrentHealth, MaxHealth);
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

        if (m_damageOverlay)
        {
            m_damageOverlay.TakeDamage();
        }
      
        if (Random.Range(0, 100) < 50)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(FMODPaths.COUGH, this.gameObject);
        }
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}
