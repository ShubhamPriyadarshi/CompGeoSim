using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateBuildings : MonoBehaviour
{

    public GameObject myPrefab;
    public GameObject Axes;
    public GameObject SurfaceCover;
    public GameObject Ground;
    public Material mat;
    

    public static bool startFlag = false;


    private void Update()
    {
        if (startFlag == true)
        {
            Vector2[,] buildingData = ConsoleInputs.BuildingData.buildingData;
            GameObject[] building = new GameObject[ConsoleInputs.BuildingData.numOfBuildings];
            for (int i = 0; i < ConsoleInputs.BuildingData.numOfBuildings; i++)
            {
                building[i] = Instantiate(myPrefab, Axes.transform, true);

                Mesh mesh = new Mesh();

                Vector3[] vertices = new Vector3[4];

                vertices[1] = buildingData[i, 0] - new Vector2(buildingData[i, 0].x, buildingData[i, 0].y);
                vertices[0] = buildingData[i, 1] - new Vector2(buildingData[i, 0].x, buildingData[i, 0].y);
                vertices[3] = buildingData[i, 2] - new Vector2(buildingData[i, 0].x, buildingData[i, 0].y);
                vertices[2] = buildingData[i, 3] - new Vector2(buildingData[i, 0].x, buildingData[i, 0].y);
                mesh.vertices = vertices;

                mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
                building[i].GetComponent<MeshRenderer>().material = mat;
                building[i].GetComponent<MeshFilter>().mesh = mesh;

                building[i].transform.position = vertices[1] + new Vector3(buildingData[i, 0].x, buildingData[i, 0].y, 0);// - new Vector3((ConsoleInputs.BuildingData.buildingData[i, 2].x - ConsoleInputs.BuildingData.buildingData[i, 0].x), (ConsoleInputs.BuildingData.buildingData[i, 1].y - ConsoleInputs.BuildingData.buildingData[i, 0].y));
                Ground.SetActive(true);
                SurfaceCover.SetActive(true);
                


                startFlag = false;

            }
        }
    }
}







