using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InstantiatePolygon : MonoBehaviour
{
    public Material mat;
    public GameObject myPrefab;
    public GameObject Axes;
    public Text OutputText;
    public static Text outputTextStatic;
    public GameObject mainCamera;
    public GameObject Point;
    public static bool startFlag = false;
    public static bool inPolygon = false;



    private void Start()
    {
        outputTextStatic = OutputText;
    }
    bool PointInPolygon()
    {
        int numOfVertices = ConsoleInputs.PolygonData.numOfVertices;
        Vector2[] polygonData = ConsoleInputs.PolygonData.polygonData;
        Vector2 pointCoordinates = ConsoleInputs.PolygonData.pointCoordinates;

        int i, j = numOfVertices - 1;
        bool oddNodes = false;

        for (i = 0; i < numOfVertices; i++)
        {
            if (polygonData[i].y < pointCoordinates.y && polygonData[j].y >= pointCoordinates.y
            || polygonData[j].y < pointCoordinates.y && polygonData[i].y >= pointCoordinates.y)
            {
                if (polygonData[i].x + (pointCoordinates.y - polygonData[i].y) / (polygonData[j].y - polygonData[i].y) * (polygonData[j].x - polygonData[i].x) < pointCoordinates.x)
                {
                    oddNodes = !oddNodes;
                }
            }
            j = i;
        }

        return oddNodes;
    }

    void SetPointOnClick()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        ////GameObject gameObject = GetComponent<GameObject>
        //SceneView sceneView = SceneView.currentDrawingSceneView;
        //Camera camera = sceneView.camera;
        Camera cameraloc = mainCamera.GetComponent<Camera>();
        //mousePos.y = (cameraloc.pixelHeight - mousePos.y);
        //mousePos.x = (cameraloc.pixelWidth - mousePos.x);
        mousePos = cameraloc.ScreenToWorldPoint(mousePos);

      //  mousePos.y = -mousePos.y;
       //mousePos.x = -mousePos.x;
        Debug.Log(mousePos);
        ConsoleInputs.PolygonData.pointCoordinates = mousePos;
        Point.transform.position = mousePos;
    }

    private void Update()
    {
        if (startFlag == true )
        {
            GameObject polygon;
            int numOfVertices = ConsoleInputs.PolygonData.numOfVertices;
            Vector2[] polygonData = ConsoleInputs.PolygonData.polygonData;
            polygon = Instantiate(myPrefab, Axes.transform, true);
            Mesh mesh = new Mesh();
            Vector2[] vertices2D = new Vector2[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                vertices2D[i] = polygonData[i]- new Vector2(polygonData[0].x, polygonData[0].y);
            }
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            // Create the mesh
         
            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            polygon.GetComponent<MeshRenderer>().material = mat;
            polygon.GetComponent<MeshFilter>().mesh = mesh;
            polygon.transform.position = vertices2D[0] + new Vector2(polygonData[0].x, polygonData[0].y);
            inPolygon = PointInPolygon();
            outputTextStatic.text = (inPolygon == true ? "Output: Point lies inside the polygon" : "Output: Point lies outside the polygon");
            startFlag = false;

        }
        if (Input.GetMouseButtonDown(0))
        {
            SetPointOnClick();
            

            GameObject polygon;
            int numOfVertices = ConsoleInputs.PolygonData.numOfVertices;
            Vector2[] polygonData = ConsoleInputs.PolygonData.polygonData;
            polygon = Instantiate(myPrefab, Axes.transform, true);
            Mesh mesh = new Mesh();
            Vector2[] vertices2D = new Vector2[numOfVertices];

            for (int i = 0; i < numOfVertices; i++)
            {
                vertices2D[i] = polygonData[i] - new Vector2(polygonData[0].x, polygonData[0].y);
            }
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            // Create the mesh

            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            polygon.GetComponent<MeshRenderer>().material = mat;
            polygon.GetComponent<MeshFilter>().mesh = mesh;
            polygon.transform.position = vertices2D[0] + new Vector2(polygonData[0].x, polygonData[0].y);
            inPolygon = PointInPolygon();
            outputTextStatic.text = (inPolygon == true ? "Output: Point lies inside the polygon" : "Output: Point lies outside the polygon");
            startFlag = false;

        }
    }
}