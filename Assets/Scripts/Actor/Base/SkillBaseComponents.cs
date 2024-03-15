using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileFire : IComponentData
{
    public Entity resEntity;
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

#region tag
public struct SkillTag : IComponentData
{

}
#endregion tag