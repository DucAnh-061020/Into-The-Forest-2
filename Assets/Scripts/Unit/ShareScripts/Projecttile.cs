using UnityEngine;

public abstract class Projecttile : MonoBehaviour
{
    public float Damage { private set; get; }
    public float Speed { private set; get; }
    public float TTL { set; get; }
    public Rigidbody2D m_Rigidbody2D { private set; get; }

    public void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        m_Rigidbody2D.velocity = transform.right * Speed;
    }

    public virtual void Update()
    {
        if (TTL <= 0)
        {
            Destroy(gameObject);
        }
        TTL -= Time.deltaTime;
    }

    public void SetData(float damage, float speed, float ttl)
    {
        Damage = damage;
        Speed = speed;
        TTL = ttl;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        IUnitHp unitHp = collision.GetComponent<IUnitHp>();
        if (unitHp == null)
        {
            return;
        }
    }

    public virtual void SetOnFire(Transform position)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = position.position;
    }
}
