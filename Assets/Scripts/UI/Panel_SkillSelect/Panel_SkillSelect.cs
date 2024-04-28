using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel_SkillSelect : UISingleton<Panel_SkillSelect>
{
    [SerializeField] private SkillSelectItem[] _items;

    [SerializeField] private SkillIcon[] _activeSkills;
    [SerializeField] private SkillIcon[] _passiveSkills;



    #region button event
    public void OnClickSkill(int idx)
    {
        Debug.LogErrorFormat("onclick Skill : {0}", idx);
        Util.GameReStart();

        Hide();
    }
    #endregion 
}
