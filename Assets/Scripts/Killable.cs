using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{
    public GameObject deathParticlesPrefab;
    public void Kill()
    {
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        var part = Instantiate(deathParticlesPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            yield return null;
            t += Time.deltaTime;
        }
        
        Destroy(part);
        Destroy(gameObject);
    }
}
