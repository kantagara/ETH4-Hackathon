using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance => instance;
    protected virtual bool ShouldntBeDestroyedOnLoad => true;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
            if(ShouldntBeDestroyedOnLoad)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
