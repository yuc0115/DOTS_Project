public enum eResDatas
{
    Actor_normal,
    Projectile_Normal,
    Gem,
    End
}

public enum ePhysicsCategoryNames
{
    Terrain = 1,
    Player = 1 << 1,
    Enemy = 1 << 2,
    End = 1 << 3
}

public enum eActorType
{
    None = -1,
    Player,
    Enemy,
    End,
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

public enum eSkillSpawnType
{
    None = 0,
    Time,
}

public enum eSkillMoveType
{
    None = 0,
    Forward,
    RandomTarget,
}

public enum eSkillSpawnPositionType
{
    None = 0,
    Forward,
    RandomTarget,
}


public enum eSkillFireType
{
    None = 0,
    OneShot,
}


