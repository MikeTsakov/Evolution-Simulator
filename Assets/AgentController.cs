using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour {

    #region Variables
    private int foodGrass;
    private int foodMeat;
    private int energy;
    int[,] a = new int[6,2];
    #endregion

    void Awake() {
    }

    // Use this for initialization
    void Start () {
		if (energy == 0) {
            if (foodGrass != 0 || foodMeat != 0) {
                energy = (foodGrass * 5) + (foodMeat * 5); 
                foodGrass = 0;
                foodMeat = 0;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
