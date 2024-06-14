using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel_LevelInfo : MonoBehaviour
{
    [SerializeField] Slider _sliderExp = null;
    [SerializeField] TMP_Text _level;

    private int _showLevel = 1;
    private float _showCurExp = 0;
    private float _showMaxExp = 0;

    public float speed = 10;

    private void Start()
    {
        var playerData = GameManager.Instance.playerData;
        _showLevel = playerData.level;
        _showCurExp = playerData.curExp;
        _showMaxExp = playerData.maxExp;
    }

    private void LateUpdate()
    {
        var playerData = GameManager.Instance.playerData;
        if (_showLevel < playerData.level)
        {
            _showCurExp = Mathf.Lerp(_showCurExp, _showMaxExp, Time.deltaTime * speed);
            // level up.
            if (_showMaxExp - _showCurExp < _showMaxExp * 0.01f)
            { 
                _showLevel++;
                _showMaxExp = Util.GetMaxExp(_showLevel);
                _showCurExp = 0;

                LevelUp();
            }
        }
        else
        {
            _showCurExp = Mathf.Lerp(_showCurExp, playerData.curExp, Time.deltaTime * speed);
        }

        _sliderExp.value = _showCurExp / (float)_showMaxExp;

        _level.text = _showLevel.ToString();

        if (Input.GetKeyDown(KeyCode.X))
        {
            LevelUp();
        }
    }

    

    private void LevelUp()
    {
        Panel_SkillSelect.Instance.Show();
        Util.GamePause();
    }
}
