using System;
using System.Text.RegularExpressions;
using static System.Console;

public class CircleChord
{

    public Vector2D circlePos;
    public float radius;
    public float angle;
    public float clearance;
    public double lineLength;

    public class Vector2D
    {
        public double x;
        public double y;
        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    static public void Main()
    {
        CircleChord C = new CircleChord();
        string[] rawInputData = C.ReadInputData();
        C.ProcessRawCoordinates(rawInputData);


        if (C.lineLength == 2 * C.radius)
            C.lineLength -= 1E-14;

        C.GetChordCoordinates();
    }

    string[] ReadInputData()
    {
        string userInput;
        WriteLine("Enter the values of x, y, radius, angle (in degrees), clearance, lineLength (separated by comma)");
        userInput = ReadLine();
        userInput = userInput.Replace(",", " ").Trim();
        userInput = Regex.Replace(userInput, @"\s+", " ");
        string[] InputDataRaw = userInput.Split(' ');
        while (InputDataRaw.Length != 6)
        {
            WriteLine("Wrong number of coordinates or wrong format, please try again. ( FORMAT: 5, 5, 5, 45, 2, 8 )");
            WriteLine("Enter the values of x, y, radius, angle (in degrees), clearance, lineLength (separated by comma)");
            userInput = ReadLine();
            userInput = userInput.Replace(",", " ").Trim();
            userInput = Regex.Replace(userInput, @"\s+", " ");
            InputDataRaw = userInput.Split(' ');
        }
        return InputDataRaw;
    }

    void ProcessRawCoordinates(string[] rawString)
    {
        float coordX, coordY;
        coordX = Convert.ToSingle(rawString[0]);
        coordY = Convert.ToSingle(rawString[1]);
        circlePos = new Vector2D(coordX, coordY);

        radius = Convert.ToSingle(rawString[2]);
        angle = Convert.ToSingle(rawString[3]);
        clearance = Convert.ToSingle(rawString[4]);
        lineLength = Convert.ToDouble(rawString[5]);

    }
    void GetChordCoordinates()
    {
        bool intersects = true;

        if (angle == 0)
            angle = 360;

        double angleRad = angle * Convert.ToSingle(Math.PI) / 180f;
        double circlepoint_x = circlePos.x + ((radius) * Math.Cos(angleRad));
        double mid_x = circlePos.x + ((radius + clearance) * Math.Cos(angleRad));

        double mid_y = circlePos.y + ((radius + clearance) * Math.Sin(angleRad));
        double circlepoint_y = circlePos.y + ((radius) * Math.Sin(angleRad));
        Vector2D circlePoint = new Vector2D(circlepoint_x, circlepoint_y);
        Vector2D lineSegmentMid = new Vector2D(mid_x, mid_y);
        Vector2D circlePosD = new Vector2D(circlePos.x, circlePos.y);

        double slope = (mid_y - circlePos.y) / (mid_x - circlePos.x);
        double slopeInverse = -1 / slope;


        Vector2D[] lineSegment = GetLineCoordinates(mid_x, mid_y, mid_x, mid_y, slopeInverse, lineLength / 2);
        Vector2D[] chordCoordinates = new Vector2D[2];
        Vector2D[] tempLine;
        Vector2D P, Q;


        if ((angle < 90 && angle > 0) || angle >= 270)
        {
            tempLine = GetLineCoordinates(lineSegment[0].x, lineSegment[0].y, circlePosD.x, circlePos.y, slope, radius); // for lower coordinate
            chordCoordinates[0] = new Vector2D(tempLine[0].x, tempLine[0].y);
            tempLine = GetLineCoordinates(lineSegment[1].x, lineSegment[1].y, circlePosD.x, circlePos.y, slope, radius); // for upper coordinate
            chordCoordinates[1] = new Vector2D(tempLine[0].x, tempLine[0].y);
            if (chordCoordinates[0].x == 0 && chordCoordinates[0].y == 0 && chordCoordinates[1].x == 0 && chordCoordinates[1].y == 0)
            {
                intersects = false;
            }
        }
        else if (angle >= 90 && angle < 270)
        {
            tempLine = GetLineCoordinates(lineSegment[0].x, lineSegment[0].y, circlePosD.x, circlePos.y, slope, radius); // for lower coordinate
            chordCoordinates[0] = new Vector2D(tempLine[1].x, tempLine[1].y);
            tempLine = GetLineCoordinates(lineSegment[1].x, lineSegment[1].y, circlePosD.x, circlePos.y, slope, radius); // for upper coordinate
            chordCoordinates[1] = new Vector2D(tempLine[1].x, tempLine[1].y);
            if (chordCoordinates[0].x == 0 && chordCoordinates[0].y == 0 && chordCoordinates[1].x == 0 && chordCoordinates[1].y == 0)
            {

                intersects = false;
            }

        }
        if (intersects)
        {
            P = chordCoordinates[0];
            Q = chordCoordinates[1];
            WriteLine("Output: Values of P:" + "( " + Math.Round(P.x, 3) + ", " + Math.Round(P.y, 3) + " )" + "& Q:" + "(" + Math.Round(Q.x, 3) + ", " + Math.Round(Q.y, 3) + ")");
        }
        else
        {
            WriteLine("Output: Lines do not interesect the circle");

        }
    }

    Vector2D[] GetLineCoordinates(double x, double y, double a, double b, double m, double r)
    {
        Vector2D[] lineSegment = new Vector2D[2] { new Vector2D(0, 0), new Vector2D(0, 0) };

        double c = y - m * x;
        double d = (Math.Pow(r, 2) * (1 + Math.Pow(m, 2)) - Math.Pow((b - m * a - c), 2));
        if (d < 0)
            return lineSegment;
        lineSegment[0].x = (a + b * m - c * m + Math.Sqrt(d)) / (1 + Math.Pow(m, 2));//lower x
        lineSegment[1].x = (a + b * m - c * m - Math.Sqrt(d)) / (1 + Math.Pow(m, 2));
        lineSegment[0].y = (c + a * m + b * Math.Pow(m, 2) + m * Math.Sqrt(d)) / (1 + Math.Pow(m, 2));// lower y
        lineSegment[1].y = (c + a * m + b * Math.Pow(m, 2) - m * Math.Sqrt(d)) / (1 + Math.Pow(m, 2));

        return lineSegment;
    }
}
