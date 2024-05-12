using Unity.Entities;
using UnityEngine;

public class SkillPrefabAuthoring : MonoBehaviour
{
    [SerializeField] private float radius = 1;
    [SerializeField] private float prefabScale = 1;

    public class Baker : Baker<SkillPrefabAuthoring>
    {
        public override void Bake(SkillPrefabAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SkillData_PrefabSize
            {
                radius = authoring.radius,
                scale = authoring.prefabScale
            });
        }
    }
}

public struct SkillData_PrefabSize : IComponentData
{
    public float radius;
    public float scale;
}