using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;

    private bool cameraSet;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;

        cameraSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraSet)
        {
            float x = (GlobalVars.hallWidth * GlobalVars.maxCols) / 2;
            float y = secondaryCamera.transform.position.y;
            float z = (GlobalVars.hallHeight * GlobalVars.maxRows) / 2;

            secondaryCamera.gameObject.transform.position = new Vector3(x, y, z);
            cameraSet = true;
        }

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
