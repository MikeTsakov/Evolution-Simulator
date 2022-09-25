using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager: MonoBehaviour {

    #region Variables
    //[HideInInspector] public GameObject playerParentObject;
    [HideInInspector] public GameObject eggParentObject;
    [HideInInspector] public GameObject herbivoreParentObject;
    [HideInInspector] public GameObject carnivoreParentObject;
    [HideInInspector] public int playerID = 0;
    [HideInInspector] public int eggID = 0;
    public GameObject[] foodPrefabs;
    public GameObject[] playerPrefabs;
    public GameObject[] eggPrefabs;
    GameObject foodObject;
    GameObject newPlayer;
    GameObject newEgg;
    GameObject gS;
    float targetOrtho;
    float cameraSpeed = 1;
    bool destroy;
    int number = 0;

    private SelectPlayer selectScript;
    private Button btnClicked;
    private GameObject cN;
    private Button button;

    [Header("Camera Settings")]
    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;

    [Header("Player Settings")]
    public float playerInitialSpawn;
    GameObject playerPrefab;
    GameObject foodPrefab;
    GameObject eggPrefab;

    [Header("Food Settings")]
    public float foodInitialSpawn;
    public float spawnAfter;
    public float spawnAfterReset;
    public float foodDeathTimer;

    [Header("X Spawn Range")]
    public float xMin;
    public float xMax;

    [Header("Y Spawn Range")]
    public float yMin;
    public float yMax;

    [Header("Text Objects Egg")]
    EggController selectedEgg;

    [Header("Text Objects Player")]
    PlayerController selectedPlayer;
    private Text grassText;
    private Text meatText;
    private Text speedText;
    private Text sightText;
    private Text deathText;
    private Text layEggText;
    private Text eggSpawnText;
    private Text consumeFoodText;
    private Text surviveTimerText;
    private Text playerNumberText;

    [Header("Test")]
    public GameObject[] pulsate;
    public GameObject[] spin;
    Vector3 originalScale;
    Vector3 destinationScale;
    float scaleTimer;
    #endregion

    void Awake() {
        gS = GameObject.Find("GameSpeed").gameObject;
        cN = GameObject.Find("Canvas").gameObject;
        button = cN.GetComponentInChildren<Button>();
        //playerParentObject = new GameObject { name = "Players" };
        eggParentObject = new GameObject { name = "Eggs" };
        herbivoreParentObject = new GameObject { name = "Herbivores" };
        carnivoreParentObject = new GameObject { name = "Carnivores" };

        selectScript = GetComponent<SelectPlayer>();
        grassText = GameObject.Find("Grass").GetComponent<Text>();
        meatText = GameObject.Find("Meat").GetComponent<Text>();
        speedText = GameObject.Find("Speed").GetComponent<Text>();
        sightText = GameObject.Find("Canvas").transform.Find("Sight").GetComponent<Text>();
        deathText = GameObject.Find("Death").GetComponent<Text>();
        layEggText = GameObject.Find("Lay Egg").GetComponent<Text>();
        eggSpawnText = GameObject.Find("Egg Spawn").GetComponent<Text>();
        consumeFoodText = GameObject.Find("Consume Food").GetComponent<Text>();
        surviveTimerText = GameObject.Find("SurviveTime").GetComponent<Text>();
        playerNumberText = GameObject.Find("Player Number").GetComponent<Text>();
        
        pulsate = GameObject.FindGameObjectsWithTag("Pulsate");
        spin = GameObject.FindGameObjectsWithTag("Spin");
        scaleTimer = Time.deltaTime;
    }

    void Start() {
        ResetCamera = Camera.main.transform.position;
        targetOrtho = Camera.main.orthographicSize;
        for (int i = 0;i < foodInitialSpawn;i++) { //This spawns the initial food, which does not have a death timer
            SpawnFood(false);
        }
        for (int i = 0;i < playerInitialSpawn;i++) { //This spawns the initial players
            SpawnFirstPlayers();
        }
    }

    void Update() {
        spawnAfter -= Time.deltaTime;
        if (spawnAfter <= 0) { //This spawns the food with the death timer
            SpawnFood(true);
            spawnAfter = spawnAfterReset;
        }

        if (selectScript.selectedObject != null) {
            if (selectScript.selectedObject.tag == "Herbivore" || selectScript.selectedObject.tag == "Carnivore") { //These are the labels for the Player
                selectedPlayer = selectScript.selectedObject.GetComponent<PlayerController>();
                grassText.text = "Grass: " + selectedPlayer.foodGrass;
                meatText.text = "Meat: " + selectedPlayer.foodMeat;
                speedText.text = "Speed: " + selectedPlayer.moveSpeed.ToString("F2");
                deathText.text = "Death: " + selectedPlayer.deathTimer.ToString("F2");
                layEggText.text = "Lay Egg: " + selectedPlayer.layEggTimer.ToString("F2");
                eggSpawnText.text = "Egg Spawn: " + selectedPlayer.eggSpawnTimer.ToString("F2");
                consumeFoodText.text = "Consume Food: " + selectedPlayer.consumeFoodTimer.ToString("F2");
                sightText.text = "Sight: " + selectedPlayer.sight.ToString("F2");
                surviveTimerText.text = "Survive: " + selectedPlayer.surviveTimer;
                playerNumberText.text = selectedPlayer.name;
            } else if (selectScript.selectedObject.tag == "Egg") {
                selectedEgg = selectScript.selectedObject.GetComponent<EggController>();
                ResetLabels();
                speedText.text = "Speed: " + selectedEgg.moveSpeed.ToString("F2");
                deathText.text = "Death: " + selectedEgg.deathTimer.ToString("F2");
                layEggText.text = "Lay Egg: " + selectedEgg.layEggTimer.ToString("F2");
                eggSpawnText.text = "Egg Spawn: " + selectedEgg.spawnTimer.ToString("F2");
                consumeFoodText.text = "Consume Food: " + selectedEgg.consumeFoodTimer.ToString("F2");
                sightText.text = "Sight: " + selectedPlayer.sight.ToString("F2");
                playerNumberText.text = selectedEgg.name;
            }
        } else {
            ResetLabels();
        }

        if (Input.GetKey(KeyCode.K)) {
            cN.SetActive(false);
            //Make button be highlighted when canvas is enabled back
        }

        if (Input.GetKey(KeyCode.L)) {
            cN.SetActive(true);
            //Make button be highlighted when canvas is enabled back
            button.Select();
        }

        #region obstaclePulsate
        if (gS.GetComponent<GameSpeed>().paused == false) {
            for (int i = 0;i < pulsate.Length;i++) {
                if (pulsate[i].transform.localScale.x <= 0.5) { //This sets the size of the obstacle
                    destinationScale = new Vector3(1.1f, 1.1f, 1.1f);
                } else if (pulsate[i].transform.localScale.x >= 1) {
                    destinationScale = new Vector3(0.45f, 0.45f, 0.45f);
                }

                originalScale = pulsate[i].transform.localScale;
                pulsate[i].transform.localScale = Vector3.Lerp(originalScale, destinationScale, scaleTimer * (float)1.1); //This makes some of the obstacles pulsate
            }
        }
        #endregion

        #region obstacleRotate
        if (gS.GetComponent<GameSpeed>().paused == false) {
            for (int i = 0;i < spin.Length;i++) {
                spin[i].transform.Rotate(Vector3.back * scaleTimer * 10);
            }
        }
        #endregion
    }

    void LateUpdate() {
        ZoomMap();
        CameraMove();
        MouseDragCamera();
    }

    private void ResetLabels() { //This resets the labels
        grassText.text = "Grass: N/A";
        meatText.text = "Meat: N/A";
        speedText.text = "Speed: N/A";
        deathText.text = "Death: N/A";
        layEggText.text = "Lay Egg: N/A";
        eggSpawnText.text = "Egg Spawn: N/A";
        consumeFoodText.text = "Consume Food: N/A";
        sightText.text = "Sight: N/A";
        surviveTimerText.text = "SurviveTime: N/A";
        playerNumberText.text = "Player: N/A";
    }

    public void SpawnFood(bool destroy) { //This is the function that spawns the food (with or without the death timer)
        Vector2 position = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        foodPrefab = foodPrefabs[Random.Range(0, foodPrefabs.Length)];
        foodObject = Instantiate(foodPrefab, position, transform.rotation);

        if (destroy == true) {
            Destroy(foodObject, foodDeathTimer);
        }
    }

    public void SpawnFirstPlayers() { //This function is used to spawn the initial players set by the user
        Vector2 position = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        //playerPrefab = playerPrefabs[Random.Range(0, playerPrefabs.Length)];
        if (number < 5) {
            newPlayer = Instantiate(playerPrefabs[0], position, transform.rotation);
            newPlayer.GetComponent<PlayerController>().initialPlayers = true;
            newPlayer.GetComponent<PlayerController>().SetupInitialPlayers();
            SetNewParent(newPlayer, herbivoreParentObject);
            number += 1;
        } else if (number >= 5) {
            newPlayer = Instantiate(playerPrefabs[1], position, transform.rotation);
            newPlayer.GetComponent<PlayerController>().SetupInitialPlayers();
            SetNewParent(newPlayer, carnivoreParentObject);
            number += 1;
        }
        newPlayer.name = "Player " + playerID;
        playerID += 1;
    }

    public void SpawnEgg(float x, float y, int foodType, PlayerController player) {
        Vector2 position = new Vector2(x, y);
        eggPrefab = eggPrefabs[Random.Range(0, eggPrefabs.Length)];
        newEgg = Instantiate(eggPrefab, position, transform.rotation);
        newEgg.GetComponent<EggController>().Setup(player);
        SetNewParent(newEgg, eggParentObject);
        newEgg.name = "Egg " + eggID;
        eggID += 1;
    }

    public void SpawnPlayerFromEgg(string playerType, float x, float y, float moveSpeed, float deathTimer, float layEggTimer, float eggSpawnTimer, float consumeFoodTimer, float sight) {
        Vector2 position = new Vector2(x, y);
        //playerPrefab = playerPrefabs[Random.Range(0, playerPrefabs.Length)];
        if (playerType == "Herbivore") {
            newPlayer = Instantiate(playerPrefabs[0], position, transform.rotation);
            newPlayer.GetComponent<PlayerController>().SetupSpawnedPlayers(moveSpeed, deathTimer, layEggTimer, eggSpawnTimer, consumeFoodTimer, sight);
            SetNewParent(newPlayer, herbivoreParentObject);
        } else if (playerType == "Carnivore") {
            newPlayer = Instantiate(playerPrefabs[1], position, transform.rotation);
            newPlayer.GetComponent<PlayerController>().SetupSpawnedPlayers(moveSpeed, deathTimer, layEggTimer, eggSpawnTimer, consumeFoodTimer, sight);
            SetNewParent(newPlayer, carnivoreParentObject);
        }
        newPlayer.name = "Player " + playerID;
        playerID += 1;
    }

    void SetNewParent(GameObject currentPlayer, GameObject newParent) { //This function moves the player or egg to the respective parent
        currentPlayer.transform.parent = newParent.transform;
    }

    void ZoomMap() { //This function is used to zoom
        float zoomSpeed = 5.0f;
        float smoothSpeed = 3.0f;
        float minOrtho = 1.0f;
        float maxOrtho = 5.0f;
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f) {
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }

    void CameraMove() { //This function is to move the cameras position with the arrow keys
        if (Input.GetKey(KeyCode.RightArrow)) {
            Camera.main.transform.position += Vector3.right * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Camera.main.transform.position += Vector3.left * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            Camera.main.transform.position += Vector3.up * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            Camera.main.transform.position += Vector3.down * cameraSpeed * Time.deltaTime;
        }
    }

    void MouseDragCamera() { //This function uses the mouse to move the camera
        if (Input.GetMouseButton(0)) {
            Diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (Drag == false) {
                Drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        } else {
            Drag = false;
        }
        if (Drag == true) {
            Camera.main.transform.position = Origin - Diference;
        }
        if (Input.GetMouseButton(1)) {
            targetOrtho = 5;
            Camera.main.transform.position = ResetCamera;
            Camera.main.orthographicSize = 5;
        }
    }
}