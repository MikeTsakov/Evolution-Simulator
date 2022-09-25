using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    GameObject gM;

    private void Awake() {
        gM = GameObject.Find("GameManager");
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Pulsate" || collision.gameObject.tag == "Spin") {
            Destroy(gameObject);
            gM.GetComponent<GameManager>().SpawnFood(true);
        }
    }
}
