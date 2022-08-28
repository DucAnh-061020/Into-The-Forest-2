using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
    private void Start()
    {
        upgradeSelected = UpgradeChoose.None;
        GameEvents.WaveEndUpgrade += ActiveUpgrade;
        gameObject.SetActive(false);
    }

    private void Awake()
    {
    }

    private UpgradeChoose upgradeSelected;

    public void ConfirmUpgrade()
    {
        if (upgradeSelected == UpgradeChoose.None)
            return;
        Time.timeScale = 1;
        gameObject.SetActive(false);
        GameEvents.OnUpgradeInitiated(upgradeSelected);
        upgradeSelected = UpgradeChoose.None;
    }

    public void AttackUpgrade()
    {
        upgradeSelected = UpgradeChoose.Attack;
    }
    public void DefenseUpgrade()
    {
        upgradeSelected = UpgradeChoose.Defence;
    }
    public void HealthUpgrade()
    {
        upgradeSelected = UpgradeChoose.Health;
    }

    private void ActiveUpgrade()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        GameEvents.WaveEndUpgrade -= ActiveUpgrade;
    }
}

public enum UpgradeChoose
{
    Attack,
    Defence,
    Health,
    None
}