using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class SurfaceCover : MonoBehaviour
{
    int numOfBuildings;
    Vector2 sunCoordinates;
    Vector2[,] buildingData;
    bool onTop;
    int bisectionPoint;
    bool sunRightmost;
    bool leftPart;
    bool rightPart;
    public Text OutputText;
    public GameObject myPrefab;
    public GameObject mainCamera;
    public GameObject LinesPrefab;
    public GameObject LinesPrefab2;
    int linesCount = 0;
    Vector2[,] linesArray = new Vector2[100, 2];

    public static float surfaceValue1 = 0f;

    private void OnEnable()
    {
        float surfaceValue = 0f;
        numOfBuildings = ConsoleInputs.BuildingData.numOfBuildings;
        sunCoordinates = ConsoleInputs.BuildingData.sunCoordinates;
        buildingData = ConsoleInputs.BuildingData.buildingData;
        bisectionPoint = Bisection();
        leftPart = bisectionPoint == 0 ? false : true;
        rightPart = bisectionPoint == numOfBuildings - 1 ? false : true;
        surfaceValue = CalculateSurface();
        OutputText.text = "Length of surface exposed to sunlight: " + Convert.ToString(surfaceValue);

    }
    void SetSunOnClick()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -10f;
        Camera cameraloc = mainCamera.GetComponent<Camera>();
        mousePos = cameraloc.ScreenToWorldPoint(mousePos);

        mousePos.y = -mousePos.y;
        mousePos.x = -mousePos.x;
        Debug.Log(mousePos);
        ConsoleInputs.BuildingData.sunCoordinates = mousePos;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            float surfaceValue = 0f;
            SetSunOnClick();
            sunCoordinates = ConsoleInputs.BuildingData.sunCoordinates;
            numOfBuildings = ConsoleInputs.BuildingData.numOfBuildings;
            buildingData = ConsoleInputs.BuildingData.buildingData;

            ConsoleInputs.SetSun(sunCoordinates);
            bisectionPoint = Bisection();
            print("bisectionPoint" + bisectionPoint);
            leftPart = bisectionPoint == 0 ? false : true;
            rightPart = bisectionPoint == numOfBuildings - 1 ? false : true;

            GameObject[] OldLines1 = GameObject.FindGameObjectsWithTag("LinesOnGround");
            for (int i = 0; i < OldLines1.Length; i++)
                Destroy(OldLines1[i]);
            GameObject[] OldLines = GameObject.FindGameObjectsWithTag("Lines");
            for (int i = 0; i < OldLines.Length; i++)
                Destroy(OldLines[i]);

            surfaceValue = CalculateSurface();

            OutputText.text = "Length of surface exposed to sunlight " + Convert.ToString(surfaceValue);
        }
    }

    int Bisection()
    {
        for (int i = 0; i < numOfBuildings; i++)
        {
            if (sunCoordinates.x < buildingData[i, 0].x)
            {
                sunRightmost = false;
                onTop = false;
                print("OnTOP" + onTop);
                return i;
            }
            else if (sunCoordinates.x >= buildingData[i, 0].x && sunCoordinates.x <= buildingData[i, 3].x)
            {
                sunRightmost = false;
                onTop = true;
                print("OnTOP" + onTop);
                return i;

            }
        }
        onTop = false;
        sunRightmost = true;
        print("sunRightmost" + sunRightmost);
        return numOfBuildings-1;
    }

    float FindSlope(float x1, float x2, float y1, float y2)
    {
        return (y2 - y1) / (x2 - x1);
    }

    //float FindDistance(float x1, float x2, float y1, float y2)
    //{
    //    return Mathf.Sqrt(Mathf.Pow((y2 - y1), 2f) + (Mathf.Pow((x2 - x1), 2f)));
    //}
    float FindDistance(Vector2 V1, Vector2 V2)
    {
        return Mathf.Sqrt(Mathf.Pow((V2.y - V1.y), 2f) + (Mathf.Pow((V2.x - V1.x), 2f)));
    }

    float FindYIntercept(float x, float y, float m)
    {
        return (y - (m * x));
    }

    float FindX(float y, float m, float c)
    {
        return ((y - c) / m);
    }
    float FindY(float x, float m, float c)
    {
        return ((m * x) + c);
    }

    void DrawLine(Vector2 start, Vector2 end)
    {

        GameObject Line = Instantiate(LinesPrefab);
        LineRenderer LineRend = Line.GetComponent<LineRenderer>();
        LineRend.positionCount = 3;
        LineRend.widthMultiplier = 0.05f;
        LineRend.SetPosition(0, start);
        LineRend.SetPosition(1, sunCoordinates);
        LineRend.SetPosition(2, end);
        linesArray[linesCount, 0] = start;
        linesArray[linesCount, 1] = end;
        linesCount++;

    }

    void DrawLineOnBuilding()
    {
        float distanceAlt = 0f;
        for (int i = 0; i < linesCount; i++)
        {
            GameObject Line1 = Instantiate(LinesPrefab2);
            LineRenderer LineRend1 = Line1.GetComponent<LineRenderer>();
            LineRend1.positionCount = 2;
            LineRend1.widthMultiplier = 0.7f;
            LineRend1.SetPosition(0, linesArray[i, 0]);
            LineRend1.SetPosition(1, linesArray[i, 1]);
            distanceAlt += FindDistance(linesArray[i, 0], linesArray[i, 1]);
            print("---------------------------------->distanceAlt" + distanceAlt);
        }
        linesCount = 0;
    }
    float CalculateSurface()
    {
        float surfaceValue = 0f;
        float m1 = 0f, m2 = 0f, m3 = 0f, m4 = 0f; ;
        float c;
        Vector2 partialPoint;
        Vector2 sCor = sunCoordinates;
        bool partial = false;
        bool sunLow;

        if (onTop)
        {
            //surfaceValue += FindDistance(buildingData[bisectionPoint, 3].x, buildingData[bisectionPoint, 0].x, buildingData[bisectionPoint, 3].y, buildingData[bisectionPoint, 0].y);
            DrawLine(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);
            surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);
        }
        if (bisectionPoint != 0) // START OF LEFT <<<<<<<<<<<<<<------------------------------------------------------------------------------------------
        {
            sunLow = false;
            //m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 0].x, sCor.y, buildingData[bisectionPoint, 0].y); ///try NOW
            float maxSlope = -Mathf.Infinity;
            float minSlope = -Mathf.Infinity;
            float maxheight = -Mathf.Infinity;
            //float maxSlope = float.NegativeInfinity;
            //float minSlope = float.NegativeInfinity;
            //float maxheight = float.NegativeInfinity;
            bool previousFlag = false;
            int maxSlopeIndex = bisectionPoint - 1;
            int minSlopeIndex = bisectionPoint - 1;
            if (onTop)
            {
                m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 0].x, sCor.y, buildingData[bisectionPoint, 0].y);
                minSlope = m1;
                print("ONTOP MINSLOPE" + m1);
                minSlopeIndex = bisectionPoint;
                maxSlopeIndex = bisectionPoint;
                maxheight = buildingData[bisectionPoint, 3].y;
            }
            else if (bisectionPoint > 0 && !sunRightmost)
            {
                if (!onTop && sCor.y > buildingData[bisectionPoint - 1, 0].y) //Sun is above the next building//////
                {
                    surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 0]);
                    DrawLine(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 0]);
                    surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);
                    DrawLine(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);
                    //if (bisectionPoint < numOfBuildings) //TryNOW
                    m1 = FindSlope(sCor.x, buildingData[bisectionPoint - 1, 0].x, sCor.y, buildingData[bisectionPoint - 1, 0].y);
                    minSlope = m1;
                    maxSlopeIndex = bisectionPoint - 1;
                    minSlopeIndex = bisectionPoint - 1;
                    maxheight = buildingData[bisectionPoint - 1, 3].y;
                    //else
                    //  m1 = FindSlope(sCor.x, buildingData[bisectionPoint - 2, 0].x, sCor.y, buildingData[bisectionPoint - 2, 0].y);///////
                }
                else if (!onTop && sCor.y <= buildingData[bisectionPoint - 1, 0].y) // Sun is below the next building
                {
                    surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);
                    DrawLine(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);
                    m3 = FindSlope(sCor.x, buildingData[bisectionPoint - 1, 3].x, sCor.y, buildingData[bisectionPoint - 1, 3].y);
                    maxSlope = m3;
                    maxSlopeIndex = bisectionPoint - 1;
                    previousFlag = true;
                    sunLow = true;
                    maxheight = buildingData[bisectionPoint - 1, 3].y;
                }
             

            }
            else if (sunRightmost)
            {
                if (!onTop && sCor.y > buildingData[bisectionPoint, 0].y) //Sun is above the next building//////
                {
                    print("SR first if");
                    surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);
                    DrawLine(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);
                    surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 2]);
                    DrawLine(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 2]);
                    //if (bisectionPoint < numOfBuildings) //TryNOW
                    m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 0].x, sCor.y, buildingData[bisectionPoint, 0].y);
                    minSlope = m1;
                    maxSlopeIndex = bisectionPoint;
                    minSlopeIndex = bisectionPoint;
                    maxheight = buildingData[bisectionPoint, 3].y;
                    //else
                    //  m1 = FindSlope(sCor.x, buildingData[bisectionPoint - 2, 0].x, sCor.y, buildingData[bisectionPoint - 2, 0].y);///////
                }
                else if (!onTop && sCor.y <= buildingData[bisectionPoint, 0].y) // Sun is below the next building
                {
                    print("SR second if");
                    surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 2]);
                    DrawLine(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 2]);
                    m3 = FindSlope(sCor.x, buildingData[bisectionPoint, 3].x, sCor.y, buildingData[bisectionPoint, 3].y);
                    maxSlope = m3;
                    maxSlopeIndex = bisectionPoint;
                    previousFlag = true;
                    sunLow = true;
                    maxheight = buildingData[bisectionPoint, 3].y;
                }
            }

            for (int i = (onTop || sunRightmost) ? bisectionPoint - 1 : bisectionPoint - 2; i >= 0; i--)
            {

                m3 = FindSlope(sCor.x, buildingData[i + 1, 3].x, sCor.y, buildingData[i + 1, 3].y);// slope of next building
                if (m3 < maxSlope)
                {
                    maxSlope = m3;
                    maxSlopeIndex = i + 1;
                }
                m4 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
                if (m4 < minSlope)
                {
                    minSlope = m4;
                    minSlopeIndex = i + 1;
                }
                if (maxheight < buildingData[i + 1, 3].y)
                {
                    maxheight = buildingData[i + 1, 3].y;
                }
                print(i + "st MAX HEIGHT" + maxheight);
                // Debug.Log(i + " M1: " + m1);
                if (buildingData[i, 0].y < sCor.y && sunLow == false)// next  building is shorter than the sun's height
                {

                    for (int j = 2; j != 1; j = (j + 1) % 4)
                    {
                        m2 = FindSlope(sCor.x, buildingData[i, j].x, sCor.y, buildingData[i, j].y);
                        //Debug.Log(i + " " + j + " M2: " + m2);
                        if (j == 2)//for 2,3 |
                        {
                            //Debug.Log(i + " " + j + " Minslope | M2 for j=2 " + minSlope +m2);
                            if (m2 <= minSlope) //if slope of vertex 2 of building is less than minslope, then that vertex along with the line segment can be lit
                            {

                                surfaceValue += FindDistance(buildingData[i, 2], buildingData[i, 3]);
                                DrawLine(buildingData[i, 2], buildingData[i, 3]);
                                //Debug.Log(i + " " + j + " 2,3|| " + m2);
                                partial = false;
                                //print(j + "Partial false");
                            }
                            else
                            {
                                partial = true;
                                //print(j + "Partial true");
                            }


                        }
                        else if (j == 3)// for 3,0 -
                        {
                            if (m2 <= minSlope && partial == true)
                            {
                                m3 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
                                c = FindYIntercept(sCor.x, sCor.y, minSlope);
                                partialPoint.x = buildingData[i, 3].x;
                                partialPoint.y = FindY(buildingData[i, 3].x, minSlope, c);
                                //Debug.Log("ParPoint: " + partialPoint);// ||||||||
                                surfaceValue += FindDistance(buildingData[i, 3], partialPoint);
                                DrawLine(buildingData[i, 3], partialPoint);
                                surfaceValue += FindDistance(buildingData[i, 3], buildingData[i, 0]);
                                DrawLine(buildingData[i, 3], buildingData[i, 0]);
                                partial = false;
                            }
                            else if (m2 <= minSlope && partial == false)
                            {
                                surfaceValue += FindDistance(buildingData[i, 3], buildingData[i, 0]);
                                DrawLine(buildingData[i, 3], buildingData[i, 0]);
                                //Debug.Log("YO ");// ||||||||
                            }
                            else if (m2 >= minSlope)
                            {
                                partial = true;
                            }
                        }
                        else if (j == 0)
                        {
                            if (m2 <= minSlope && partial == true)
                            {
                                m3 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
                                c = FindYIntercept(sCor.x, sCor.y, minSlope);
                                partialPoint.y = buildingData[i, 0].y;
                                partialPoint.x = FindX(buildingData[i, 0].y, minSlope, c);
                                //Debug.Log("ParPoint: " + partialPoint);// --------
                                surfaceValue += FindDistance(buildingData[i, 0], partialPoint);
                                DrawLine(buildingData[i, 0], partialPoint);
                                partial = false;
                            }
                            else
                            {
                                partial = false;
                            }
                        }
                    }
                }
                else if (buildingData[i, 0].y >= sCor.y || sunLow == true)// next building is taller than the sun's height
                {
                    sunLow = true;
                    if (buildingData[i, 0].y <= maxheight)//building is shorter than max height
                    {
                        print("------------------------------BUiLDING IGNORED" + i + " MAX HEIGHT-------------------");

                        continue;
                    }
                    else
                    {
                        if (buildingData[maxSlopeIndex, 0].y > sCor.y)  //> sCor.y) // previous building is taller than sun
                        {
                            m3 = FindSlope(sCor.x, buildingData[i + 1, 3].x, sCor.y, buildingData[i + 1, 3].y);
                            m2 = FindSlope(sCor.x, buildingData[i, 3].x, sCor.y, buildingData[i, 3].y);
                            //print("TEST maxslope, m2 " + maxSlope + " " + m2);
                            if (m2 > maxSlope)
                                continue;
                            //print("HIYYYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa");
                            c = FindYIntercept(sCor.x, sCor.y, maxSlope);
                            partialPoint.x = buildingData[i, 3].x;
                            partialPoint.y = FindY(buildingData[i, 3].x, maxSlope, c);
                            surfaceValue += FindDistance(buildingData[i, 3], partialPoint);
                            DrawLine(buildingData[i, 3], partialPoint);
                            previousFlag = true;
                        }
                        else if (!previousFlag)
                        {// previous building is shorter than sun
                            m2 = FindSlope(sCor.x, buildingData[i, 2].x, sCor.y, buildingData[i, 2].y); //CHECK TryNOW
                            m3 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
                            if (m2 <= minSlope)
                            {
                                print("m2: " + m2 + " <= minSlope:" + minSlope);
                                surfaceValue += FindDistance(buildingData[i, 3], buildingData[i, 2]);
                                DrawLine(buildingData[i, 3], buildingData[i, 2]);
                            }
                            else
                            {

                                c = FindYIntercept(sCor.x, sCor.y, minSlope);
                                partialPoint.x = buildingData[i, 3].x;
                                partialPoint.y = FindY(buildingData[i, 3].x, minSlope, c);
                                surfaceValue += FindDistance(buildingData[i, 3], partialPoint);
                                DrawLine(buildingData[i, 3], partialPoint);
                            }
                        }
                    }
                }

                m1 = FindSlope(sCor.x, buildingData[i, 0].x, sCor.y, buildingData[i, 0].y);
                //Debug.Log(i + " M1: " + m1);
            }

        }// END OF LEFT <<<<<<<<<<<<<<------------------------------------------------------------------------------------------
        if ((bisectionPoint < numOfBuildings - 1 || !onTop) && !sunRightmost) //START for right bisection ----------------------------------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>
        {
            sunLow = false;
            //m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 0].x, sCor.y, buildingData[bisectionPoint, 0].y);

            float maxSlope = -Mathf.Infinity;
            float minSlope = -Mathf.Infinity;
            float maxheight = -Mathf.Infinity;
            //float maxSlope = float.NegativeInfinity;
            //float minSlope = float.NegativeInfinity;
            //float maxheight = float.NegativeInfinity;
            bool previousFlag = false;
            int maxSlopeIndex = bisectionPoint;
            int minSlopeIndex = bisectionPoint;

            if (onTop)
            {
                //surfaceValue += FindDistance(buildingData[bisectionPoint, 3].x, buildingData[bisectionPoint, 0].x, buildingData[bisectionPoint, 3].y, buildingData[bisectionPoint, 0].y);
                m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 3].x, sCor.y, buildingData[bisectionPoint, 3].y);
                minSlope = m1;
                maxheight = buildingData[bisectionPoint, 3].y;
                minSlopeIndex = bisectionPoint;
                maxSlopeIndex = bisectionPoint;
            }
            else if (!onTop && sCor.y > buildingData[bisectionPoint, 0].y) //sun above
            {
                surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);
                DrawLine(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);
                surfaceValue += FindDistance(buildingData[bisectionPoint, 0], buildingData[bisectionPoint, 1]);
                DrawLine(buildingData[bisectionPoint, 0], buildingData[bisectionPoint, 1]);
                m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 3].x, sCor.y, buildingData[bisectionPoint, 3].y);
                minSlope = m1;
                maxSlopeIndex = bisectionPoint - 1;
                minSlopeIndex = bisectionPoint - 1;
                maxheight = buildingData[bisectionPoint, 3].y;
            }
            else if (!onTop && sCor.y <= buildingData[bisectionPoint, 0].y)//sun below
            {
                surfaceValue += FindDistance(buildingData[bisectionPoint, 0], buildingData[bisectionPoint, 1]);
                DrawLine(buildingData[bisectionPoint, 0], buildingData[bisectionPoint, 1]);
                m3 = FindSlope(sCor.x, buildingData[bisectionPoint, 0].x, sCor.y, buildingData[bisectionPoint, 0].y);
                maxSlope = m3;
                maxSlopeIndex = bisectionPoint;
                previousFlag = true;
                sunLow = true;
                maxheight = buildingData[bisectionPoint, 3].y;
            }


            for (int i = bisectionPoint + 1; i < numOfBuildings; i++) //OLD
                                                                      //for (int i = onTop ? bisectionPoint:bisectionPoint+1; i < numOfBuildings; i++)
            {

                m3 = FindSlope(sCor.x, buildingData[i - 1, 0].x, sCor.y, buildingData[i - 1, 0].y);// slope of next building
                if (m3 > maxSlope) //TRY NEW
                {
                    maxSlope = m3;
                    maxSlopeIndex = i - 1;
                }
                m4 = FindSlope(sCor.x, buildingData[i - 1, 3].x, sCor.y, buildingData[i - 1, 3].y);
                if (m4 > minSlope)
                {
                    minSlope = m4;
                    minSlopeIndex = i - 1;
                }
                if (maxheight < buildingData[i - 1, 3].y)
                {
                    maxheight = buildingData[i - 1, 3].y;
                }
                //Debug.Log(i+" M1: " + m1);
                if (buildingData[i, 0].y < sCor.y && sunLow == false)// next  building is shorter than the sun's height
                {



                    //print(i + "m3|m4|Min Slope" + m3 + " " + m4 + " " + minSlope + " ");
                    for (int j = 1; j != 2; j = (j != 0) ? (j - 1) % 4 : (j + 3))// for j to loop as 1,0,3 and terminate at 2
                    {
                        m2 = FindSlope(sCor.x, buildingData[i, j].x, sCor.y, buildingData[i, j].y);


                        //Debug.Log(i+" "+j+" M2: " + m2);
                        if (j == 1)// for 1,0 |
                        {
                            if (m2 >= minSlope) //if slope of vertex 1 of building is less than slope of 3th vertex of current building, then that vertex along with the line segment can be lit
                            {
                                surfaceValue += FindDistance(buildingData[i, 1], buildingData[i, 0]);
                                DrawLine(buildingData[i, 1], buildingData[i, 0]);
                                partial = false;
                            }
                            else
                            {
                                //Debug.Log("M2<M1");
                                partial = true;
                            }


                        }
                        else if (j == 0)// for 0,3 - 
                        {
                            if (m2 >= minSlope && partial == true)
                            {
                                // m3 = FindSlope(sCor.x, buildingData[i - 1, 3].x, sCor.y, buildingData[i - 1, 3].y);
                                c = FindYIntercept(sCor.x, sCor.y, minSlope);
                                partialPoint.x = buildingData[i, 0].x; // |||||
                                partialPoint.y = FindY(buildingData[i, 0].x, minSlope, c);
                                surfaceValue += FindDistance(buildingData[i, 0], partialPoint);
                                DrawLine(buildingData[i, 0], partialPoint);
                                surfaceValue += FindDistance(buildingData[i, 0], buildingData[i, 3]);
                                DrawLine(buildingData[i, 0], buildingData[i, 3]);
                                partial = false;
                            }
                            else if (m2 >= minSlope && partial == false)
                            {
                                surfaceValue += FindDistance(buildingData[i, 0], buildingData[i, 3]);
                                DrawLine(buildingData[i, 0], buildingData[i, 3]);
                            }
                            else if (m2 <= minSlope)
                            {
                                partial = true;
                            }
                        }
                        else if (j == 3)
                        {
                            if (m2 >= minSlope && partial == true)
                            {
                                //m3 = FindSlope(sCor.x, buildingData[i - 1, 3].x, sCor.y, buildingData[i - 1, 3].y);
                                c = FindYIntercept(sCor.x, sCor.y, minSlope);
                                partialPoint.y = buildingData[i, 3].y;
                                partialPoint.x = FindX(buildingData[i, 3].y, minSlope, c);
                                surfaceValue += FindDistance(buildingData[i, 3], partialPoint);
                                DrawLine(buildingData[i, 3], partialPoint);
                                partial = false;
                            }
                            else
                            {
                                partial = false;
                            }
                        }
                    }
                }
                else if (buildingData[i, 0].y >= sCor.y || sunLow == true)// building is taller than the sun's height
                {

                    //print(maxheight);
                    sunLow = true;
                    if (buildingData[i, 0].y <= maxheight)// building is shorter than previous building
                    {
                        //print("oh no");
                        continue;
                    }
                    else
                    {
                        if (buildingData[maxSlopeIndex, 0].y > sCor.y) // previous building is taller than sun ( case  I ) 
                        {   // from here

                            m2 = FindSlope(sCor.x, buildingData[i, 0].x, sCor.y, buildingData[i, 0].y);// slope of current building
                            if (m2 < maxSlope)
                                continue;//right
                            c = FindYIntercept(sCor.x, sCor.y, maxSlope);
                            partialPoint.x = buildingData[i, 0].x;
                            partialPoint.y = FindY(partialPoint.x, maxSlope, c);
                            surfaceValue += FindDistance(buildingData[i, 0], partialPoint);
                            DrawLine(buildingData[i, 0], partialPoint);
                            previousFlag = true;

                        }
                        else if (!previousFlag) // previous building is shorter than sun (Case II)
                        {
                            m2 = FindSlope(sCor.x, buildingData[i, 1].x, sCor.y, buildingData[i, 1].y);
                            //m3 = FindSlope(sCor.x, buildingData[i-1, 0].x, sCor.y, buildingData[i-1, 0].y);
                            if (m2 >= minSlope)
                            {
                                surfaceValue += FindDistance(buildingData[i, 0], buildingData[i, 1]);
                                DrawLine(buildingData[i, 0], buildingData[i, 1]);
                            }
                            else
                            {
                                c = FindYIntercept(sCor.x, sCor.y, minSlope);
                                partialPoint.x = buildingData[i, 0].x;
                                partialPoint.y = FindY(partialPoint.x, minSlope, c);
                                surfaceValue += FindDistance(buildingData[i, 0], partialPoint);
                                DrawLine(buildingData[i, 0], partialPoint);
                            }
                        }
                    }

                    m1 = FindSlope(sCor.x, buildingData[i, 0].x, sCor.y, buildingData[i, 0].y);
                }






            }

        }//END for right bisection ----------------------------------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>// 
         //Debug.Log("Debug: " + FindDistance(5, 1, 5, 1));
         //Debug.Log("Surface: " + surfaceValue);
        DrawLineOnBuilding();
        return surfaceValue;
    }


}
