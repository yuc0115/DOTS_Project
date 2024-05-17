using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;
using System.Text;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, IObjectPool<GameObject>> _poolDictionary = new Dictionary<string, IObjectPool<GameObject>>();

    private GameObject CreatePooledItem(GameObject prefab, string tag)
    {
        GameObject obj = Instantiate(prefab);
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetPooledObject(string folder, string tag)
    {
        if (_poolDictionary.ContainsKey(tag) == false)
        {
            var prefab = ResourceManager.Instance.LoadObject(string.Format("{0}/{1}", folder, tag));
            IObjectPool<GameObject> objectPool = new ObjectPool<GameObject>(
                () => CreatePooledItem(prefab, tag),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject);

            _poolDictionary.Add(tag, objectPool);
        }

        return _poolDictionary[tag].Get();
    }

    public void ReleasePooledObject(string poolTag, GameObject obj)
    {
        if (_poolDictionary.ContainsKey(poolTag) == false)
        {
            Debug.LogErrorFormat("pool is null tag : {0}", poolTag);
            return;
        }

        _poolDictionary[poolTag].Release(obj);
    }
}