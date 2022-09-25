using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController: MonoBehaviour {

    #region Variables
    Transform tF;
    GameObject gM; //GameManager
    GameObject prey;
    GameObject predator;
    [HideInInspector] public int playerID;
    [HideInInspector] public int foodType;
    public GameObject soundPrefab;

    private Randomizer rand;
    public CircleCollider2D vision;

    int consumeNumber;
    private float angle_n;
    private float angle_p;
    private int predatorPrey = 3;
    public bool ignoreAll = false;

    double layEggTimerReset;
    double consumeFoodTimerReset;

    double temp;
    public bool herbivore = false;
    public bool carnivore = false;
    public bool initialPlayers = false;

    [Header("Player Characteristics")]
    public int foodGrass;
    public int foodMeat;
    public double sight;
    public double moveSpeed;
    public double deathTimer;
    public double layEggTimer;
    public double eggSpawnTimer;
    public double consumeFoodTimer;
    public float surviveTimer;

    [Header("Player Characteristics hidden")]
    [HideInInspector] public double moveSpeedStart;
    [HideInInspector] public double deathTimerStart;
    [HideInInspector] public double layEggTimerStart;
    [HideInInspector] public double eggSpawnTimerStart;
    [HideInInspector] public double consumeFoodTimerStart;

    public Vector3 velocity;
    readonly static System.Random rnd = new System.Random();
    #endregion

    private void Awake() {
        rand = new Randomizer();
        tF = GetComponent<Transform>();
        gM = GameObject.Find("GameManager");
        vision = GetComponentInChildren<CircleCollider2D>();
    }

    public void SetupSpawnedPlayers(float moveSpeedF, float deathTimerF, float layEggTimerF, float eggSpawnTimerF, float consumeFoodTimerF, float sightF) {
        tF.rotation = Quaternion.Euler(0, 0, (float)rand.RandomizeDirection(0, 360));
        moveSpeed = moveSpeedStart = moveSpeedF;
        deathTimer = deathTimerStart = deathTimerF;
        layEggTimer = layEggTimerStart = layEggTimerF;
        eggSpawnTimer = eggSpawnTimerStart = eggSpawnTimerF;
        consumeFoodTimer = consumeFoodTimerStart = consumeFoodTimerF;
        sight = sightF;
        surviveTimer = 0;
        vision.radius = (float)sight;
    }

    public void SetupInitialPlayers() {
        tF.rotation = Quaternion.Euler(0, 0, (float)rand.RandomizeDirection(0, 360));
        moveSpeed = moveSpeedStart = rand.RandomizeSpeed(moveSpeed, 0.5f, 0.5f);
        deathTimer = deathTimerStart = rand.RandomizeDeathTimer(deathTimer, 10, 20);
        layEggTimer = layEggTimerStart = rand.RandomizeLayEggTimer(layEggTimer, 10, 10);
        eggSpawnTimer = eggSpawnTimerStart = rand.RandomizeEggSpawnTimer(eggSpawnTimer, 20, 10);
        consumeFoodTimer = consumeFoodTimerStart = rand.RandomizeConsumeFoodTimer(consumeFoodTimer, 20, 10);
        int number = rnd.Next(0, 3);
        if (number == 0) {
            sight = rand.RandomizeSight(sight, 1.0f) + 0.2f;
        } else if (number == 1) {
            sight = rand.RandomizeSight(sight, 1.0f) - 0.2f;
        } else {
            sight = rand.RandomizeSight(sight, 1.0f);
        }
        surviveTimer = 0;
        vision.radius = (float)sight;
    }

    void Start() {
        layEggTimerReset = layEggTimer;
        consumeFoodTimerReset = consumeFoodTimer;
    }

    void FixedUpdate() {
        tF.position += tF.up * (float)moveSpeed * Time.deltaTime / 4;
    }

    void Update() {
        surviveTimer += Time.deltaTime;
        layEggTimer -= Time.deltaTime;
        consumeFoodTimer -= Time.deltaTime;
        deathTimer -= Time.deltaTime;

        if (prey != null) {
            Vector3 vectorToTarget = prey.transform.position - tF.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
        }

        if (predator != null) {
            ignoreAll = true;
        } else {
            ignoreAll = false;
        }

        if (layEggTimer <= 0) {
            if (foodGrass <= 0 && foodMeat <= 0) {
                foodType = 3;
            } else {
                foodType = Random.Range(0, 2);
            }
            layEggTimer = layEggTimerReset;
            switch (foodType) {
                case 0:
                    if (foodGrass <= 0) {
                        break;
                    }
                    foodGrass -= 1;
                    gM.GetComponent<GameManager>().SpawnEgg(tF.position.x, tF.position.y, foodType, this);
                    break;
                case 1:
                    if (foodMeat <= 0) {
                        break;
                    }
                    foodMeat -= 1;
                    gM.GetComponent<GameManager>().SpawnEgg(tF.position.x, tF.position.y, foodType, this);
                    break;
                case 3:
                    break;
            }
        }

        if (consumeFoodTimer <= 0) {
            if (foodGrass <= 0 && foodMeat <= 0) {
                consumeNumber = 3;
            } else {
                consumeNumber = Random.Range(0, 2);
            }
            consumeFoodTimer = consumeFoodTimerReset;
            switch (consumeNumber) {
                case 0:
                    if (foodGrass <= 0) {
                        break;
                    }
                    foodGrass -= 1;
                    deathTimer += deathTimerStart / 3.5f;
                    break;
                case 1:
                    if (foodMeat <= 0) {
                        break;
                    }
                    foodMeat -= 1;
                    deathTimer += deathTimerStart / 3;
                    break;
                case 3:
                    break;
            }
        }

        if (deathTimer <= 0) {
            if (foodGrass >= 5) {
                deathTimer += deathTimerStart / 2f;
                foodGrass -= 5;
            } else if (foodMeat >= 5) {
                deathTimer += deathTimerStart / 2f;
                foodMeat -= 5;
            } else {
                Destroy(gameObject);
            }
        }

        if (transform.parent.name == "Herbivores") {
            herbivore = true;
            transform.tag = "Herbivore";
        } else if (transform.parent.name == "Carnivores") {
            carnivore = true;
            transform.tag = "Carnivore";
        }

        if (foodGrass >= 10) {
            foodGrass = 0;
            int nSwitch = rnd.Next(0, 2);
            switch (nSwitch) {
                case 0:
                    IncreaseSight();
                    break;
                case 1:
                    IncreaseMoveSpeed();
                    break;
            }
        }

        if (foodMeat >= 10) {
            foodMeat = 0;
            int nSwitch = rnd.Next(0, 2);
            switch (nSwitch) {
                case 0:
                    IncreaseSight();
                    break;
                case 1:
                    IncreaseMoveSpeed();
                    break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Grass") {
            if (herbivore) {
                foodGrass += 1;
                Destroy(collision.gameObject);
                tF.rotation = Quaternion.Euler(0, 0, (float)rand.RandomizeDirection(0, 360));
            } else if (carnivore) {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            }
        } else if (collision.gameObject.tag == "Meat") {
            if (herbivore) {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            } else if (carnivore) {
                foodMeat += 1;
                Destroy(collision.gameObject);
                tF.rotation = Quaternion.Euler(0, 0, (float)rand.RandomizeDirection(0, 360));
            }
        } else if (collision.gameObject.tag == "Egg") {
            if (herbivore) {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            } else if (carnivore) {
                foodMeat += 1;
                Destroy(collision.gameObject);
                tF.rotation = Quaternion.Euler(0, 0, (float)rand.RandomizeDirection(0, 360));
            }
        } else if (collision.gameObject.tag == "Herbivore") {
            if (herbivore) {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            } else if (carnivore) {
                Destroy(collision.gameObject);
                foodMeat += 2;
            }
        } else if (collision.gameObject.tag == "Carnivore") {
            Destroy(gameObject);
        } else if (collision.gameObject.tag == "Pulsate" || collision.gameObject.tag == "Spin") {
            Destroy(gameObject);
        } else {
            ContactPoint2D contact = collision.contacts[0];
            float angle = Vector2.SignedAngle(new Vector2(1f, 0f), contact.normal);

            if (angle < 0) {
                angle = 360 + angle;
            }

            float degreeOffset = 70;
            angle_p = angle + degreeOffset;
            angle_n = angle - degreeOffset;


            tF.rotation = Quaternion.Euler(0, 0, rand.RandomizeAngle(angle_p, angle_n) - 90);

            //This is used to draw the angle at which the agent can move after it hit a wall.
            Vector2 d1 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle_p), Mathf.Sin(Mathf.Deg2Rad * angle_p));
            Vector2 d2 = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle_n), Mathf.Sin(Mathf.Deg2Rad * angle_n));

            Debug.DrawRay(contact.point, contact.normal, Color.green, 20, false);
            Debug.DrawRay(contact.point, d1, Color.blue, 20, false);
            Debug.DrawRay(contact.point, d2, Color.red, 20, false);

            //The 90 is used to change the angle from mathematical representation to Unity representation
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (carnivore) {
            int NSwitchOut = rnd.Next(0, 101);
            if (collision.tag == "Herbivore") {
                prey = collision.gameObject;
            }
            if (collision.tag == "Carnivore") {
                if (NSwitchOut > 15) {
                    Vector3 vectorToTarget = collision.gameObject.transform.position - tF.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
                }
            }
            if (collision.tag == "Meat") {
                Vector3 vectorToTarget = collision.gameObject.transform.position - tF.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
            } else if (collision.tag == "Grass" && NSwitchOut > 75) {
                Vector3 vectorToTarget = collision.gameObject.transform.position - tF.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
                if (collision.tag == "Herbivore") {
                    prey = collision.gameObject;
                }
            }

            if (collision.tag == "Pulsate" || collision.tag == "Spin" || collision.tag == "Carniovre") {
                int nSwitchIn = rnd.Next(0, 9);
                if (nSwitchIn <= 7) {
                    Vector3 vectorToTarget = collision.gameObject.transform.position - tF.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
                }
            }
        }

        if (herbivore) {
            if (collision.tag == "Carnivore") {
                predator = collision.gameObject;
                Instantiate(soundPrefab, tF.position, transform.rotation);
                Vector3 vectorToTarget = collision.gameObject.transform.position - tF.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
            }

            if (collision.tag == "Grass" && ignoreAll == false) {
                Vector3 vectorToTarget = collision.gameObject.transform.position - tF.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
            }

            if (collision.tag == "Pulsate" || collision.tag == "Spin") {
                int nSwitchIn = rnd.Next(0, 9);
                if (nSwitchIn <= 7) {
                    Vector3 vectorToTarget = collision.gameObject.transform.position - tF.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        predatorPrey--;
        if (predatorPrey == 0) {
            prey = null;
            predator = null;
            predatorPrey = 3;
        }
    }

    public void Sound(Vector3 position) {
        Vector3 vectorToTarget = position - tF.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        tF.rotation = Quaternion.Slerp(tF.rotation, q, (float)moveSpeed);
    }

    private void IncreaseSight() {
        sight = sight * 1.1f;
    }

    private void IncreaseMoveSpeed() {
        moveSpeed = moveSpeedStart = moveSpeed * 1.1f;
    }
}
