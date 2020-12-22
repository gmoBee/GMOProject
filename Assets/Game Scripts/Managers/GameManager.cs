using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }
}
