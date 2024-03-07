using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public GameObject LoadObject(string path)
    {
        GameObject go = Resources.Load(path) as GameObject;
        if (go == null)
        {
            Debug.LogErrorFormat("LoadError : {0}", path);
            return null;
        }
        return go;
    }

    public GameObject LoadObjectInstantiate(string path)
    {
        GameObject go = LoadObject(path);
        if (go == null)
            return null;
        go = GameObject.Instantiate(go);
        return go;
    }
}
