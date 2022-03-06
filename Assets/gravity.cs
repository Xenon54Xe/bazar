using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity : MonoBehaviour
{
    GameObject earth;
    public float earthMassPow;
    Rigidbody rb;
    public bool gravityB = true;
    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        gravityB = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        gravityB = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Direction(earth.transform.position, gameObject.transform.position);
        if (gravityB)
        {
            earthMassPow = Mathf.Clamp(earthMassPow, 13, 14);
            float dist = RelativeDistance(gameObject.transform.position, earth.transform.position);
            float force = 6.6742f * Mathf.Pow(10, -11) * (rb.mass * 5.972f * Mathf.Pow(10, earthMassPow) / Mathf.Pow(dist, 2));

            AppliquerForces(-dir, force, Color.red);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            AppliquerForces(dir, 50, Color.blue);
        }else if (Input.GetKeyDown(KeyCode.R))
        {
            rb.Sleep();
        }
        if(RelativeDistance(gameObject.transform.position, earth.transform.position) > 1000)
        {
            gameObject.transform.position = Vector3.zero;
        }
    }

    public void AppliquerForces(Vector3 direction, float force, Color color)
    {
        rb.AddForce(direction * force);
        Debug.DrawRay(gameObject.transform.position, direction * force * 2, color);
    }

    public float RelativeDistance(Vector3 posA, Vector3 posB)
    {
        Vector3 dir = posB - posA;
        float step1 = Mathf.Sqrt(Mathf.Pow(dir.x, 2) + Mathf.Pow(dir.y, 2));
        float step2 = Mathf.Sqrt(Mathf.Pow(step1, 2) + Mathf.Pow(dir.z, 2));
        return step2;
    }

    public Vector3 Direction(Vector3 posA, Vector3 posB)
    {
        Vector3 relativePos = posB - posA;
        float hyp = RelativeDistance(Vector3.zero, relativePos);
        return relativePos / hyp;
    }

}
