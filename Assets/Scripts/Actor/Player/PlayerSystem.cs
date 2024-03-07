 using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerSystem : ISystem
{
    void OnUpdate(ref SystemState state) 
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach(var (stat, tr) in SystemAPI.Query<RefRO<ActorMoveStat>, RefRW<LocalTransform>>().WithAll<PlayerTag>())
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                float3 normal = new float3(horizontal, 0, vertical);
                tr.ValueRW = tr.ValueRO.Translate(normal * stat.ValueRO.moveSpeed * deltaTime);
                tr.ValueRW.Rotation = Quaternion.Lerp(tr.ValueRO.Rotation, Quaternion.LookRotation(normal), deltaTime * stat.ValueRO.rotSpeed);
            }
        }
    }
}
