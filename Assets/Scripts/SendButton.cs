using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using UnityEngine;
using UnityEngine.UI;


public class SendButton : MonoBehaviour
{
    int it;
    public InputField inputField;
    public Text currText;
    GameObject IO;
    public GameObject Buildings;
    public GameObject defaultOptions1;
    public GameObject defaultOptions2;
    public GameObject resetButton;
    public GameObject SimulationMode;

    void Start()
    {
        it = 0;
        IO = GameObject.Find("IO");

    }

    public void RetrieveCmd()
    {
        List<string> cmd = new List<string>();
        string temp = inputField.text.ToString();
        inputField.text = "";
        cmd.Add(temp);
        
        //Debug.Log(cmd[0]);
        switch (ConsoleInputs.option)
        {
            case (1):
                
                int numOfVertices;
                switch (it)
                {

                    case (0):
                        

                        temp = temp.Replace("], [", " ").Replace("[", "").Replace("]", "");
                        string[] coordinates = temp.Split(' ');
                        numOfVertices = coordinates.Length;
                        ConsoleInputs.PolygonData.numOfVertices = numOfVertices;
                      
                        if (numOfVertices>=3)
                        {
                            currText.text = "Enter the coordinates of the point ( X and Y separated by a comma)";
                            ConsoleInputs.PolygonData.polygonDataRaw = temp.Split(' ');
                            defaultOptions1.SetActive(false);
                        }
                        else
                        {
                            currText.text = "Error: Wrong number of coordinates ( enter atleast three coordinates ) or wrong format, try again ( Tip: Use the format given in default options )";
                            it--;
                        }
                        
                        //else if (temp == "")
                        //{
                        //    if (7 == numOfVertices)
                        //    {
                        //        {
                        //            string defaultOption = "3.36,6.79 3.49,4.70 1.03,3.44 2.18,1.19 3.72,3.78 6.00,1.78 6.86,3.89";

                        //            print(defaultOption);
                        //            ConsoleInputs.PolygonData.polygonDataRaw = defaultOption.Split(' ');
                        //            defaultOptions1.SetActive(false);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        currText.text = "Error: Wrong number of coordinates or wrong format, enter coordinates of " + numOfVertices + " Vertices";
                        //        it--;
                        //    }
                        //}
                        // Debug.Log(ConsoleInputs.PolygonData.polygonDataRaw);


                        break;


                    case (1):
                        string[] coord;
                        float coordX, coordY;
                        if (temp != "")
                        {
                            temp = temp.Replace("[", "").Replace("]", "");
                            coord = temp.Split(',');
                            coordX = Convert.ToSingle(coord[0]);
                            coordY = Convert.ToSingle(coord[1]);
                            ConsoleInputs.PolygonData.pointCoordinates = new Vector2(coordX, coordY);
                            //Debug.Log(ConsoleInputs.PolygonData.pointCoordinates);
                        }
                        else if (temp == "")
                            ConsoleInputs.PolygonData.pointCoordinates = new Vector2(1, 1);
                        SimulationMode.SetActive(true);
                        IO.SetActive(false);
                        resetButton.SetActive(true);
                        ConsoleInputs.DrawPolygon();
                        break;
                }
                break;
            case (2):
                int numOfBuildings;
                switch (it) {

                    case (0):
                        if (temp != "")
                        {
                            temp = temp.Replace("]], [[", " ").Replace("],[", " ").Replace("[", "").Replace("]", "");
                            string[] coordinates = temp.Split(' ');
                            if (coordinates.Length % 4 == 0 && coordinates.Length >=4) 
                            {
                                defaultOptions2.SetActive(false);
                                numOfBuildings = coordinates.Length / 4;
                                ConsoleInputs.BuildingData.numOfBuildings = numOfBuildings;
                                ConsoleInputs.BuildingData.buildingDataRaw = coordinates;
                                currText.text = "Enter the coordinates of the light source ( X and Y separated by a comma) ( Default [1,1] )";
                            }
                            else
                            {
                                currText.text = "Wrong number of coordinates or wrong format, please try again. ( Tip: Use the format given in default options )";
                                it--;
                            }
                        }
                        else
                        {
                            currText.text = "Wrong number of coordinates or wrong format, please try again. ( Tip: Use the format given in default options )";
                            it--;
                        }
                        break;
                        
                        


                    case (1):
                        string[] coord;
                        float coordX, coordY;
                        if (temp != "")
                        {
                            temp = temp.Replace("[", "").Replace("]", "");
                            coord = temp.Split(',');
                            coordX = Convert.ToSingle(coord[0]);
                            coordY = Convert.ToSingle(coord[1]);
                            ConsoleInputs.BuildingData.sunCoordinates = new Vector2(coordX, coordY);
                        }
                        else
                        {
                            ConsoleInputs.BuildingData.sunCoordinates = new Vector2(1, 1);
                        }
                        SimulationMode.SetActive(true);
                       // Debug.Log(ConsoleInputs.BuildingData.sunCoordinates);
                        IO.SetActive(false);
                        resetButton.SetActive(true);
                        ConsoleInputs.DrawBuilding();
                        break;



                        //case(3):


                } break;
                
        } it++;
    }
}

//case (1):
                        
//                        currText.text = "Enter the coordinates of the light source ( X and Y separated by a comma) ( Default [1,1] )";
//                         numOfBuildings = ConsoleInputs.BuildingData.numOfBuildings;
//                        if (temp != "")
//                        {
//                            temp = temp.Replace("]], [[", " ").Replace("],[", " ").Replace("[", "").Replace("]", "");
//                            print(temp);
//                            string[] coordinates = temp.Split(' ');
//                            if (coordinates.Length == numOfBuildings * 4)
//                            {
//                                ConsoleInputs.BuildingData.buildingDataRaw = coordinates;
//                                defaultOptions2.SetActive(false);
//                            }
//                            else
//                            {
//                                currText.text = "Error: Wrong number of Coordinates or Wrong format, enter coordinates of " + numOfBuildings + " Building(s)";
//                                it--;
//                            }

//                        }
//                        else if (temp == "")
//                        {
//                            string defaultOption = "[[4,0],[4,-7],[7,-7],[7,0]], [[0.4,-2],[0.4,-7],[2.5,-7],[2.5,-2]], [[8.5,2],[8.5,-7],[11.5,-7],[11.5,2]], [[-11.5,5],[-11.5,-7],[-8.5,-7],[-8.5,5]], [[-7,-3],[-7,-7],[-4,-7],[-4,-3]], [[-20.5,5],[-20.5,-7],[-15,-7],[-15,5]], [[15.5,2],[15.5,-7],[20.5,-7],[20.5,2]]]";
//                            defaultOption = defaultOption.Replace("]], [[", " ").Replace("],[", " ").Replace("[", "").Replace("]", "");
//                            print(defaultOption);
//                            if (28 == numOfBuildings * 4)
//                            {
//                                ConsoleInputs.BuildingData.buildingDataRaw = defaultOption.Split(' ');
//                                defaultOptions2.SetActive(false);
//                            }

//                            else
//                            {
//                                currText.text = "Error: Wrong number of Coordinates or Wrong format, enter coordinates of " + numOfBuildings + " Building(s)";
//                                it--;
//                            }
//                            //Debug.Log(ConsoleInputs.BuildingData.buildingDataRaw);
//                        }

//                        break;