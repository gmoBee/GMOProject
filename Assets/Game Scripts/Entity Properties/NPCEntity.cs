﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEntity : LivingEntity
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandleAbilityUsage()
    {
        throw new System.NotImplementedException();
    }
}