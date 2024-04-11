using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Entities;

public class Panel_GameTime : UISingleton<Panel_GameTime>
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _time;

    public void SetTime(float time)
    {
        int m = (int)(time / 60);
        int s = (int)(time % 60);
        _time.text = string.Format("{0:00}:{1:00}", m, s);
    }
}
