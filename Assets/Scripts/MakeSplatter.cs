using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSplatter : MonoBehaviour
{
    private GameObject splatterDecal;
    public bool splatter = false;
    // Start is called before the first frame update
    void Start()
    {
        splatterDecal = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (splatter)
        {
            splatterDecal.SetActive(true);
        }
    }
}
