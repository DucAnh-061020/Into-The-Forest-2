using UnityEngine;

public class Bullet : Projecttile
{
    public override void Update()
    {
        if (TTL <= 0)
        {
            gameObject.SetActive(false);
        }
        TTL -= Time.deltaTime;
    }


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        IUnitHp unitHp = collision.GetComponent<IUnitHp>();
        if (collision.CompareTag("Player"))
        {
            unitHp.TakeDamage(Damage);
            gameObject.SetActive(false);
        }
    }

    public override void SetOnFire(Transform position)
    {
        base.SetOnFire(position);
        transform.rotation = position.rotation;
        transform.eulerAngles = position.rotation.eulerAngles;
        m_Rigidbody2D.velocity = transform.right * Speed;
    }
}
