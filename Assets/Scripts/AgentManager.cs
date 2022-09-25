using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {

    #region Variables
    [HideInInspector] public GameObject agentParentObject;
    [HideInInspector] public int agentID = 0;
    [HideInInspector] public int agentPop = 0;
    public GameObject agentPrefab;
    GameObject newAgent;
    public int playerCount;
    #endregion

    private void Awake() {
        agentParentObject = new GameObject { name = "Agents" };
    }

    // Use this for initialization
    void Start () {
        for (int i=0; i < playerCount; i++) {
            SpawnAgents();
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (agentPop <= 0) {
            SpawnAgents();
        }
	}

    void SpawnAgents() {
        newAgent = Instantiate(agentPrefab, new Vector3(0,0,0), transform.rotation);
        //spawn agent with characteristics
        SetNewParent(newAgent, agentParentObject);
        newAgent.name = "Agent " + agentID;
        agentID += 1;
    }

    void SetNewParent(GameObject currentPlayer, GameObject newParent) { //This function moves the player or egg to the respective parent
        currentPlayer.transform.parent = newParent.transform;
    }
}
