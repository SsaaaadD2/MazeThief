using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CameraSwitch"))
        {
            secondaryCamera.enabled = true;
            mainCamera.enabled = false;
        }

        if (Input.GetButtonUp("CameraSwitch"))
        {
            secondaryCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }
}
