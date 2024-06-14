using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SkillSpawnSystem : ISystem
{
    private ComponentLookup<LocalTransform> _trLookup;

    void OnCreate(ref SystemState state)
    {
        _trLookup = state.GetComponentLookup<LocalTransform>();
    }

    void OnUpdate(ref SystemState state)
    {
        foreach(var spawnData in SystemAPI.Query<RefRO<SkillData_Spawn>>())
        {
            var spawnItems = spawnData.ValueRO.datas;
            while(spawnItems.Count > 0)
            {
                var item = spawnItems.Dequeue();
                SkillSpawn(ref state, item.attackerType, item.id, item.tr, item.atkPower);
            }
        }
    }

    private void SkillSpawn(ref SystemState state, eActorType attakerType, uint id, LocalTransform tr, int atkPower)
    {
        var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Projectile_Normal].prefab);
        var skillTable = Table_Skill.instance.GetData(id);

        ecb.SetName(entity, string.Format("SkillID_{0}", skillTable.id));

        float scale = skillTable.scale;
        double elTime = SystemAPI.Time.ElapsedTime;

        float3 spawnPos = float3.zero;
        quaternion spawnRotation = quaternion.identity;
        SetSpawnTransform(ref state, attakerType, in ecb, in entity, in skillTable, tr, scale, out spawnPos, out spawnRotation);

        SetMoveType(ref state, in ecb, in entity, in skillTable, tr);

        SetGOModel(in ecb, in entity, scale, spawnPos, spawnRotation, skillTable.resPath);

        SetDestoryType(in ecb, in entity, in skillTable, elTime);

        SetHitType(in ecb, in entity, in skillTable);

        ecb.AddComponent(entity, new SkillData_Attaker { actorType = attakerType });

        // 스킬 데미지. 
        ecb.AddComponent(entity, new SkillData_Damage { damage = (int)(skillTable.damage * atkPower) });

        // 태그.
        ecb.AddComponent(entity, new SkillTag());

        // 히트된 애들 저장용.
        ecb.AddComponent(entity, new SkillData_Hit { hitEffect = skillTable.hitEffect, hitDatas = new NativeList<HitDataItem>(Allocator.Persistent)});

        if (skillTable.pushPower > 0)
            ecb.AddComponent(entity, new SkillData_PushPower { power = skillTable.pushPower });
    }

    private void SetHitType(in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData)
    {
        switch (tableData.hitType)
        {
            case eSkillHitType.HitCount:
                /// 별다른 처리 안함.
                break;

            case eSkillHitType.HitWithIntervals:
                ecb.AddComponent(entity, new SkillData_HitWithIntervals { interval = tableData.hitTypeValue });
                break;

            default:
                Debug.LogErrorFormat("처리 안됌 : {0}", tableData.hitType);
                break;
        }
    }

    private void SetDestoryType(in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData, double elTime)
    {
        switch (tableData.destoryType)
        {
            case eSkillDestoryType.Hit:
                ecb.AddComponent(entity, new SkillData_HitCount { count = tableData.destoryValue });
                break;

            case eSkillDestoryType.Time:
                ecb.AddComponent(entity, new SkillData_DestoryTime { time = elTime + tableData.destoryValue });
                return;

            default:
                Debug.LogError("타입 처리 안됌 : " + tableData.destoryType);
                break;
        }
        // 삭제 관련
        ecb.AddComponent(entity, new SkillData_DestoryTime { time = elTime + 10 });
    }

    private void SetGOModel(in EntityCommandBuffer ecb, in Entity entity, float scale, float3 spawnPos, quaternion rotation, string prefabPath)
    {
        GameObject goModel = ResourceManager.Instance.LoadObjectInstantiate(ResourceManager.PathSkill, prefabPath);
        Transform tr = goModel.transform;
        tr.position = spawnPos;
        tr.rotation = rotation;
        tr.localScale = Vector3.one * scale;

        ecb.AddComponent(entity, new SkillData_ModelTransform { trasnform = tr });
    }

    private void SetMoveType(ref SystemState state, in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData, LocalTransform actorTr)
    {
        switch(tableData.moveType)
        {
            // 이동 하지 않는 경우
            case eSkillMoveType.None:
                break;

            case eSkillMoveType.Forward:
                ecb.AddComponent(entity, new SkillData_MoveForward
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
                
                ecb.AddComponent(entity, new SkillData_MoveForward
                {
                    direction = math.normalize(enemyPos - myPos),
                    speed = tableData.moveTypeValue
                });
                break;

            default:
                Debug.LogErrorFormat("tableData.MoveType Error tableID : {0} type : {1}", tableData.id, tableData.moveType);
                break;
        }
    }

    private void SetSpawnTransform(ref SystemState state, eActorType attakerType, in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData, LocalTransform actorTr, float scale, out float3 spawnPos, out quaternion rotation)
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

            default:
                Debug.LogError("moveType 처리안됌 : " + tableData.spawnPositionType);
                return;
        }

        ecb.SetComponent(entity, tr);
    }
}