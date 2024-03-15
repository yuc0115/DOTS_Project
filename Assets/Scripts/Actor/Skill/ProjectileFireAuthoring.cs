using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Mathematics;
using UnityEngine;
using Unity.Physics.Authoring;

public class ProjectileFireAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject goRes;
    [SerializeField] private float delay = 0.3f;
    

    public class Baker : Baker<ProjectileFireAuthoring>
    {
        public override void Bake(ProjectileFireAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ProjectileFire
            {
                resEntity = GetEntity(authoring.goRes, TransformUsageFlags.Dynamic),
                spawnTime = authoring.delay,
                spawnDelay = authoring.delay
            });
        }
    }
}

