using UnityEngine;
using UnityEngine.UI;

public class MainPlayerUI : MonoBehaviour
{
    [Header("Data Requirements")]
    [SerializeField] private PlayerEntity player = null;

    [Header("UI Requirements")]
    [SerializeField] private Slider abilitySlider = null;

    #region Unity BuiltIn Methods
    private void Start()
    {
        
    }

    private void Update()
    {
        if (player.HasAbility)
            abilitySlider.value = 1f - player.Ability.CooldownInPercent;
    }
    #endregion
}
