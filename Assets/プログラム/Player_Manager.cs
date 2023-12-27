using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mamager : MonoBehaviour
{
    public static Player_Mamager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
