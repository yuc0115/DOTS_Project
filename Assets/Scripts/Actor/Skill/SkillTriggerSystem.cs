using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SkillTriggerSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach (var (skillData, animator, entity) in SystemAPI.Query<RefRW<SkillData_Trigger>, ActorData_ModelAnimator>().WithEntityAccess())
        {
            if (skillData.ValueRO.isTrigger == false)
                continue;
            skillData.ValueRW.isTrigger = false;

            // 아이디에 맞는 애니메이터 트리거 켜줘야함.
            animator.animator.SetTrigger("doAttack");
        }
    }
}