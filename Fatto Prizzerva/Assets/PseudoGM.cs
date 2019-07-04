using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoGM : MonoBehaviour
{
    PseudoGM _instance;

    public TestPlayer player;
    public TestPlayer Player { get; protected set; }


    private void Awake()
    {
        #region(Singleton Pattern)
        DontDestroyOnLoad(this);
        // Si _instancia tiene una referencia que no somos nosotros nos destruimos 
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        #endregion
    }


}
