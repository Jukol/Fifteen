using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    RaycastHit hitInfo;

    private void Update()
    {
        Debug.DrawRay(transform.position, -Vector3.forward * 10);

    }
}
