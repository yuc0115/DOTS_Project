using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct SkillData_Trigger : IComponentData
{
    public NativeList<SkillData_TriggerItem> datas;
}

public struct SkillData_TriggerItem
{
    public uint id;
    public double spawnTime;
    public float spawnDelay;

    // 애니메이션이 있는지.
    public bool isAnimation;
}

public struct SkillData_Spawn : IComponentData  
{
    public NativeQueue<SkillData_SpawnItem> datas;
}

public struct SkillData_SpawnItem
{
    public uint id;
    public eActorType attackerType;
    public LocalTransform tr;
    public int atkPower;
}

public struct SkillData_MoveForward : IComponentData
{
    public float3 direction;
    public float speed;
}

public class SkillData_ModelTransform : ICleanupComponentData
{
    public Transform trasnform;
}

public struct SkillData_Attaker : IComponentData
{
    public eActorType actorType;
}


public struct SkillData_Damage : IComponentData
{
    public int damage;
}

public class SkillData_Hit : IComponentData
{
    public string hitEffect;
    public List<HitDataItem> hitDatas;
}

public struct HitDataItem
{
    public Entity hitEntity;
    public double hitTime;
}

public struct SkillData_HitCount : IComponentData
{
    public int count;
}

public struct SkillData_DestoryTime : IComponentData
{
    public double time;
}

public struct SkillData_PushPower : IComponentData
{
    public float power;
}


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