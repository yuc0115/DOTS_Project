using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Table_SkillData
{
    public uint id;

    // 설명, 이름
    public string name;
    public string description;

    public string iconName;

    // 생성 타입
    public eSkillSpawnType spawnType;
    public float spawnTypeValue;

    // 초기 생성 위치.
    public eSkillSpawnPositionType spawnPositionType;

    // 이동 타입
    public eSkillMoveType moveType;
    public float moveTypeValue;

    // 발사 타입
    public eSkillFireType fireType;
    public float fireTypeValue;

    // 삭제 타입.
    public eSkillDestoryType destoryType;
    public int destoryValue;

    // 히트 타입.
    public eSkillHitType hitType;
    public float hitTypeValue;

    public string hitEffect;

    // Scale
    public float scale;

    // 미는 힘
    public float pushPower;

    /// <summary>
    /// 애니메이터 트리거. 애니메이터 사용안하는 스킬은 비워둠.
    /// </summary>
    public string animTriggerName;

    public float damage;

    public string resPath;

}

public class Table_Skill : TableBase<Table_Skill>
{
    private Dictionary<uint, Table_SkillData> _data = null;
    public Dictionary<uint, Table_SkillData> data { get { return _data; } }

    public override void LoadTable(string filePath)
    {
        _data = new Dictionary<uint, Table_SkillData>();
        TempData();
    }

    public Table_SkillData GetData(uint id)
    {
        Table_SkillData data = null;
        if (_data.TryGetValue(id, out data) == false)
        {
            Debug.LogErrorFormat("Table_ActorData KeyValue Error!! key : {0}", id);
        }

        return data;
    }

