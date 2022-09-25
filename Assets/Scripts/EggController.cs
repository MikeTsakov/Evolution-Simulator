using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggController : MonoBehaviour {

    #region Variables
    Transform tF;
    GameObject gM;
    Randomizer rand;
    
    [HideInInspector] public int foodType;

    [Header("Egg Spawn Timer")]
    public double spawnTimer;

    [Header("Egg - New Player Characteristics")]
    private string playerType;

    public double moveSpeed;
    public double deathTimer;
    public double layEggTimer;
    public double eggSpawnTimer;
    public double consumeFoodTimer;
    public double sight;
    #endregion

    private void Awake() {
        tF = GetComponent<Transform>();
        gM = GameObject.Find("GameManager");
        rand = new Randomizer();
    }

    public void Setup(PlayerController pC) {
        spawnTimer = pC.eggSpawnTimer;
        foodType = pC.foodType;

        if (pC.herbivore == true) {
            playerType = "Herbivore";
        } else if (pC.carnivore == true) {
            playerType = "Carnivore";
        }

        if (foodType == 0) {
            moveSpeed = rand.RandomizeSpeed(pC.moveSpeedStart, 0.1f, 0.35f);
            deathTimer = rand.RandomizeDeathTimer(pC.deathTimerStart, 10f, 20f);
            layEggTimer = rand.RandomizeLayEggTimer(pC.layEggTimerStart, 10f, 10f);
            eggSpawnTimer = rand.RandomizeEggSpawnTimer(pC.eggSpawnTimerStart, 20f, 10f);
            consumeFoodTimer = rand.RandomizeConsumeFoodTimer(pC.consumeFoodTimerStart, 20f, 10f);
            sight = rand.RandomizeSight(pC.sight, 1.05F);
        } else if (foodType == 1) {
            moveSpeed = rand.RandomizeSpeed(pC.moveSpeedStart, 0.15f, 0.3f);
            deathTimer = rand.RandomizeDeathTimer(pC.deathTimerStart, 10f, 30f);
            layEggTimer = rand.RandomizeLayEggTimer(pC.layEggTimerStart, 15f, 10f);
            eggSpawnTimer = rand.RandomizeEggSpawnTimer(pC.eggSpawnTimerStart, 25f, 10f);
            consumeFoodTimer = rand.RandomizeConsumeFoodTimer(pC.consumeFoodTimerStart, 25f, 10f);
            sight = rand.RandomizeSight(pC.sight, 1.2F);
        }
    }

	void Update () {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0) {
            gM.GetComponent<GameManager>().SpawnPlayerFromEgg(playerType, tF.position.x, tF.position.y, 
                (float)moveSpeed, (float)deathTimer, (float)layEggTimer, (float)eggSpawnTimer, (float)consumeFoodTimer, (float) sight);
            Destroy(gameObject);
        }
	}
}
