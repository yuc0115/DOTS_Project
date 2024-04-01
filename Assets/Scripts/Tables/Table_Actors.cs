using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table_ActorData
{
    public uint id;
    
    // 스텟 관련
    public int hp;
    public float moveSpeed;
    public float rotSpeed;
    public int damage;

    public float minAtkRange;
    public float maxAtkRange;
    
    public uint[] skills;
    public float spawnDelay;
    public string resPath;
}

public class Table_Actors : TableBase<Table_Actors>
{
    private Dictionary<uint, Table_ActorData> _data = null;


    public override void LoadTable(string filePath)
    {
        _data = new Dictionary<uint, Table_ActorData>();
        TempData();
    }

    public Table_ActorData GetData(uint id)
    {
        Table_ActorData data;
        if (_data.TryGetValue(id, out data) == false)
        {
            Debug.LogErrorFormat("Table_ActorData KeyValue Error!! key : {0}", id);
        }

        return data;
    }

    public void TempData()
    {
        Table_ActorData data;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        data = new Table_ActorData();
        data.id = 1;

        data.hp = 100;
        data.moveSpeed = 10;
        data.rotSpeed = 20;
        data.damage = 0;
        data.minAtkRange = 100;
        data.maxAtkRange = 100;

        data.skills = new uint[] {1};
        data.spawnDelay = 0f;
        data.resPath = "PlayerModel";

        _data.Add(data.id, data);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        data = new Table_ActorData();
        data.id = 2;

        data.hp = 100;
        data.moveSpeed = 8;
        data.rotSpeed = 20;
        data.damage = 10;
        data.minAtkRange = 3;
        data.maxAtkRange = 5;

        data.skills = new uint[] { 1 };
        data.spawnDelay = 0f;
        data.resPath = "Knight_Small";

        _data.Add(data.id, data);
    }
}