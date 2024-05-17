using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceManager : Singleton<ResourceManager>
{
    public static string PathActor = "Prefabs/Actor";
    public static string PathSkill = "Prefabs/Skill";

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

    public GameObject LoadObjectInstantiate(string folder, string path)
    {
        GameObject go = PoolManager.Instance.GetPooledObject(folder, path);
        go.name = path;
        return go;
    }
}
