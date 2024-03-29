using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public World _world;

    private void Awake()
    {
        //_world = World.DefaultGameObjectInjectionWorld;
    }

    void Start()
    {
        _world = World.DefaultGameObjectInjectionWorld;
        Entity entity = _world.EntityManager.CreateEntity();
        Debug.LogError(entity);
        _world.EntityManager.AddComponent<GameInitialzeSystem>(entity);
        //_world.EntityManager.AddComponent<>
    }
}
