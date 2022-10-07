using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var k = other.GetComponent<Killable>();
        if (k != null)
        {
            k.Kill();
        }
    }
}
