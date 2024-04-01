using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Table_SkillData
{
    public uint id;

    public eSkillType skillType;
    public int damage;

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
        data.skillType = eSkillType.AutoSkill;
        data.damage = 5;
        data.fireDelay = 1.5f;

        _data.Add(data.id, data);

    }
}