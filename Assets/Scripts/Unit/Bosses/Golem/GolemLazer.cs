using System;
using UnityEngine;

public class GolemLazer : MonoBehaviour
{
    private float Damage;
    public bool CanDamage = false;

    public void SetData(float damage)
    {
        Damage = damage;
    }

    public void SetOnFire(Transform position)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = position.position;
        gameObject.transform.rotation = position.rotation;
    }

    private void EndLazer() => gameObject.SetActive(false);

    private void Remove() => Destroy(gameObject);

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!CanDamage)
            return;
        IUnitHp unitHp = collision.GetComponent<IUnitHp>();
        if (unitHp == null)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {
            unitHp.TakeDamage(Damage);
        }
    }
}
