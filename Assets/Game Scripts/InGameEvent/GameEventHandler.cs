using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    public static GameEventHandler instance;

    // Delegates
    public delegate void EntityWeaponRoll(OnWeaponRollArgs args);

    // Events
    public event EntityWeaponRoll OnEntityWeaponRollEvent;

    #region Unity BuiltIn Methods
    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public void CallWeaponRollEvent(OnWeaponRollArgs arg)
    {
        OnEntityWeaponRollEvent?.Invoke(arg);
    }
}
