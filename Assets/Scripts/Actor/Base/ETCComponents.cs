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
}

public struct GemSpawnData : IComponentData
{
    public float3 spawnPos;
    public bool isTrigger; 
}

#endregion