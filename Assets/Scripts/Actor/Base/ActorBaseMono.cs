using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ActorBaseMono : MonoBehaviour
{
    private Entity _entity;
    private World _world;

    public void Awake()
    {
        _world = World.DefaultGameObjectInjectionWorld;
    }
    public void SetEntity(Entity entity)
    {
        _entity = entity;
    }

    #region animator event
    private void OnAnimFire()
    {
        var data = _world.EntityManager.GetComponentData<SkillTrigger>(_entity);
        data.isFire = true;
        _world.EntityManager.SetComponentData<SkillTrigger>(_entity, data);
    }
    #endregion
}
