using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class ResDataAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs = null;

    public class Baker : Baker<ResDataAuthoring>
    {
        public override void Bake(ResDataAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            DynamicBuffer<ResData> buffer = AddBuffer<ResData>(entity);            

            foreach (var prefab in authoring.prefabs)
            {
                buffer.Add(new ResData { prefab = GetEntity(prefab, TransformUsageFlags.WorldSpace) });
            }
        }
    }
}

public struct ResData : IBufferElementData
{
    public Entity prefab;
}
