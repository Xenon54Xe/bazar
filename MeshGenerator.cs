using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    Vector3[] usedVertices;

    int[] triangles;
    int[] usedTriangles;

    [Range(3, 1000)]
    public int points = 3;
    int lastPoint;

    [Range(0, 1)]
    public float rate = 1;
    float lastRate;

    public float distanceWithCenter = 1;
    private float lastDistanceWithCenter;

    public bool showVertices = false;
    public float verticesPointSize = 0.03f;

    public float surface;
    public float perimeter;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void Update()
    {
        if (lastPoint != points || lastDistanceWithCenter != distanceWithCenter)
        {
            CreateShape();
            UpdateMesh();
        }
        else if (lastRate != rate)
        {
            UpdateMesh();
        }
    }

    void CreateShape()
    {
        vertices = new Vector3[points + 1];
        vertices[0] = Vector3.zero;

        float angleBetweePoint = 2 * Mathf.PI / points; 
        for (int i = 0; i < points; i++)
        {
            float adjacent = Mathf.Cos(i * angleBetweePoint) * -1 * distanceWithCenter;
            float opposite = Mathf.Sin(i * angleBetweePoint) * distanceWithCenter;

            vertices[i + 1] = new Vector3(adjacent, 0, opposite);
        }

        triangles = new int[3 * points];

        for (int i = 0; i < points; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        triangles[triangles.Length - 1] = 1;
        
        lastPoint = points;
    }

    void UpdateMesh()
    {
        mesh.Clear();

        int nbOfTriangles = Mathf.RoundToInt(triangles.Length * rate / 3);

        usedTriangles = new int[nbOfTriangles * 3];
        for (int i = 0; i < nbOfTriangles * 3; i++)
        {
            usedTriangles[i] = triangles[i];
        }

        if (nbOfTriangles == 0)
        {
            usedTriangles = new int[0];
            usedVertices = new Vector3[0];
        }

        UpdateUsedVertices();
        CalculateData(nbOfTriangles);

        mesh.vertices = usedVertices;
        mesh.triangles = usedTriangles;

        mesh.RecalculateNormals();

        lastDistanceWithCenter = distanceWithCenter;
        lastRate = rate;
    }

    void UpdateUsedVertices()
    {
        List<int> usedIndexes = new List<int>();

        foreach (int index in usedTriangles)
        {
            bool addIndex = true;
            foreach (int usedIndex in usedIndexes)
            {
                if (index == usedIndex)
                {
                    addIndex = false;
                    break;
                }
            }

            if (addIndex)
            {
                usedIndexes.Add(index);
            }
        }

        usedVertices = new Vector3[usedIndexes.Count];
        for (int i = 0; i < usedIndexes.Count; i++)
        {
            usedVertices[i] = vertices[usedIndexes[i]];
        }
    }

    void CalculateData(int nbOfTriangles)
    {
        float angleBetweenPoints = 2 * Mathf.PI / points;

        float adjacent = Mathf.Cos(angleBetweenPoints / 2) * distanceWithCenter;
        float opposite = Mathf.Sin(angleBetweenPoints / 2) * distanceWithCenter * 2;

        float surfaceOfATriangle = adjacent * opposite / 2;

        surface = surfaceOfATriangle * nbOfTriangles;

        perimeter = opposite * nbOfTriangles;
    }

    private void OnDrawGizmos()
    {
        if (showVertices && usedVertices != null)
        {
            Gizmos.color = Color.black;

            foreach (Vector3 vertices in usedVertices)
            {
                Gizmos.DrawSphere(vertices, verticesPointSize);
            }
        }
    }
}
