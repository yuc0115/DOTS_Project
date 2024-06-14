using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class Panel_SkillSelect : UISingleton<Panel_SkillSelect>
{
    [SerializeField] private SkillSelectItem[] _items;

    [SerializeField] private SkillIcon[] _activeSkills;
    [SerializeField] private SkillIcon[] _passiveSkills;

    private World _world;

    private void Awake()
    {
        _world = World.DefaultGameObjectInjectionWorld;
    }

    public override void Show()
    {
        base.Show();

        // ½ºÅ³ »Ì±â.
        List<uint> ls = new List<uint>();
        foreach(var skillItem in Table_Skill.instance.data)
        {
            ls.Add(skillItem.Key);
        }

        List<uint> lsSkills = new List<uint>();
        int randomIdx = 0;
        for (int i = 0; i < 3; i++)
        {
            randomIdx = Random.Range(0, ls.Count);
            lsSkills.Add(ls[randomIdx]);
            ls.RemoveAt(randomIdx);
        }

        for (int i = 0; i < lsSkills.Count; i++)
        {
            _items[i].Show(lsSkills[i], 1);
        }
        for (int i = lsSkills.Count; i < 3; i++)
        {
            _items[i].Hide();
        }
    }

    #region button event
    public void OnClickSkill(int idx)
    {
        EntityQuery query = _world.EntityManager.CreateEntityQuery(new ComponentType[] { typeof(PlayerTag), typeof(SkillData_Trigger) });
        SkillData_Trigger skillDataTrigger;
        if (query.TryGetSingleton(out skillDataTrigger))
        {
            Table_SkillData data = Table_Skill.instance.GetData(_items[idx].GetSkillID());

            SkillData_TriggerItem item = new SkillData_TriggerItem();
            item.id = data.id;
            item.spawnDelay = data.spawnTypeValue;
            item.isAnimation = !string.IsNullOrEmpty(data.animTriggerName);
            skillDataTrigger.datas.Add(item);

            Entity e = query.GetSingletonEntity();
            _world.EntityManager.SetComponentData<SkillData_Trigger>(e, skillDataTrigger);
        }
        
        Util.GameReStart();

        Hide();
    }
    #endregion 
}
