using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Captor
{
    public Transform captor;
    public Sides side;

    public float GetDistance(Vector3 pos)
    {
        float dist = Vector3.Distance(captor.position, pos);
        return dist;
    }

    public RaycastHit GetNearestHit(float range)
    {
        RaycastHit[] hits = GetHits(range);

        float nearestDist = Mathf.Infinity;
        RaycastHit nearestHit = new RaycastHit();
        foreach(RaycastHit hit in hits)
        {
            float dist = Vector3.Distance(captor.position, hit.point);
            if(dist < nearestDist)
            {
                nearestDist = dist;
                nearestHit = hit;
            }
        }
        return nearestHit;
    }

    public RaycastHit[] GetHits(float range)
    {
        RaycastHit[] hits = Physics.RaycastAll(captor.position, captor.TransformDirection(Vector3.forward), range);
        return hits;
    }

    public Vector3 GetDirection(Vector3 vector)
    {
        return captor.TransformDirection(vector);
    }

    public Transform GetTransform()
    {
        return captor;
    }

    public Sides GetSide()
    {
        return side;
    }
}
public enum Sides
{
    Forward,
    MidRight,
    MidLeft,
    Right,
    Left
}