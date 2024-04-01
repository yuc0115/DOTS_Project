public enum eResDatas
{
    Actor_normal,
    Projectile_Normal,
    End
}

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
    Die,
    End,
}

public enum eSkillType
{
    None = 0,
    AutoSkill,
}
