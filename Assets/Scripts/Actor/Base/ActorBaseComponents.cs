using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#region ActorTag
public struct PlayerTag : IComponentData{ }

public struct EnemyTag : IComponentData { }
#endregion 

public struct ActorData_MoveStat : IComponentData
{
    public float moveSpeed;
    public float rotSpeed;
}

public struct ActorData_AtkRangeStat : IComponentData
{
    public float minAtkRange;
    public float maxAtkRange;
}

public struct ActorData_Target : IComponentData
{
    public Entity entity;
}

public struct ActorData_AtkPower : IComponentData
{
    public int atkPower;
}

public struct ActorData_HP : IComponentData
{
    public float hp;
}

public struct ActorData_Push : IComponentData
{
    public float power;
    public float3 normal;
}

public struct ActorData_State :IComponentData
{
    public eActorState actorState;
}

#region GameObject
public struct IsActorInit : IComponentData, IEnableableComponent
{
    public uint actorTableID;
}

public class ActorData_ModelTransform : ICleanupComponentData
{
    public Transform trasnform;
}

public class ActorData_ModelAnimator : IComponentData
{
    public uint skillID;
    public Animator animator;
}
#endregion

