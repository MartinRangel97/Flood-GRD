using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Camera cam;
    private Vector3 startLocation;

    private void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
 {
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 10;
            if (cam.orthographicSize <= 5) {
                cam.orthographicSize = 5;
            }
        }

        if (Input.GetMouseButtonDown(2)) {
            startLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2)) {
            cam.transform.position -= Camera.main.ScreenToWorldPoint(Input.mousePosition) - startLocation;
            startLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

}
