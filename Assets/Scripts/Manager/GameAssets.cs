using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            return _i;
        }
    }

    private void Start()
    {
        if(_i == null)
        {
            _i = this;
        }
        if(_i != this)
        {
            Destroy(gameObject);
        }
    }

    public GameObject pfDamagePopup;
    public GameObject pfPlayyerArrow;
    public GameObject pfGolemProjectile;
    public GameObject pfGolemProjectileGlow;
    public GameObject pfGolemLazer;
    public GameObject pfArcherArrow;
    public List<GameObject> enemies;
}
