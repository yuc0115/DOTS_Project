using Unity.Entities;
using UnityEngine;

#region ActorTag
public struct PlayerTag : IComponentData{ }

public struct EnemyTag : IComponentData { }
#endregion 

public struct ActorMoveStat : IComponentData
{
    public float moveSpeed;
    public float rotSpeed;
}

public struct ActorAtkRangeStat : IComponentData
{
    public float minAtkRange;
    public float maxAtkRange;
}

public struct ActorTarget : IComponentData
{
    public Entity entity;
}

public struct ActorHP : IComponentData
{
    public float hp;
}

public struct ActorState :IComponentData
{
    public eActorState actorState;
}


#region GameObject
public struct IsActorInit : IComponentData, IEnableableComponent
{
    public uint actorTableID;
}

public class ActorModelTransform : ICleanupComponentData
{
    public Transform trasnform;
}

public class ActorModelAnimator : IComponentData
{
    public Animator animator;
}
#endregion

