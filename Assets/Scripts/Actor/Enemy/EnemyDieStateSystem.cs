using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

//[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public partial struct EnemyDieStateSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach (var (goAnim, actorState, tr, entity) in SystemAPI.Query<ActorData_ModelAnimator, RefRO<ActorData_State>, RefRO<LocalTransform>>().WithAll<EnemyTag>().WithEntityAccess())
        {
            if (actorState.ValueRO.actorState != eActorState.Die)
                continue;

            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            goAnim.animator.SetTrigger("doDeath");

            var buffer = state.EntityManager.GetBuffer<GameData_Enemy>(SystemAPI.GetSingletonEntity<GameDataTag>());
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i].entity == entity)
                {
                    buffer.RemoveAt(i);
                    break;
                }
            }

            ecb.DestroyEntity(entity);

            //// gem 생성.
            //GemSpawnData gemSpawnData = SystemAPI.GetSingleton<GemSpawnData>();
            //GemSpawnItem item = new GemSpawnItem();
            //item.spawnPos = tr.ValueRO.Position;
            //gemSpawnData.datas.Enqueue(item);
            
            //SystemAPI.SetSingleton<GemSpawnData>(gemSpawnData);
        }

        // 모델 삭제 처리. 
        //foreach (var (actorModel, entity) in SystemAPI.Query<ActorData_ModelTransform>().WithNone<LocalToWorld>().WithEntityAccess())
        //{
        //    if (actorModel.trasnform != null)
        //        GameObject.Destroy(actorModel.trasnform.gameObject);
        //    ecb.RemoveComponent<ActorData_ModelTransform>(entity);
        //}
    }
}