using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ProjectileFire : IComponentData
{
    public double spawnTime;
    public float spawnDelay;
}

public struct SkillData_Trigger : IComponentData
{
    public bool isTrigger;
    public bool isFire;
    public uint id;
}

public struct SkillData_Damage : IComponentData
{
    public int damage;
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

public struct SkillData_AutoSkill : IComponentData
{
    public uint skillID;
    public double fireTime;
    public float fireDelay;
}

public class SkillData_Hit : IComponentData
{
    public List<HitDataItem> hitDatas;
}

public struct HitDataItem
{
    public Entity hitEntity;
    public double hitTime;
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