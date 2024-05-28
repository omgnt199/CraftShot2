using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance => _instance;

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnAwake()
    {

    }
}