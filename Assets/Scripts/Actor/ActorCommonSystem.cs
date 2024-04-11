using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ActorCommonSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        // 히트 데미지 처리.
        foreach (var (hit, tr, actorHP, entity) in SystemAPI.Query<RefRW<ActorData_Hit>, RefRO<LocalTransform>, RefRW<ActorData_HP>>().WithEntityAccess())
        {
            if (hit.ValueRO.trigger == false)
                continue;
            hit.ValueRW.trigger = false;

            bool isDamage = false;
            var skillHitData = state.EntityManager.GetComponentData<SkillData_Hit>(hit.ValueRO.attacker);
            if (skillHitData.hitDatas.FindIndex(x => x.hitEntity == entity) == -1)
            {
                isDamage = true;
                HitDataItem item = new HitDataItem();
                item.hitTime = SystemAPI.Time.ElapsedTime;
                item.hitEntity = entity;
                skillHitData.hitDatas.Add(item);
                state.EntityManager.SetComponentData<SkillData_Hit>(hit.ValueRO.attacker, skillHitData);
            }
            else // 데미지 인터벌 처리 해야함.
            {
                
            }

            if (isDamage)
            {
                Debug.LogError("HP 임시 0 처리.");
                //actorHP.ValueRW.hp -= hit.ValueRO.damage;
                actorHP.ValueRW.hp = 0;
                DamageManager.Instance.SpawnDamage(tr.ValueRO.Position, hit.ValueRO.damage);
            }
        }
    }
}