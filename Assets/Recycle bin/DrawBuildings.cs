using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBuildings : MonoBehaviour
{
    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;
    public Material mat;

    void Start()
    {   

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];

        vertices[0] = new Vector3(4+1.5f, 0+2f);
        vertices[1] = new Vector3(4+1.5f, 4+2f);
        vertices[2] = new Vector3(7+1.5f, 4+2f);
        vertices[3] = new Vector3(7+1.5f, 0+2f);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        GetComponent<MeshRenderer>().material = mat;
        GetComponent<MeshFilter>().mesh = mesh;

        this.transform.position = vertices[0] - new Vector3(3f, 4);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
