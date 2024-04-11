using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public World _world;

    private void Awake()
    {
        TableManager.Instance.LoadTable();
        _world = World.DefaultGameObjectInjectionWorld;
    }

    void Start()
    {
        Entity entity = _world.EntityManager.CreateEntity();
        _world.EntityManager.AddComponent<GameInitialzeSystem>(entity);
    }
}
