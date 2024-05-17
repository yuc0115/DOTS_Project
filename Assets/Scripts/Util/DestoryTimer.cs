using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryTimer : MonoBehaviour
{
    [SerializeField] private float destoryTime = 2;

    public void OnEnable()
    {
        Invoke("ReleaseObject", destoryTime);
    }

    private void ReleaseObject()
    {
        PoolManager.Instance.ReleasePooledObject(gameObject.name, gameObject);
    }
}
