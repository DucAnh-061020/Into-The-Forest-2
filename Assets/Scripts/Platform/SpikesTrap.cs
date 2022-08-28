using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null)
            return;
        if(collision.GetComponent<IUnitHp>() != null)
            collision.GetComponent<IUnitHp>().TakeDamage(damage);
    }
}
