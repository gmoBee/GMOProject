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
        if (playerReference.HasAbility)
            abilitySlider.value = 1f - playerReference.Ability.CooldownInPercent;
    }
}
