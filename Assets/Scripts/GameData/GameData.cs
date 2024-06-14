using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public class Baker : Baker<GameData>
    {
        public override void Bake(GameData authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new GameDataTag());

            AddBuffer<GameData_Player>(entity);
            AddBuffer<GameData_Enemy>(entity);
        }
    }
}

public struct GameDataTag : IComponentData
{
}


public struct GameData_Player : IBufferElementData
{
    public Entity entity;
}

public struct GameData_Enemy : IBufferElementData
{
    public Entity entity;
}