    private void TempData()
    {
        Table_SkillData data = null;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        data = new Table_SkillData();
        data.id = 1;

        ///////////////////////////////////////////////////////////////////////////////
        ///스킬 설명 및 이름
        data.name = "활 기본 공격";
        data.description = "전방으로 화살 1개 발사, 1히트 후 사라진다";
        data.iconName = "Arrow";
        data.scale = 1;

        ///////////////////////////////////////////////////////////////////////////////
        data.damage = 1.2f;

        ///////////////////////////////////////////////////////////////////////////////
        /// 초기 생성 위치
        data.spawnPositionType = eSkillSpawnPositionType.Forward;
        ///////////////////////////////////////////////////////////////////////////////
        /// 생성 타입
        data.spawnType = eSkillSpawnType.Time;
        data.spawnTypeValue = 0.5f;
        ///////////////////////////////////////////////////////////////////////////////
        /// 이동 타입
        data.moveType = eSkillMoveType.Forward;
        data.moveTypeValue = 30;
        ///////////////////////////////////////////////////////////////////////////////
        /// 발사 타입
        data.fireType = eSkillFireType.OneShot;
        data.fireTypeValue = 0;
        ///////////////////////////////////////////////////////////////////////////////
        /// 삭제 타입
        data.destoryType = eSkillDestoryType.Hit;
        data.destoryValue = 1;
        ///////////////////////////////////////////////////////////////////////////////
        /// 히트 타입
        data.hitType = eSkillHitType.HitCount;
        data.hitTypeValue = 1;

        data.hitEffect = "SkillHit_1";

        data.pushPower = 15;

        ///////////////////////////////////////////////////////////////////////////////
        /// 애니메이터 트리거.
        data.animTriggerName = "doAttack";

        data.resPath = "Skill_1";
        _data.Add(data.id, data);



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        data = new Table_SkillData();
        data.id = 2;

        ///////////////////////////////////////////////////////////////////////////////
        ///스킬 설명 및 이름
        data.name = "번개 구체";
        data.description = "전방으로 구체 발사 0.1초 간격으로 다단히트 된다";
        data.iconName = "BallLightning";
        data.scale = 2.0f;

        ///////////////////////////////////////////////////////////////////////////////
        data.damage = 2.2f;

        ///////////////////////////////////////////////////////////////////////////////
        /// 초기 생성 위치
        data.spawnPositionType = eSkillSpawnPositionType.Forward;
        ///////////////////////////////////////////////////////////////////////////////
        /// 생성 타입
        data.spawnType = eSkillSpawnType.Time;
        data.spawnTypeValue = 0.6f;
        ///////////////////////////////////////////////////////////////////////////////
        /// 이동 타입
        data.moveType = eSkillMoveType.Forward;
        data.moveTypeValue = 5;
        ///////////////////////////////////////////////////////////////////////////////
        /// 발사 타입
        data.fireType = eSkillFireType.OneShot;
        data.fireTypeValue = 0;
        ///////////////////////////////////////////////////////////////////////////////
        /// 삭제 타입
        data.destoryType = eSkillDestoryType.Time;
        data.destoryValue = 10;
        ///////////////////////////////////////////////////////////////////////////////
        /// 히트 타입
        data.hitType = eSkillHitType.HitWithIntervals;
        data.hitTypeValue = 0.1f;

        data.hitEffect = "SkillHit_2";

        data.pushPower = 5;

        data.resPath = "Skill_2";
        _data.Add(data.id, data);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        data = new Table_SkillData();
        data.id = 3;

        ///////////////////////////////////////////////////////////////////////////////
        ///스킬 설명 및 이름
        data.name = "벼락";
        data.description = "가까운 적에게 벼락 공격";
        data.iconName = "Lightning";
        data.scale = 3.0f;

        ///////////////////////////////////////////////////////////////////////////////
        data.damage = 2.0f;

        ///////////////////////////////////////////////////////////////////////////////
        /// 초기 생성 위치
        data.spawnPositionType = eSkillSpawnPositionType.ClosestTarget;
        ///////////////////////////////////////////////////////////////////////////////
        /// 생성 타입
        data.spawnType = eSkillSpawnType.Time;
        data.spawnTypeValue = 0.3f;
        ///////////////////////////////////////////////////////////////////////////////
        /// 이동 타입
        data.moveType = eSkillMoveType.None;
        data.moveTypeValue = 0;
        ///////////////////////////////////////////////////////////////////////////////
        /// 발사 타입
        data.fireType = eSkillFireType.OneShot;
        data.fireTypeValue = 0;
        ///////////////////////////////////////////////////////////////////////////////
        /// 삭제 타입
        data.destoryType = eSkillDestoryType.Time;
        data.destoryValue = 1;
        ///////////////////////////////////////////////////////////////////////////////
        /// 히트 타입
        data.hitType = eSkillHitType.HitCount;
        data.hitTypeValue = 1;

        data.hitEffect = "SkillHit_2";


        data.resPath = "Skill_3";
        _data.Add(data.id, data);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        data = new Table_SkillData();
        data.id = 4;

        ///////////////////////////////////////////////////////////////////////////////
        ///스킬 설명 및 이름
        data.name = "표창 던지기";
        data.description = "가까운 적에게 빠르게 공격한다";
        data.iconName = "Lightning";

        data.scale = 1.0f;

        ///////////////////////////////////////////////////////////////////////////////
        data.damage = 0.1f;

        ///////////////////////////////////////////////////////////////////////////////
        /// 초기 생성 위치
        data.spawnPositionType = eSkillSpawnPositionType.MyPos;
        ///////////////////////////////////////////////////////////////////////////////
        /// 생성 타입
        data.spawnType = eSkillSpawnType.Time;
        data.spawnTypeValue = 0.03f;
        ///////////////////////////////////////////////////////////////////////////////
        /// 이동 타입
        data.moveType = eSkillMoveType.MoveToNearbyEnemy;
        data.moveTypeValue = 15;
        ///////////////////////////////////////////////////////////////////////////////
        /// 발사 타입
        data.fireType = eSkillFireType.OneShot;
        data.fireTypeValue = 0;
        ///////////////////////////////////////////////////////////////////////////////
        /// 삭제 타입
        data.destoryType = eSkillDestoryType.Hit;
        data.destoryValue = 1;
        ///////////////////////////////////////////////////////////////////////////////
        /// 히트 타입
        data.hitType = eSkillHitType.HitCount;
        data.hitTypeValue = 1;

        data.hitEffect = "SkillHit_1";


        data.resPath = "Skill_4";
        _data.Add(data.id, data);
    }
}