using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (T)FindObjectOfType(typeof(T));
                if (m_Instance == null)
                {
                    var singletonObject = new GameObject();
                    m_Instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + "?(Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }
            return m_Instance;
        }
    }
}