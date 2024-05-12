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
            //Debug.LogError(attaker.ValueRO.actorType);

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
            if (physicsWorld.OverlapSphere(tr.ValueRO.Position, size.ValueRO.radius, ref closestHitCollector, filter))
            {
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
                    var actorDataHP = state.EntityManager.GetComponentData<ActorData_HP>(collector.Entity);
                    actorDataHP.hp -= damage.ValueRO.damage;
                    state.EntityManager.SetComponentData<ActorData_HP>(collector.Entity, actorDataHP);

                    // 데미지 UI처리
                    var hitTr = state.EntityManager.GetComponentData<LocalTransform>(collector.Entity);
                    DamageManager.Instance.SpawnDamage(hitTr.Position, damage.ValueRO.damage);
                }
            }
        }
    }
}