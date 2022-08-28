using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KeepOnLoad : MonoBehaviour
{
    public static KeepOnLoad instace;
    [HideInInspector]
    public string objectID;

    private void Awake()
    {
        objectID = name;
    }
    private void Start()
    {
        for(int i = 0; i < FindObjectsOfType<KeepOnLoad>().Length; i++)
        {
            if (FindObjectsOfType<KeepOnLoad>()[i] != this
                && FindObjectsOfType<KeepOnLoad>()[i].objectID == objectID)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(this);
    }

    public void DestroyOnExit()
    {
        Destroy(gameObject);
    }
}
