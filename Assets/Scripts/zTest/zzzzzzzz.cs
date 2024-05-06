using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class zzzzzzzz : MonoBehaviour
{
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Time.timeScale = 1;
        }
    }
}

