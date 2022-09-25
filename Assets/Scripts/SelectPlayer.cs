using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayer : MonoBehaviour {
    public Camera mainCamera;
    [HideInInspector] public GameObject selectedObject;

    // Use this for initialization
    void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 selectedPosition = new Vector2(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(selectedPosition.x, selectedPosition.y, 1f), new Vector3(0, 0, -1f), 5.0f);
            if (hit.collider != null) {
                if (hit.collider.gameObject.CompareTag("Carnivore") || hit.collider.gameObject.CompareTag("Herbivore") || hit.collider.gameObject.CompareTag("Egg")) {
                    selectedObject = hit.collider.gameObject;
                }
            } else {
                selectedObject = null;
            }
        }
    }
}
