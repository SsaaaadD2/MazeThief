/*
 * written by Joseph Hocking 2017
 * released under MIT license
 * text of license https://opensource.org/licenses/MIT
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

// basic WASD-style movement control
public class FpsMovement : MonoBehaviour
{
    [SerializeField] private Camera headCam;

    public float speed = 6.0f;
    public float gravity = -9.8f;

    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;
    public AudioSource audioSource;

    private float rotationVert = 0;

    //Needs to be synced to gun bob animation time
    private float stepTime = 0.4f;
    private float accumulatedDistance = 0;

    private CharacterController charController;
    private GunMovement gunMovement;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        gunMovement = GetComponentInChildren<GunMovement>();
        gunMovement.gameObject.SetActive(false);
    }

    void Update()
    {
        MoveCharacter();
        RotateCharacter();
        RotateCamera();
    }

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        //Animate the gun to move if we are moving
        if (gunMovement && (deltaX != 0 || deltaZ != 0))
        {
            gunMovement.isMoving = true;
        }
        else
        {
            gunMovement.isMoving = false;
        }

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        charController.Move(movement);

        if (charController.velocity.sqrMagnitude > 0)
        {
            accumulatedDistance += Time.deltaTime;
            if (accumulatedDistance > stepTime)
            {
                audioSource.Play();
                accumulatedDistance = 0;
            }
        }
        else
        {
            accumulatedDistance = 0;
            audioSource.Stop();
        }
    }

    private void RotateCharacter()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
    }

    private void RotateCamera()
    {
        rotationVert -= Input.GetAxis("Mouse Y") * sensitivityVert;
        rotationVert = Mathf.Clamp(rotationVert, minimumVert, maximumVert);

        headCam.transform.localEulerAngles = new Vector3(
            rotationVert, headCam.transform.localEulerAngles.y, 0
        );
    }

    public void EnableGun()
    {
        gunMovement.gameObject.SetActive(true);
    }
}
