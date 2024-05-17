using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    private static MainGame _instance;
    public static MainGame instance => _instance;

    private World _world;

    private void Awake()
    {
        TableManager.Instance.LoadTable();
        _world = World.DefaultGameObjectInjectionWorld;
        _instance = this;
    }

    void Start()
    {
        GameInitialize();   
    }

    private void GameInitialize()
    {
        Entity entity = _world.EntityManager.CreateEntity();
        _world.EntityManager.AddComponent<GameInitialzeSystem>(entity);

        GameManager.Instance.playerData.SetLevel(1);
    }
}
