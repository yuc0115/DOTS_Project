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
        double elTime = SystemAPI.Time.ElapsedTime;
        foreach (var (item, tr, atkPower, anim) in SystemAPI.Query<RefRW<SkillData_Trigger>, RefRO<LocalTransform>, RefRO<ActorData_AtkPower>, ActorData_ModelAnimator>())
        {
            int length = item.ValueRO.datas.Length;
            for (int i = 0; i < length; i++)
            {
                var data = item.ValueRO.datas[i];
                if (data.spawnTime <= elTime)
                {
                    data.spawnTime = elTime + data.spawnDelay;
                    item.ValueRW.datas[i] = data;
                    var actorMono = anim.animator.GetComponent<ActorBaseMono>();

                    if (data.isAnimation)
                    {
                        actorMono.AddSkillSpawnID(data.id, atkPower.ValueRO.atkPower);
                        anim.animator.SetTrigger(Table_Skill.instance.GetData(data.id).animTriggerName);
                    }
                    else // 애니 없는경우 바로 스폰.
                    {
                        var skillSpawn = SystemAPI.GetSingleton<SkillData_Spawn>();
                        SkillData_SpawnItem spawnItem = new SkillData_SpawnItem();
                        spawnItem.attackerType = actorMono._actorType;
                        spawnItem.skillID = data.id;
                        spawnItem.tr = tr.ValueRO;
                        spawnItem.atkPower = atkPower.ValueRO.atkPower;
                        skillSpawn.datas.Enqueue(spawnItem);
                    }
                }
            }
        }
    }
}