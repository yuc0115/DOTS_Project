using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SkillCollision : ISystem
{
    [BurstCompile]
    void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        var closestHitCollector = new NativeList<DistanceHit>(state.WorldUpdateAllocator);
        CollisionFilter filter = new CollisionFilter();

        var hpLookup = SystemAPI.GetComponentLookup<ActorData_HP>();
        var intervalLookup = SystemAPI.GetComponentLookup<SkillData_HitWithIntervals>();
        var hitCountLookup = SystemAPI.GetComponentLookup<SkillData_HitCount>();
        var pushPowerLookup = SystemAPI.GetComponentLookup<SkillData_PushPower>();
        var pushLookup = SystemAPI.GetComponentLookup<ActorData_Push>();
        var controllEnableLookup = SystemAPI.GetComponentLookup<ControllEnable>();

        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (attaker, tr, damage, hit, entity) in SystemAPI.Query<RefRO<SkillData_Attaker>, RefRO<LocalTransform>, RefRO<SkillData_Damage>, SkillData_Hit>().WithAll<SkillTag>().WithEntityAccess())
        {
            float colliderRadius = tr.ValueRO.Scale * 0.5f;

            switch (attaker.ValueRO.actorType)
            {
                case eActorType.Player:
                    filter.BelongsTo = (uint)ePhysicsCategoryNames.Player;
                    filter.CollidesWith = (uint)ePhysicsCategoryNames.Enemy;
                    break;

                case eActorType.Enemy:
                    filter.BelongsTo = (uint)ePhysicsCategoryNames.Enemy;
                    filter.CollidesWith = (uint)ePhysicsCategoryNames.Player;
                    break;

                default:
                    Debug.LogError(string.Format("ActorType Error : {0}", attaker.ValueRO.actorType));
                    continue;
            }

            if (!physicsWorld.OverlapSphere(tr.ValueRO.Position, colliderRadius, ref closestHitCollector, filter))
                continue;

            double elTime = SystemAPI.Time.ElapsedTime;

            // 중복 히트 처리.
            foreach (var collector in closestHitCollector)
            {
                bool found = false;
                int idx = 0;
                for (int i = 0; i < hit.hitDatas.Length; i++)
                {
                    if (hit.hitDatas[i].hitEntity == collector.Entity)
                    {
                        idx = i;
                        found = true;
                        break;
                    }
                }

                // 직접 할당을 피하는 코드
                if (!found)
                {
                    HitDataItem item = new HitDataItem { hitEntity = collector.Entity, hitTime = elTime };
                    hit.hitDatas.Add(item);
                }
                else
                {
                    if (intervalLookup.HasComponent(entity))
                    {
                        var interval = intervalLookup[entity];
                        if (hit.hitDatas[idx].hitTime + interval.interval <= elTime)
                        {
                            hit.hitDatas[idx] = new HitDataItem { hitEntity = hit.hitDatas[idx].hitEntity, hitTime = elTime };
                        }
                        else
                            continue;
                    }
                    else
                        continue;
                }

                // HP 감소
                if (!hpLookup.HasComponent(collector.Entity))
                {
                    Debug.LogError(string.Format("component is null!! entity : {0}", collector.Entity));
                    continue;
                }

                var actorDataHP = hpLookup[collector.Entity];
                actorDataHP.hp -= damage.ValueRO.damage;
                hpLookup[collector.Entity] = actorDataHP;

                // 데미지 표기
                var hitTr = SystemAPI.GetComponent<LocalTransform>(collector.Entity);
                DamageManager.Instance.SpawnDamage(hitTr.Position, damage.ValueRO.damage);

                // 히트 카운트 처리
                if (hitCountLookup.HasComponent(entity))
                {
                    var hitCount = hitCountLookup[entity];
                    hitCount.count--;

                    if (hitCount.count > 0)
                    {
                        hitCountLookup[entity] = hitCount;
                    }
                    else
                    {
                        ecb.DestroyEntity(entity);
                    }
                }

                // 푸쉬 처리
                if (pushPowerLookup.HasComponent(entity))
                {
                    float3 normal = math.normalize(new float3(hitTr.Position.x, 0, hitTr.Position.z) - new float3(tr.ValueRO.Position.x, 0, tr.ValueRO.Position.z));
                    var push = pushPowerLookup[entity];
                    var actorPush = pushLookup[collector.Entity];

                    actorPush.power = push.power;
                    actorPush.normal = normal;

                    pushLookup[collector.Entity] = actorPush;
                    controllEnableLookup.SetComponentEnabled(collector.Entity, false);
                }
            }
        }
    }
}