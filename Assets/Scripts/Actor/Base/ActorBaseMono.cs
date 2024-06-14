using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ActorBaseMono : MonoBehaviour
{
    private Entity _entity;
    private World _world;
    private string _poolTag;
    public eActorType _actorType;

    /// <summary>
    /// 스폰 되야할 스킬 아이디.
    /// </summary>
    private uint _curAnimClipSkillID;
    private int _atkPower;

    private void Awake()
    {
        _world = World.DefaultGameObjectInjectionWorld;
    }
    
    public void SetEntity(Entity entity, eActorType actorType, string poolTag)
    {
        _entity = entity;
        _actorType = actorType;
        _poolTag = poolTag;
    }

    public void AddSkillSpawnID(uint skillID, int atkPower)
    {
        _curAnimClipSkillID = skillID;
        _atkPower = atkPower;
    }

    #region animator event
    private void OnAnimFire()
    {
        EntityQuery query = _world.EntityManager.CreateEntityQuery(new ComponentType[] { typeof(SkillData_Spawn) });
        
        SkillData_Spawn rw;
        if (query.TryGetSingleton(out rw))
        {
            var tr = _world.EntityManager.GetComponentData<LocalTransform>(_entity);
            SkillData_SpawnItem spawnItem = new SkillData_SpawnItem();
            spawnItem.attackerType = _actorType;
            spawnItem.id = _curAnimClipSkillID;
            spawnItem.tr = tr;
            spawnItem.atkPower = _atkPower;
            rw.datas.Enqueue(spawnItem);
        }
    } 

    private void OnAnimDeath()
    {
        switch(_actorType)
        {
            case eActorType.Enemy:
                var model = _world.EntityManager.GetComponentData<ActorData_ModelTransform>(_entity);
                PoolManager.Instance.ReleasePooledObject(_poolTag, gameObject);
                //GameObject.Destroy(model.trasnform.gameObject);
                _world.EntityManager.RemoveComponent<ActorData_ModelTransform>(_entity);
                break;

            default:
                Debug.LogErrorFormat("OnAnimDeath() 처리 안됌 : {0}", _actorType);
                break;
        }
    }
    #endregion
}
