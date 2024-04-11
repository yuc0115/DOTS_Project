using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class TestMono : MonoBehaviour
{
    public class Baker : Baker<TestMono>
    {
        public override void Bake(TestMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TestEvent
            {
                isTrigger = false
            });
        }
    }

}

public partial struct TestEvent : IComponentData
{
    public bool isTrigger;
}