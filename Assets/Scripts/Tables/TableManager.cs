using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    private bool _loaded = false;

    public void LoadTable()
    {
        if (_loaded)
            return;
        _loaded = true;

        Debug.Log("LoadTable");
        Table_Actors.instance.LoadTable("Data_Actors");
        Table_Skill.instance.LoadTable("Data_Skill");
        
    }
}

public class TableBase<T> where T : class
{
    public static T instance = Activator.CreateInstance(typeof(T)) as T;

    //public static T instance1 = new T();
    public virtual void LoadTable(string filePath)
    {

    }
}