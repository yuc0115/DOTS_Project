using Unity.Entities;

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
