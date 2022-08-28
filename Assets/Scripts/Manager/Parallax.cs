using UnityEngine;

class Parallax : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private GameObject Camera;

    private float StartPos;
    private float Length;

    private void Start()
    {
        StartPos = transform.position.x;
        Length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = Camera.transform.position.x * (1 - Speed);
        float distanceX = Camera.transform.position.x * Speed;

        transform.position = new Vector3(StartPos + distanceX, transform.position.y, transform.position.z);

        if(temp > StartPos + Length)
        {
            StartPos += Length;
        }
        else if(temp < StartPos - Length)
        {
            StartPos -= Length;
        }
    }
}