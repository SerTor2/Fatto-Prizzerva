using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPlayer : MonoBehaviour
{
    public static ChessPlayer instance;
    public int movements;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);

    }
}
