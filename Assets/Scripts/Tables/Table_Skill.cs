using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Table_SkillData
{
    public uint id;

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

    /// <summary>
    /// 애니메이터 트리거. 애니메이터 사용안하는 스킬은 비워둠.
    /// </summary>
    public string animTriggerName;

    public float damage;

    public string resPath;

    // 사용안함
    [Obsolete("구데이터")]
    public eSkillType skillType;
    

    // 사용안함
    [Obsolete("구데이터")]
    public float fireDelay;
}

public class Table_Skill : TableBase<Table_Skill>
{
    private Dictionary<uint, Table_SkillData> _data = null;

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
        data.damage = 1.2f;

        ///////////////////////////////////////////////////////////////////////////////
        /// 초기 생성 위치
        data.spawnPositionType = eSkillSpawnPositionType.Forward;
        ///////////////////////////////////////////////////////////////////////////////
        /// 생성 타입
        data.spawnType = eSkillSpawnType.Time;
        data.spawnTypeValue = 1f;
        ///////////////////////////////////////////////////////////////////////////////
        /// 이동 타입
        data.moveType = eSkillMoveType.Forward;
        data.moveTypeValue = 1;
        ///////////////////////////////////////////////////////////////////////////////
        /// 발사 타입
        data.fireType = eSkillFireType.OneShot;
        data.fireTypeValue = 0;

        ///////////////////////////////////////////////////////////////////////////////
        /// 애니메이터 트리거.
        data.animTriggerName = "doAttack";

        data.resPath = "Skill_1";
        _data.Add(data.id, data);



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        data = new Table_SkillData();
        data.id = 2;

        ///////////////////////////////////////////////////////////////////////////////
        data.damage = 1.2f;

        ///////////////////////////////////////////////////////////////////////////////
        /// 초기 생성 위치
        data.spawnPositionType = eSkillSpawnPositionType.Forward;
        ///////////////////////////////////////////////////////////////////////////////
        /// 생성 타입
        data.spawnType = eSkillSpawnType.Time;
        data.spawnTypeValue = 1.15f;
        ///////////////////////////////////////////////////////////////////////////////
        /// 이동 타입
        data.moveType = eSkillMoveType.Forward;
        data.moveTypeValue = 30;
        ///////////////////////////////////////////////////////////////////////////////
        /// 발사 타입
        data.fireType = eSkillFireType.OneShot;
        data.fireTypeValue = 0;

        ///////////////////////////////////////////////////////////////////////////////
        data.resPath = "Skill_1";

        _data.Add(data.id, data);

    }
}