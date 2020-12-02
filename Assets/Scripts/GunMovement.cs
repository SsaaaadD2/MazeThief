using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovement : MonoBehaviour
{
    public bool isMoving;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        BobGun();
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

    }

    void BobGun()
    {
        if (isMoving && !animator.GetCurrentAnimatorStateInfo(0).IsName("GunFire"))
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void Fire()
    {
        animator.SetTrigger("Fire");
    }

    public void EnableGun()
    {

    }
}
