using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct SkillSystem : ISystem
{
    void OnUpdate(ref SystemState state) 
    {
        double elTime = SystemAPI.Time.ElapsedTime;
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (skillDestoryTime, entity) in SystemAPI.Query<RefRO<SkillData_DestoryTime>>().WithEntityAccess())
        {
            if (skillDestoryTime.ValueRO.time <= elTime)
            {
                ecb.DestroyEntity(entity);
            }
        }

        foreach(var (model, entity) in SystemAPI.Query<SkillData_ModelTransform>().WithNone<LocalTransform>().WithEntityAccess())
        {            
            if (model.trasnform != null)
                GameObject.Destroy(model.trasnform.gameObject);
            ecb.RemoveComponent<SkillData_ModelTransform>(entity);
        }
    }
}
