public enum ePhysicsCategoryNames
{
    Terrain = 1,
    Player = 1 << 1,
    Enemy = 1 << 2,
    End = 1 << 3
}

public enum eActorState : byte
{
    None = 0,
    Idle,
    Move,
    Attack,
    End,
}
