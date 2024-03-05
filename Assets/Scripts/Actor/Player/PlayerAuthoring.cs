using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float rotSpeed;

    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            ActorMoveStat moveStat = default;
            moveStat.moveSpeed = authoring.moveSpeed;
            moveStat.rotSpeed = authoring.rotSpeed;
            AddComponent(entity, moveStat);
        }
    }
}
