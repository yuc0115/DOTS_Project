using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SkillSelectItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _icon;
    [SerializeField] private Image[] _levelStar;
    [SerializeField] private SpriteAtlas _atlas;

    private uint _skillID = 0;

    public uint GetSkillID() { return _skillID; }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(uint skillId, int skillLevel)
    {
        _skillID = skillId;
        Table_SkillData data = Table_Skill.instance.GetData(skillId);

        _title.text = data.name;
        _description.text = data.description;

        for (int i = 0; i < _levelStar.Length; i++)
        {
            _levelStar[i].color = skillLevel > i ? Color.yellow : Color.black;
        }

        _icon.sprite = _atlas.GetSprite(data.iconName);
    }
}
