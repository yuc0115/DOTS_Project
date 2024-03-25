using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerSpawnAuthoring : MonoBehaviour
{
    public GameObject goModel = null;

    public class Baker : Baker<PlayerSpawnAuthoring>
    {
        public override void Bake(PlayerSpawnAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerSpawnRes
            {
                entity = GetEntity(authoring.goModel, TransformUsageFlags.Dynamic)
            });
        }
    }
}

public struct PlayerSpawnRes : IComponentData
{
    public Entity entity;
}
