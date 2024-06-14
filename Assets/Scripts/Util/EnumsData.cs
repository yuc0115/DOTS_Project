public enum eResDatas
{
    Player,
    Enemy,
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

#region skill
public enum eSkillSpawnType
{
    None = 0,
    Time,
}

public enum eSkillMoveType
{
    /// <summary>
    /// 이동하지 않는 경우 사용.
    /// </summary>
    None = 0,
    /// <summary>
    /// 시전자의 normal
    /// </summary>
    Forward,
    /// <summary>
    /// 가까운 적 탐지 후 이동.
    /// </summary>
    MoveToNearbyEnemy,
}

public enum eSkillSpawnPositionType
{
    None = 0,
    MyPos,
    Forward,
    ClosestTarget,
}

public enum eSkillFireType
{
    None = 0,
    OneShot,
}

public enum eSkillDestoryType
{
    None = 0,
    Hit,
    Time,
}

/// <summary>
/// 단일 타겟에 대한 히트 타입. 
/// </summary>
public enum eSkillHitType
{
     None = 0,
     HitCount = 0,
     HitWithIntervals,
}

#endregion

