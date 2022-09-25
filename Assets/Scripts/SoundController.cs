using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    float time = 5;
    CircleCollider2D cC;

    // Use this for initialization
    void Start() {
        Destroy(gameObject, time);
        cC = GetComponent<CircleCollider2D>();
        cC.radius = 5;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (cC.radius >= 20) {
            cC.radius = 20;
        } else {
            cC.radius = cC.radius * 1.005f;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Herbivore") {
            collision.GetComponent<PlayerController>().Sound(transform.position);
            collision.GetComponent<PlayerController>().ignoreAll = true;
        }
    }
}
