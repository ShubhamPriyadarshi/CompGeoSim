using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
//using System.Numerics;
using UnityEngine;
using UnityEngine.UI;




public class ConsoleInputs : MonoBehaviour
{
    public static class PolygonData
    {
        public static int numOfVertices;
        public static string[] polygonDataRaw;
        public static Vector2 pointCoordinates;
        public static Vector2[] polygonData;
    }

    public static class BuildingData
    {
        public static int numOfBuildings;
        public static string[] buildingDataRaw;
        public static Vector2 sunCoordinates;
        public static Vector2[,] buildingData;
    }

    public static int option = 0;
    public Text currText;
    public GameObject SunlightBuilding;
    public GameObject PointInPolygon;
    public GameObject Sun;
    public GameObject Point;

    public GameObject defaultOptions1;
    public GameObject defaultOptions2;
    public static GameObject SunlightBuildingStatic;
    public static GameObject PointInPolygonStatic;
    public static GameObject SunStatic;
    public static GameObject PointStatic;

    private void Start()
    {
        SunlightBuildingStatic = SunlightBuilding;
        PointInPolygonStatic = PointInPolygon;
        SunStatic = Sun;
        PointStatic = Point;
        SunlightBuilding.SetActive(false);
        PointInPolygon.SetActive(false);
    }
    public void SetOption(int opt)
    {
        option = opt;
        currText.text = option > 1 ? "Enter the values of [ N x 4 x 2 ] array consisting the coordinates of N buildings in 2D space, where N is the number of buildings" : "Enter the values of [N x 2] array consisting the coordinates of N vertices of a polygon in 2D space";

        if (option == 1)
            defaultOptions1.SetActive(true);
        else
            defaultOptions2.SetActive(true);
    }

    public static void DrawBuilding()
    {
       
     
        SunStatic.transform.position = BuildingData.sunCoordinates;
        ConsoleInputs.BuildingData.buildingData = new Vector2[BuildingData.numOfBuildings, 4];
        ConsoleInputs.BuildingCoordinates();
    }
    public static void SetSun(Vector2 pos)
    {
        SunStatic.transform.position = pos;
    }

    public static void DrawPolygon()
    {


        PointStatic.transform.position = PolygonData.pointCoordinates;
        ConsoleInputs.PolygonData.polygonData = new Vector2[PolygonData.numOfVertices];
        ConsoleInputs.PolygonCoordinates();
    }

    public static void BuildingCoordinates()
    {
        int k = 0;
        for (int i = 0; i < BuildingData.numOfBuildings; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                string[] temp = BuildingData.buildingDataRaw[k].Split(',');
                k++;
                float tempX = Convert.ToSingle(temp[0]);
                float tempY = Convert.ToSingle(temp[1]);
                BuildingData.buildingData[i, j].Set(tempX, tempY);
            }
        }
        SortBuildingData();
        SunlightBuildingStatic.SetActive(true);
        InstantiateBuildings.startFlag = true;
    }
    public static void SortBuildingData()
    {
        int n = BuildingData.numOfBuildings, i, j, flag;
        float val;
        Vector2[] valVec = new Vector2[4];
        for (i = 1; i < n; i++)
        {
            val = BuildingData.buildingData[i, 0].x;

            for (int k = 0; k < 4; k++)
            {
                valVec[k] = BuildingData.buildingData[i, k];
            }
            flag = 0;
            for (j = i - 1; j >= 0 && flag != 1;)
            {
                if (val < BuildingData.buildingData[j, 0].x)
                {
                    for (int k = 0; k < 4; k++)
                        BuildingData.buildingData[j + 1, k] = BuildingData.buildingData[j, k];
                    j--;
                    for (int k = 0; k < 4; k++)
                        BuildingData.buildingData[j + 1, k] = valVec[k];
                }
                else flag = 1;
            }
        }
    }

    public static void PolygonCoordinates()
    {
        for (int i = 0; i < PolygonData.numOfVertices; i++)
        {
            string[] temp = PolygonData.polygonDataRaw[i].Split(',');
            float tempX = Convert.ToSingle(temp[0]);
            float tempY = Convert.ToSingle(temp[1]);
            PolygonData.polygonData[i].Set(tempX, tempY);
        }
        PointInPolygonStatic.SetActive(true);
        InstantiatePolygon.startFlag = true;
    }

}
