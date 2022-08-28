using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider m_slider;
    public Gradient m_gradient;
    public Image m_fill;

    private void Start()
    {
        
    }
    private void Awake()
    {
        GameEvents.SetHealth += SetMaxHealth;
        GameEvents.UpdateHealth += SetHealth;
    }
    // Start is called before the first frame update
    public void SetMaxHealth(float maxHp, float hp)
    {
        try
        {
            m_slider.maxValue = maxHp;
            m_slider.value = hp;
            m_fill.color = m_gradient.Evaluate(1f);
        }
        catch { }
    }

    public void SetHealth(float health)
    {
        try
        {
            m_slider.value = health;
            m_fill.color = m_gradient.Evaluate(m_slider.normalizedValue);
        }
        catch { }
    }

    private void OnDestroy()
    {
        GameEvents.SetHealth += SetMaxHealth;
        GameEvents.UpdateHealth += SetHealth;
    }
}
