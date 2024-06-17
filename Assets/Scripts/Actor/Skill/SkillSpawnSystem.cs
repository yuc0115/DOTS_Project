using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SkillSpawnSystem : ISystem
{
    private ComponentLookup<LocalTransform> _trLookup;

    void OnCreate(ref SystemState state)
    {
        _trLookup = state.GetComponentLookup<LocalTransform>();
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        foreach(var spawnData in SystemAPI.Query<RefRO<SkillData_Spawn>>())
        {
            var spawnItems = spawnData.ValueRO.datas;
            while(spawnItems.Count > 0)
            {
                var item = spawnItems.Dequeue();
                SkillSpawn(ref state, item);
            }
        }
    }

    [BurstCompile]
    private void SkillSpawn(ref SystemState state, SkillData_SpawnItem spawnData)
    {
        var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var skillEntity = ecb.Instantiate(resBuffer[(int)eResDatas.Projectile_Normal].prefab);
        var skillTable = Table_Skill.instance.GetData(spawnData.id);

        ecb.SetName(skillEntity, string.Format("SkillID_{0}", skillTable.id));

        float scale = skillTable.scale;
        double elTime = SystemAPI.Time.ElapsedTime;

        float3 spawnPos = float3.zero;
        quaternion spawnRotation = quaternion.identity;
        SetSpawnTransform(ref state, spawnData.casterType, in ecb, in skillEntity, in skillTable, spawnData.tr, scale, out spawnPos, out spawnRotation);

        SetMoveType(ref state, in ecb, in spawnData.caster, in skillEntity, in skillTable, spawnData.tr);

        SetGOModel(in ecb, in skillEntity, scale, spawnPos, spawnRotation, skillTable.resPath);

        SetDestoryType(in ecb, in skillEntity, in skillTable, elTime);

        SetHitType(in ecb, in skillEntity, in skillTable);

        ecb.AddComponent(skillEntity, new SkillData_Attaker { actorType = spawnData.casterType });

        // 스킬 데미지. 
        ecb.AddComponent(skillEntity, new SkillData_Damage { damage = (int)(skillTable.damage * spawnData.atkPower) });

        // 태그.
        ecb.AddComponent(skillEntity, new SkillTag());

        // 히트된 애들 저장용.
        ecb.AddComponent(skillEntity, new SkillData_Hit { hitEffect = skillTable.hitEffect, hitDatas = new NativeList<HitDataItem>(Allocator.Persistent)});

        if (skillTable.pushPower > 0)
            ecb.AddComponent(skillEntity, new SkillData_PushPower { power = skillTable.pushPower });
    }

    [BurstCompile]
    private void SetHitType(in EntityCommandBuffer ecb, in Entity skillEntity, in Table_SkillData tableData)
    {
        switch (tableData.hitType)
        {
            case eSkillHitType.HitCount:
                /// 별다른 처리 안함.
                break;

            case eSkillHitType.HitWithIntervals:
                ecb.AddComponent(skillEntity, new SkillData_HitWithIntervals { interval = tableData.hitTypeValue });
                break;

            default:
                Debug.LogError(string.Format("처리 안됌 : {0}", tableData.hitType));
                break;
        }
    }

    [BurstCompile]
    private void SetDestoryType(in EntityCommandBuffer ecb, in Entity skillEntity, in Table_SkillData tableData, double elTime)
    {
        switch (tableData.destoryType)
        {
            case eSkillDestoryType.Hit:
                ecb.AddComponent(skillEntity, new SkillData_HitCount { count = tableData.destoryValue });
                break;

            case eSkillDestoryType.Time:
                ecb.AddComponent(skillEntity, new SkillData_DestoryTime { time = elTime + tableData.destoryValue });
                return;

            default:
                Debug.LogError(string.Format("타입 처리 안됌 : {0}", tableData.destoryType));
                break;
        }
        // 삭제 관련
        ecb.AddComponent(skillEntity, new SkillData_DestoryTime { time = elTime + 10 });
    }

    [BurstDiscard]
    private void SetGOModel(in EntityCommandBuffer ecb, in Entity skillEntity, float scale, float3 spawnPos, quaternion rotation, string prefabPath)
    {
        GameObject goModel = ResourceManager.Instance.LoadObjectInstantiate(ResourceManager.PathSkill, prefabPath);
        Transform tr = goModel.transform;
        tr.position = spawnPos;
        tr.rotation = rotation;
        tr.localScale = Vector3.one * scale;

        ecb.AddComponent(skillEntity, new SkillData_ModelTransform { trasnform = tr });
    }

    [BurstDiscard]
    private void SetMoveType(ref SystemState state, in EntityCommandBuffer ecb, in Entity caster, in Entity skillEntity, in Table_SkillData tableData, LocalTransform actorTr)
    {
        switch(tableData.moveType)
        {
            // 이동 하지 않는 경우
            case eSkillMoveType.None:
                break;

            case eSkillMoveType.Forward:
                ecb.AddComponent(skillEntity, new SkillData_MoveForward
                { 
                    direction = actorTr.Forward(),
                    speed = tableData.moveTypeValue
                });
                break;

            case eSkillMoveType.MoveToNearbyEnemy:
                Entity gameDataEntity = SystemAPI.GetSingletonEntity<GameDataTag>();
                float3 myPos = actorTr.Position;
                float3 enemyPos = float3.zero;
                Entity closestEnemy = Entity.Null;

                _trLookup.Update(ref state);
                var buffer = state.EntityManager.GetBuffer<GameData_Enemy>(gameDataEntity);
                Util.FindClosestEntity(buffer, myPos, ref _trLookup, x => x.entity, out closestEnemy, out enemyPos);
                
                ecb.AddComponent(skillEntity, new SkillData_MoveForward
                {
                    direction = math.normalize(enemyPos - myPos),
                    speed = tableData.moveTypeValue
                });
                break;

            case eSkillMoveType.RandomBouncingSphere:
                float3 randomVector = new float3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
                randomVector = math.normalize(randomVector);
                ecb.AddComponent(skillEntity, new SkillData_RandomBouncingSphere
                {
                    caster = caster,
                    direction = randomVector,
                    doundary = 20,
                    speed = tableData.moveTypeValue
                });
                break;

            case eSkillMoveType.RotationAround:
                ecb.AddComponent(skillEntity, new SkillData_RotationAround
                {
                    caster = caster,
                    speed = tableData.moveTypeValue
                });
                break;

            default:
                Debug.LogErrorFormat("tableData.MoveType Error tableID : {0} type : {1}", tableData.id, tableData.moveType);
                break;
        }
    }

    [BurstDiscard]
    private void SetSpawnTransform(ref SystemState state, eActorType attakerType, in EntityCommandBuffer ecb, in Entity skillEntity, in Table_SkillData tableData, LocalTransform actorTr, float scale, out float3 spawnPos, out quaternion rotation)
    {
        spawnPos = float3.zero;
        rotation = quaternion.identity;
        LocalTransform tr;
        switch (tableData.spawnPositionType)
        {
            case eSkillSpawnPositionType.MyPos:
                spawnPos = actorTr.Position + new float3(0, 1, 0);
                rotation = actorTr.Rotation;
                tr = new LocalTransform
                {
                    Position = spawnPos,
                    Scale = scale,
                    Rotation = rotation
                };
                break;

            case eSkillSpawnPositionType.Forward:
                spawnPos = actorTr.Position + new float3(0, 1, 0) + actorTr.Forward();
                rotation = actorTr.Rotation;
                tr = new LocalTransform
                {
                    Position = spawnPos,
                    Scale = scale,
                    Rotation = rotation
                }; 
                break;

            case eSkillSpawnPositionType.ClosestTarget:

                Entity gameDataEntity = SystemAPI.GetSingletonEntity<GameDataTag>();
                float3 myPos = actorTr.Position;
                rotation = actorTr.Rotation;
                if (attakerType == eActorType.Player)
                {
                    var buffer = state.EntityManager.GetBuffer<GameData_Enemy>(gameDataEntity);
                    Entity closestEnemy = Entity.Null;
                    _trLookup.Update(ref state);

                    Util.FindClosestEntity(buffer, myPos, ref _trLookup, x => x.entity, out closestEnemy, out spawnPos);
                }
                else // 공격자가 적일때.
                {
                    var buffer = state.EntityManager.GetBuffer<GameData_Player>(gameDataEntity);
                    Entity closestPlayer = Entity.Null;
                    _trLookup.Update(ref state);

                    Util.FindClosestEntity(buffer, myPos, ref _trLookup, x => x.entity, out closestPlayer, out spawnPos);
                }

                tr = new LocalTransform
                {
                    Position = spawnPos,
                    Scale = scale,
                    Rotation = rotation
                };
                break;

            case eSkillSpawnPositionType.CasterNear:
                Debug.Log("여러발 나갈경우 CasterNear 이거 손봐야함");
                spawnPos = actorTr.Position + new float3(0, 1, 3);
                rotation = actorTr.Rotation;
                tr = new LocalTransform
                {
                    Position = spawnPos,
                    Scale = scale,
                    Rotation = rotation
                };
                break;

            default:
                Debug.LogError(string.Format("moveType 처리안됌 : {0}", tableData.spawnPositionType));
                return;
        }

        ecb.SetComponent(skillEntity, tr);
    }
}