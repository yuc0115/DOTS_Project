using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;
using Unity.Mathematics;

public partial struct TestSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach(var pair in SystemAPI.Query<SystemAPI.ManagedAPI.UnityEngineComponent<TestMono>>())
        {
            Debug.LogError("asdfasfasfasfASdfasdf");

            pair.Value.text.text = "wqfqwef";

            
        }
    }
}
