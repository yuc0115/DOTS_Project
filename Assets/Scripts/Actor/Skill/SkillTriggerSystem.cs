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
        foreach (var (skillData, animator, entity) in SystemAPI.Query<RefRW<SkillTrigger>, ActorModelAnimator>().WithEntityAccess())
        {
            if (skillData.ValueRO.isTrigger == false)
                continue;
            skillData.ValueRW.isTrigger = false;

            //Debug.LogErrorFormat("SkillTriggerSystem entity : {0} , id : {1} , frame : {2}", entity, skillData.ValueRO.id, Time.frameCount);

            // 아이디에 맞는 애니메이터 트리거 켜줘야함.
            animator.animator.SetTrigger("doAttack");
        }
    }
}