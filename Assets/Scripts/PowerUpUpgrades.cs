using SubiNoOnibus.Networking.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUpUpgrades : MonoBehaviour
{
    public BoosterPreset boosterPreset0;
    public BoosterPreset boosterPreset1;
    public BoosterPreset boosterPreset2;
    public BoosterPreset boosterPreset3;

    public BusStopPreset busStopPreset0;
    public BusStopPreset busStopPreset1;
    public BusStopPreset busStopPreset2;
    public BusStopPreset busStopPreset3;

    public LightningPreset lightningPreset0;
    public LightningPreset lightningPreset1;
    public LightningPreset lightningPreset2;
    public LightningPreset lightningPreset3;

    public NitroPreset nitroPreset0;
    public NitroPreset nitroPreset1;
    public NitroPreset nitroPreset2;
    public NitroPreset nitroPreset3;

    public PlayerHealthPreset playerHealth0Preset0;
    public PlayerHealthPreset playerHealth0Preset1;
    public PlayerHealthPreset playerHealth0Preset2;
    public PlayerHealthPreset playerHealth0Preset3;

    public ShieldPreset shieldPreset0;
    public ShieldPreset shieldPreset1;
    public ShieldPreset shieldPreset2;
    public ShieldPreset shieldPreset3;

    public GameObject boosterPrefab;
    public GameObject busStopPrefab0;
    public GameObject busStopPrefab1;
    public GameObject lightningPrefab;
    public GameObject nitroPrefab;
    public GameObject playerPrefab;
    public GameObject shieldPrefab;

    private void Awake()
    {
        StartCoroutine(UserStatusRequestHandler.GetUserStatus(OnSuccess, OnFailure));
    }

    private void OnSuccess(UserStatus status)
    {
        foreach (PowerUpUpgrade upgrade in status.upgrades)
        {
            Debug.Log(upgrade.itemName + ": " + upgrade.level);
            switch (upgrade.itemName)
            {
                case "Max_Life":
                    switch (upgrade.level)
                    {
                        case 0:
                            playerPrefab.GetComponent<HealthSystem>().SetHealthMax(playerHealth0Preset0.starterHealth);
                            break;
                        case 1:
                            playerPrefab.GetComponent<HealthSystem>().SetHealthMax(playerHealth0Preset1.starterHealth);
                            break;
                        case 2:
                            playerPrefab.GetComponent<HealthSystem>().SetHealthMax(playerHealth0Preset2.starterHealth);
                            break;
                        case 3:
                            playerPrefab.GetComponent<HealthSystem>().SetHealthMax(playerHealth0Preset3.starterHealth);
                            break;
                        default:
                            playerPrefab.GetComponent<HealthSystem>().SetHealthMax(playerHealth0Preset0.starterHealth);
                            break;
                    }
                    break;
                case "Base_Acceleration":
                    // Do nothing
                    break;
                case "Booster":
                    Booster booster = boosterPrefab.GetComponent<Booster>();
                    switch (upgrade.level)
                    {
                        case 0:
                            booster.boost = boosterPreset0.boost;
                            booster.duration = boosterPreset0.duration;
                            break;
                        case 1:
                            booster.boost = boosterPreset1.boost;
                            booster.duration = boosterPreset1.duration;
                            break;
                        case 2:
                            booster.boost = boosterPreset2.boost;
                            booster.duration = boosterPreset2.duration;
                            break;
                        case 3:
                            booster.boost = boosterPreset3.boost;
                            booster.duration = boosterPreset3.duration;
                            break;
                        default:
                            booster.boost = boosterPreset0.boost;
                            booster.duration = boosterPreset0.duration;
                            break;
                    }
                    break;
                case "Nitro":
                    Nitro nitro = nitroPrefab.GetComponent<Nitro>();
                    switch (upgrade.level)
                    {
                        case 0:
                            nitro.boost = nitroPreset0.boost;
                            nitro.duration = nitroPreset0.duration;
                            break;
                        case 1:
                            nitro.boost = nitroPreset1.boost;
                            nitro.duration = nitroPreset1.duration;
                            break;
                        case 2:
                            nitro.boost = nitroPreset2.boost;
                            nitro.duration = nitroPreset2.duration;
                            break;
                        case 3:
                            nitro.boost = nitroPreset3.boost;
                            nitro.duration = nitroPreset3.duration;
                            break;
                        default:
                            nitro.boost = nitroPreset0.boost;
                            nitro.duration = nitroPreset0.duration;
                            break;
                    }
                    break;
                case "Bus_Stop":
                    BusStop busStop0 = busStopPrefab0.GetComponentInChildren<BusStop>();
                    BusStop busStop1 = busStopPrefab1.GetComponentInChildren<BusStop>();
                    switch (upgrade.level)
                    {
                        case 0:
                            busStop0.healAmount = busStopPreset0.healAmount;
                            busStop1.healAmount = busStopPreset0.healAmount;
                            break;
                        case 1:
                            busStop0.healAmount = busStopPreset1.healAmount;
                            busStop1.healAmount = busStopPreset1.healAmount;
                            break;
                        case 2:
                            busStop0.healAmount = busStopPreset2.healAmount;
                            busStop1.healAmount = busStopPreset2.healAmount;
                            break;
                        case 3:
                            busStop0.healAmount = busStopPreset3.healAmount;
                            busStop1.healAmount = busStopPreset3.healAmount;
                            break;
                        default:
                            busStop0.healAmount = busStopPreset0.healAmount;
                            busStop1.healAmount = busStopPreset0.healAmount;
                            break;
                    }
                    break;
                case "Shield":
                    Shield shield = shieldPrefab.GetComponent<Shield>();
                    switch (upgrade.level)
                    {
                        case 0:
                            shield.protectionTimes = shieldPreset0.maxHits;
                            shield.duration = shieldPreset0.maxDuration;
                            break;
                        case 1:
                            shield.protectionTimes = shieldPreset1.maxHits;
                            shield.duration = shieldPreset1.maxDuration;
                            break;
                        case 2:
                            shield.protectionTimes = shieldPreset2.maxHits;
                            shield.duration = shieldPreset2.maxDuration;
                            break;
                        case 3:
                            shield.protectionTimes = shieldPreset3.maxHits;
                            shield.duration = shieldPreset3.maxDuration;
                            break;
                        default:
                            shield.protectionTimes = shieldPreset0.maxHits;
                            shield.duration = shieldPreset0.maxDuration;
                            break;
                    }
                    break;
                case "Laser":
                    Lightning lightning = lightningPrefab.GetComponent<Lightning>();
                    switch (upgrade.level)
                    {
                        case 0:
                            lightning.size = lightningPreset0.size;
                            lightning.maxTargets = lightningPreset0.maxTargets;
                            break;
                        case 1:
                            lightning.size = lightningPreset1.size;
                            lightning.maxTargets = lightningPreset1.maxTargets;
                            break;
                        case 2:
                            lightning.size = lightningPreset2.size;
                            lightning.maxTargets = lightningPreset2.maxTargets;
                            break;
                        case 3:
                            lightning.size = lightningPreset3.size;
                            lightning.maxTargets = lightningPreset3.maxTargets;
                            break;
                        default:
                            lightning.size = lightningPreset0.size;
                            lightning.maxTargets = lightningPreset0.maxTargets;
                            break;
                    }
                    break;
            }
        }
    }

    private void OnFailure(UnityWebRequest request)
    {
        playerPrefab.GetComponent<HealthSystem>().SetHealthMax(playerHealth0Preset0.starterHealth);
        
        Booster booster = boosterPrefab.GetComponent<Booster>();
        booster.boost = boosterPreset0.boost;
        booster.duration = boosterPreset0.duration;
        
        Nitro nitro = nitroPrefab.GetComponent<Nitro>();
        nitro.boost = nitroPreset0.boost;
        nitro.duration = nitroPreset0.duration;
        
        BusStop busStop0 = busStopPrefab0.GetComponentInChildren<BusStop>();
        BusStop busStop1 = busStopPrefab1.GetComponentInChildren<BusStop>();
        busStop0.healAmount = busStopPreset0.healAmount;
        busStop1.healAmount = busStopPreset0.healAmount;
        
        Shield shield = shieldPrefab.GetComponent<Shield>();
        shield.protectionTimes = shieldPreset0.maxHits;
        shield.duration = shieldPreset0.maxDuration;
        
        Lightning lightning = lightningPrefab.GetComponent<Lightning>();
        lightning.size = lightningPreset0.size;
        lightning.maxTargets = lightningPreset0.maxTargets;
    }
    
}
