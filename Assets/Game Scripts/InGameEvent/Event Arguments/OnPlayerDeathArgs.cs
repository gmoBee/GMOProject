using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerDeathArgs
{
    private PlayerEntity player;

    public OnPlayerDeathArgs(PlayerEntity player)
    {
        this.player = player;
    }
}
