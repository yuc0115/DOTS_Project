using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ProjectileFire : IComponentData
{
    public double spawnTime;
    public float spawnDelay;
}

public struct Projectile : IComponentData
{
    public float speed;
    public float3 direction;
}

public struct ProjectileDestroyTime : IComponentData
{
    public double deleteTime;
}

#region GameObject
public class ProjectileModel : IComponentData
{
    public Transform transform;
}
#endregion 

#region tag
public struct SkillTag : IComponentData
{

}

public struct Dodge : IComponentData, IEnableableComponent
{
    public bool isTrigger;
    public float3 normal;
    public double endTime;
}

public struct ControllEnable : IComponentData, IEnableableComponent
{

}
#endregion tag