using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct CamSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.HasSingleton<Cam>() == false)
            return;

        foreach (var tr in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
        {
            var camInfo = SystemAPI.GetSingleton<Cam>();
            float3 targetPos = tr.ValueRO.Position + camInfo.cameraOffset;
            float3 camPos = targetPos;

            Camera cam = Camera.main;
            cam.transform.position = camPos;
            cam.transform.rotation = Quaternion.LookRotation(math.normalize(tr.ValueRO.Position - camPos), new float3(0, 1, 0));
        }
    }
}
