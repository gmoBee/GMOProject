using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerUI : MonoBehaviour
{
    [Header("Data Requirements")]
    [SerializeField] private PlayerEntity playerReference = null;

    [Header("UI Requirements")]
    [SerializeField] private Slider abilitySlider = null;

    private void Start()
    {
        
    }

    private void Update()
    {
        AbstractAbility ability = playerReference.GetAbility();
        if (ability != null)
            abilitySlider.value = 1f - ability.CooldownInPercent;
    }
}
