using Unity.Entities;

#region ActorTag
public struct PlayerTag : IComponentData{ }
#endregion 

public struct ActorMoveStat : IComponentData
{
    public float moveSpeed;
    public float rotSpeed;
}