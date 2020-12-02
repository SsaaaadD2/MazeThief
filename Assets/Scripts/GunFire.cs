using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    public Transform projectileSpawnPoint;
    public GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && gameObject)
        {
            Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }
}
