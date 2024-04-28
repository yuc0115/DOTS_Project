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

    public T LoadInstantiateUI<T>(string name) where T : MonoBehaviour
    {
        GameObject go = Resources.Load(string.Format("UI/Panel/{0}", name)) as GameObject;
        if (go == null)
        {
            Debug.LogErrorFormat("LoadError : {0}", name);
            return null;
        }

        go = GameObject.Instantiate(go, GameObject.Find("Canvas").transform);

        T t = go.GetComponent<T>();
        if (t == null)
        {
            Debug.LogErrorFormat("UI ComponentError! : {0}", typeof(T));
            return null;
        }
        return t;
    }

    public GameObject LoadObjectInstantiate(string path)
    {
        GameObject go = LoadObject(path);
        if (go == null)
            return null;
        go = GameObject.Instantiate(go);
        return go;
    }

    public string GetPathActorRes()
    {
        return "Prefabs/Actor";
    }
}
