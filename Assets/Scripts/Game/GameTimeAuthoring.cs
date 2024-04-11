using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;

public class GameTimeAuthoring : MonoBehaviour
{
    public TMP_Text _time;
    public class Baker : Baker<GameTimeAuthoring>
    {
        public override void Bake(GameTimeAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new GameTime
            {
                time = 0
            });
        }
    }
}



public struct GameTime : IComponentData
{
    public float time;
}
