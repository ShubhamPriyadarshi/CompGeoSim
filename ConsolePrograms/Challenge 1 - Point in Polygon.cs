using System;
using System.Text.RegularExpressions;
using static System.Console;

public class PointInPolygon
{
    int numOfVertices = 0;
    Vector2 pointCoordinates;
    Vector2[] polygonData;

    struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    static public void Main(string[] args)
    {
        PointInPolygon PP = new PointInPolygon();
        string userInput;


        WriteLine("Enter the values of [N x 2] array consisting the coordinates of N vertices of a polygon in 2D space");
        userInput = ReadLine();
        userInput = userInput.Replace("], [", " ").Replace("],", " ").Replace("[", "").Replace("]", "").Trim();
        userInput = Regex.Replace(userInput, @"\s+", " ");
        string[] polygonDataRaw = userInput.Split(' ');

        while (polygonDataRaw.Length < 3)
        {
            PP.numOfVertices = 0;
            WriteLine("Wrong number of coordinates ( Enter atleast 3 coordinates for the polygon ) or wrong format, please try again. ( FORMAT: [[1,0], [8,3], [8,8], [1,5]] )");
            WriteLine("Enter the values of [N x 2] array consisting the coordinates of N vertices of a polygon in 2D space");
            userInput = ReadLine();
            userInput = userInput.Replace("], [", " ").Replace("],", " ").Replace("[", "").Replace("]", "").Trim();
            userInput = Regex.Replace(userInput, @"\s+", " ");
            polygonDataRaw = userInput.Split(' ');
        }
        PP.ProcessRawCoordinates(polygonDataRaw, 0);
        WriteLine("Enter the coordinates of point in 2D space");
        userInput = ReadLine();
        userInput = userInput.Replace("[", "").Replace("]", "");
        string[] pointCoordinatesRaw = userInput.Split(',');
        while (!(pointCoordinatesRaw.Length == 2))
        {

            WriteLine("Wrong number of coordinates or wrong format, please try again. ( FORMAT: [1,1] )");
            WriteLine("Enter the coordinates of point in 2D space");
            userInput = ReadLine();
            pointCoordinatesRaw = userInput.Split(',');
        }
        PP.ProcessRawCoordinates(pointCoordinatesRaw, 1);
        bool result = PP.IsPointInPolygon();
        WriteLine("Output (Is point inside the polygon?): " + result);
    }

    void ProcessRawCoordinates(string[] rawString, int mode)
    {

        switch (mode)
        {
            case (0):
                numOfVertices = rawString.Length;
                PolygonCoordinates(rawString);
                break;

            case (1):
                float coordX, coordY;
                coordX = Convert.ToSingle(rawString[0]);
                coordY = Convert.ToSingle(rawString[1]);
                pointCoordinates = new Vector2(coordX, coordY);
                break;
        }
    }



    void PolygonCoordinates(string[] polygonDataRaw)
    {
        polygonData = new Vector2[numOfVertices];
        for (int i = 0; i < numOfVertices; i++)
        {
            string[] temp = polygonDataRaw[i].Split(',');
            float tempX = Convert.ToSingle(temp[0]);
            float tempY = Convert.ToSingle(temp[1]);
            polygonData[i] = new Vector2(tempX, tempY);
        }



    }

    bool IsPointInPolygon()
    {
        int i, j = numOfVertices - 1;
        bool oddNodes = false;

        for (i = 0; i < numOfVertices; i++)
        {
            if (polygonData[i].Y < pointCoordinates.Y && polygonData[j].Y >= pointCoordinates.Y
            || polygonData[j].Y < pointCoordinates.Y && polygonData[i].Y >= pointCoordinates.Y)
            {
                if (polygonData[i].X + (pointCoordinates.Y - polygonData[i].Y) / (polygonData[j].Y - polygonData[i].Y) * (polygonData[j].X - polygonData[i].X) < pointCoordinates.X)
                {
                    oddNodes = !oddNodes;
                }
            }
            j = i;
        }

        return oddNodes;
    }
}

