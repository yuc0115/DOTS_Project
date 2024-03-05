using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CamAuthoring : MonoBehaviour
{
    public float3 cameraOffset;

    public class Baker : Baker<CamAuthoring>
    {
        public override void Bake(CamAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Cam
            {
                cameraOffset = authoring.cameraOffset,
            });
        }
    }
}

public struct Cam : IComponentData
{
    public float3 cameraOffset;
}

