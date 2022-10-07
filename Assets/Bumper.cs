using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private Transform visuals;
    public float force;

    private void Update()
    {
        visuals.localScale = Vector2.MoveTowards(visuals.localScale, Vector2.one, Time.deltaTime * 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Assume we are circlar
        var point = collision.contacts[0].point;
        var otherRB = collision.gameObject.GetComponent<Rigidbody>();
        
        if (otherRB != null)
        {
            var impulse = collision.impulse.normalized;
            impulse = new Vector3(impulse.x, 0, impulse.y).normalized;
            otherRB.AddForce(impulse*force,ForceMode.Impulse);
            visuals.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        }
    }
}
