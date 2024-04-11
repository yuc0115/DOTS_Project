using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ActorBaseMono : MonoBehaviour
{
    private Entity _entity;
    private World _world;
    private eActorType _actorType;

    public void Awake()
    {
        _world = World.DefaultGameObjectInjectionWorld;
    }
    public void SetEntity(Entity entity, eActorType actorType)
    {
        _entity = entity;
        _actorType = actorType;
    }

    #region animator event
    private void OnAnimFire()
    {
        var data = _world.EntityManager.GetComponentData<SkillData_Trigger>(_entity);
        data.isFire = true;
        _world.EntityManager.SetComponentData<SkillData_Trigger>(_entity, data);
    }

    private void OnAnimDeath()
    {
        switch(_actorType)
        {
            case eActorType.Enemy:
                var model = _world.EntityManager.GetComponentData<ActorData_ModelTransform>(_entity);
                GameObject.Destroy(model.trasnform.gameObject);

                _world.EntityManager.RemoveComponent<ActorData_ModelTransform>(_entity);
                break;

            default:
                Debug.LogErrorFormat("OnAnimDeath() Ã³¸® ¾È‰Î : {0}", _actorType);
                break;
        }
    }
    #endregion
}
