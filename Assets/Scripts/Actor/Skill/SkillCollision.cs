using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public partial struct SkillCollision : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        NativeList<DistanceHit> closestHitCollector = new NativeList<DistanceHit>(state.WorldUpdateAllocator);
        CollisionFilter filter = new CollisionFilter();
        foreach (var (attaker, tr, size, damage, hit, entity) in SystemAPI.Query<RefRO<SkillData_Attaker>, RefRO<LocalTransform>, RefRO<SkillData_PrefabSize>, RefRO<SkillData_Damage>, SkillData_Hit>().WithAll<SkillTag>().WithEntityAccess())
        {
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
                    Debug.LogError(attaker.ValueRO.actorType);
                    break;
            }

            //float3 size = tr.Scale;
            //if (physicsWorld.OverlapBox(tr.ValueRO.Position, tr.ValueRO.Rotation, size.ValueRO.radius, ref closestHitCollector, filter))
            //{
            //    foreach (var pair in closestHitCollector)
            //    {
            //        Debug.LogError(pair.Entity);
            //    }
            //}

            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            // 스킬 히트처리 구충돌.
            if (physicsWorld.OverlapSphere(tr.ValueRO.Position, size.ValueRO.radius, ref closestHitCollector, filter) == false)
                continue;
            foreach (var collector in closestHitCollector)
            {
                //pair.Entity
                if (hit.hitDatas.FindIndex(x => x.hitEntity == collector.Entity) != -1)
                    continue;

                HitDataItem item = new HitDataItem();
                item.hitEntity = collector.Entity;
                item.hitTime = SystemAPI.Time.ElapsedTime;

                hit.hitDatas.Add(item);

                // hp 감소
                if (state.EntityManager.HasComponent<ActorData_HP>(collector.Entity) == false)
                {
                    Debug.LogErrorFormat("component is null!! entity : {0}", collector.Entity);
                    continue;
                }
                ActorData_HP actorDataHP = state.EntityManager.GetComponentData<ActorData_HP>(collector.Entity);
                actorDataHP.hp -= damage.ValueRO.damage;
                state.EntityManager.SetComponentData<ActorData_HP>(collector.Entity, actorDataHP);

                // 데미지 UI처리
                LocalTransform hitTr = state.EntityManager.GetComponentData<LocalTransform>(collector.Entity);
                DamageManager.Instance.SpawnDamage(hitTr.Position, damage.ValueRO.damage);

                // 히트 이펙트 처리.
                GameObject go = ResourceManager.Instance.LoadObjectInstantiate(ResourceManager.PathSkill, hit.hitEffect);
                go.transform.position = collector.Position;

                // 히트 카운트 처리.
                if (state.EntityManager.HasComponent<SkillData_HitCount>(entity))
                {
                    SkillData_HitCount hitCount = state.EntityManager.GetComponentData<SkillData_HitCount>(entity);
                    hitCount.count--;
                    if (hitCount.count > 0)
                    {
                        state.EntityManager.SetComponentData<SkillData_HitCount>(entity, hitCount);
                    }
                    else
                    {
                        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
                        ecb.DestroyEntity(entity);
                    }
                }

                if (state.EntityManager.HasComponent<SkillData_PushPower>(entity))
                {
                    float3 normal = math.normalize(new float3(hitTr.Position.x, 0, hitTr.Position.z) -
                        new float3(tr.ValueRO.Position.x, 0, tr.ValueRO.Position.z));
                    Debug.LogError(normal.y);
                    var push = state.EntityManager.GetComponentData<SkillData_PushPower>(entity);

                    var actorPush = state.EntityManager.GetComponentData<ActorData_Push>(collector.Entity);
                    actorPush.power = push.power;
                    actorPush.normal = normal;
                    state.EntityManager.SetComponentData<ActorData_Push>(collector.Entity, actorPush);
                    state.EntityManager.SetComponentEnabled<ControllEnable>(collector.Entity, false);
                }
            }
        }
    }
}