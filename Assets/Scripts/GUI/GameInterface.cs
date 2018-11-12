using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameInterface : MonoBehaviour
{
    public HealthBar HealthBar { get; private set; }
    public ProgressBar ProgressBar { get; private set; }
    public DamageOverlay DamageOverlay { get; private set; }

    public static GameInterface Instance;

    void Awake()
    {
        Assert.IsNull(Instance, "There must be only one instance of GameInterface");

        HealthBar = GetComponentInChildren<HealthBar>();
        Assert.IsNotNull(HealthBar, "GUI must contain HealthBar");

        ProgressBar = GetComponentInChildren<ProgressBar>();
        Assert.IsNotNull(HealthBar, "GUI must contain ProgressBar");
        ProgressBar.gameObject.SetActive(false);

        DamageOverlay = GetComponentInChildren<DamageOverlay>();
        Assert.IsNotNull(DamageOverlay, "GUI must contain DamageOverlay");

        Instance = this;
    }
}
