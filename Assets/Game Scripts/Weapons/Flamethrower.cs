using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Flamethrower : Weapon
{
    // TODO: This barrel attribute part is being reconsidered to be remove
    [Header("Flamethrower Barrel Attributes")]
    [SerializeField] private FuelBarrel fuelBarrel = null;
    [SerializeField] private float fuelUsePerSeconds = 1f;
    [SerializeField] private float fuelRecoverPerSec = 2f;

    [Space, Header("Flamethrower Physical Attributes")]
    [Slider(1f, 10f)] [SerializeField] private float maxBurnRange = 1f;
    [Slider(0.1f, 10f)] [SerializeField] private float intensity = 0.5f;
    [SerializeField] private float wideRadius = 1f;
    [SerializeField] private VisualEffect flameVisualEffect = null;

    private IEnumerator m_burnRoutine;
    private IEnumerator m_refuelRoutine;

    // Debuggers
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isFlameActive;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private bool m_isRefueling;
    [BoxGroup("DEBUG")] [SerializeField] [ReadOnly] private float m_overheatTimeleft;

    #region Unity Built-In Methods
    protected override void OnEnable()
    {
        InputDataForWeapon.ResetInput();
        if (FlashLight.enabled)
            FlashLight.enabled = false;
        flameVisualEffect.Stop();
    }

    private void Start()
    {
        fuelBarrel.BarrelReset();
        GenericBarrel = fuelBarrel;
    }

    protected override void Update()
    {
        if (InputDataForWeapon.ShootClicked)
            Burn();
        if (InputDataForWeapon.ShootReleased)
            Refuel();
    }

    protected override void OnDisable()
    {
        if (m_burnRoutine != null)
        {
            StopCoroutine(m_burnRoutine);
            m_isFlameActive = false;
            flameVisualEffect.Stop();
        }

        if (m_refuelRoutine != null)
        {
            StopCoroutine(m_refuelRoutine);
            m_isRefueling = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (InputDataForWeapon != null)
        {
            Vector3 barrelPos = fuelBarrel.BarrelTransform.position;
            Vector3 flameDir = (InputDataForWeapon.MidTargetPosition - barrelPos).normalized;
            barrelPos += flameDir * wideRadius;

            Gizmos.DrawWireSphere(barrelPos, wideRadius);
        }
    }
    #endregion

    private void Burn()
    {
        if (m_isRefueling)
            m_isRefueling = false;

        if (fuelBarrel.CurrentFuel <= 0f)
            return;

        if (m_refuelRoutine != null)
            StopCoroutine(m_refuelRoutine);

        m_burnRoutine = BurningRoutine();
        StartCoroutine(m_burnRoutine);
    }

    private void Refuel()
    {
        if (m_isFlameActive)
            m_isFlameActive = false;

        if (m_isRefueling || fuelBarrel.CurrentFuel >= fuelBarrel.MaxFuel)
            return;

        if (m_burnRoutine != null)
            StopCoroutine(m_burnRoutine);

        m_refuelRoutine = RefuelRoutine();
        StartCoroutine(m_refuelRoutine);
    }

    private IEnumerator BurningRoutine()
    {
        m_isFlameActive = true;
        float fireRateHolder = 0f;

        // Play VFX and lighting
        FlashLight.enabled = true;
        flameVisualEffect.Play();

        while (fuelBarrel.CurrentFuel > 0f)
        {
            // Drink fuel
            fuelBarrel.FuelUp(-fuelUsePerSeconds);

            fireRateHolder -= Time.deltaTime;
            if (fireRateHolder <= 0)
            {
                // Cast flame
                Vector3 barrelOrigin = fuelBarrel.BarrelTransform.position;
                Vector3 flameDir = (InputDataForWeapon.MidTargetPosition - barrelOrigin).normalized;
                barrelOrigin += flameDir * wideRadius; 

                RaycastHit[] hits = Physics.SphereCastAll(barrelOrigin, wideRadius, flameDir, maxBurnRange);
                Debug.DrawRay(barrelOrigin, flameDir * maxBurnRange);
                foreach (RaycastHit hit in hits)
                {
                    // if it detects self fire then ignore hit
                    if (hit.transform.Equals(transform.root))
                        continue;

                    if (Targets.Contains(hit.collider.tag))
                    {
                        LivingEntity m_entityGotHit = hit.collider.GetComponent<LivingEntity>();
                        if (m_entityGotHit != null && m_entityGotHit.RelationID != OnHandOwner.RelationID)
                        {
                            m_entityGotHit.Hit(DamageRate);
                            Debug.Log($"{m_entityGotHit.name} got burned and taken damage: {DamageRate}");
                        }
                    }
                }
                fireRateHolder = FireRate;
            }

            yield return null;
        }

        m_isFlameActive = false;
        Refuel();
    }

    private IEnumerator RefuelRoutine()
    {
        m_isRefueling = true;

        // Stop VFX and lighting
        flameVisualEffect.Stop();
        FlashLight.enabled = false;

        // Keep the cooldown running when flamethrower is running out
        if (fuelBarrel.CurrentFuel > 0f)
            m_overheatTimeleft = fuelBarrel.OverheatCooldown;

        // Wait for few moment, fuel barrel is cooling down
        while (m_overheatTimeleft > 0f)
        {
            m_overheatTimeleft -= Time.deltaTime;
            yield return null;
        }
        // Adding fuel back to its maximum
        while (fuelBarrel.CurrentFuel < fuelBarrel.MaxFuel)
        {
            fuelBarrel.FuelUp(fuelRecoverPerSec);
            yield return null;
        }

        m_isRefueling = false;
    }

}
