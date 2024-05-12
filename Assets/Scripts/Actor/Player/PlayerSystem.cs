using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerSystem : ISystem
{
    private float3 _normal;

    void OnUpdate(ref SystemState state) 
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isDodge = Input.GetKeyDown(KeyCode.Space);

        _normal.x = horizontal;
        _normal.z = vertical;
        _normal = Vector3.Normalize(_normal);

        foreach (var (stat, tr, pv) in SystemAPI.Query<RefRO<ActorData_MoveStat>, RefRW<LocalTransform>, RefRW<PhysicsVelocity>>().WithAll<PlayerTag>().WithAll<ControllEnable>())
        {
            pv.ValueRW.Linear = _normal * stat.ValueRO.moveSpeed;
            tr.ValueRW.Rotation = Quaternion.Lerp(tr.ValueRO.Rotation, Quaternion.LookRotation(GetLookVector(tr.ValueRO.Position)), deltaTime * stat.ValueRO.rotSpeed);
        }

        foreach (var (goAnim, goTr, localTr) in SystemAPI.Query<ActorData_ModelAnimator, ActorData_ModelTransform, RefRO<LocalTransform>>().WithAll<PlayerTag>())
        {
            goTr.trasnform.position = localTr.ValueRO.Position;
            goTr.trasnform.rotation = localTr.ValueRO.Rotation;

            goAnim.animator.SetFloat("MoveSpeed", Vector3.Magnitude(_normal));
        }

        // dodge Ã³¸®.
        if (isDodge)
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            bool isHas = state.EntityManager.HasComponent<Dodge>(playerEntity);

            if (isHas && state.EntityManager.IsComponentEnabled<ControllEnable>(playerEntity))
            {
                var dodgeComponent = state.EntityManager.GetComponentData<Dodge>(playerEntity);
                if (Vector3.Magnitude(_normal) != 0)
                {
                    dodgeComponent.normal = _normal;
                }
                else
                {
                    float3 pos = state.EntityManager.GetComponentData<LocalTransform>(playerEntity).Position;
                    dodgeComponent.normal = GetLookVector(pos);
                }
                
                dodgeComponent.isTrigger = true;
                dodgeComponent.endTime = SystemAPI.Time.ElapsedTime + 0.5;
                state.EntityManager.SetComponentData<Dodge>(playerEntity, dodgeComponent);
                state.EntityManager.SetComponentEnabled<Dodge>(playerEntity, true);
            }
        }
    }

    private float3 GetLookVector(float3 playerPos)
    {
        Camera mainCam = Camera.main;

        float3 pos = playerPos;
        float3 vMouse = float3.zero;
        UnityEngine.Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        UnityEngine.RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            vMouse = hit.point;
        }

        float3 v = Vector3.Normalize(vMouse - pos);
        v.y = 0;
        return v;
    }
}
