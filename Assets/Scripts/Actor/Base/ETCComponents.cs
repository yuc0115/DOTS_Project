using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


#region Gem
public struct GemTag : IComponentData
{

}

public struct GemData_Get : IComponentData, IEnableableComponent
{
    public float speed;
    public int exp;
}

public struct GemSpawnData : IComponentData
{
    public NativeQueue<GemSpawnItem> datas;
}
public struct GemSpawnItem
{
    public float3 spawnPos;
}

#endregion