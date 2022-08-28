using UnityEngine;

public class PlayerArrow : Projecttile
{
    public static void Create(Transform firePoint, float damage, float arrowSpeed, float ttl)
    {
        Transform arrowObj = Instantiate(GameAssets.i.pfPlayyerArrow.transform, firePoint.position, firePoint.rotation);
        PlayerArrow arrow = arrowObj.GetComponent<PlayerArrow>();
        arrow.SetData(damage, arrowSpeed, ttl);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        IUnitHp unitHp = collision.GetComponent<IUnitHp>();
        if (!collision.CompareTag("Player") && unitHp != null)
        {
            unitHp.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    public override void Update()
    {
        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
        TTL -= Time.deltaTime;
    }
}
