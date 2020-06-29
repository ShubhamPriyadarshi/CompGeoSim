using System;
using System.Text.RegularExpressions;
using static System.Console;

public class SurfaceCover
{
	int numOfBuildings;
	Vector2 sunCoordinates;
	Vector2[,] buildingData;
	bool onTop;
	int bisectionPoint;
	bool sunRightmost;

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
		SurfaceCover SC = new SurfaceCover();

		string[] rawBuildingCoordinates = SC.ReadBuildingCoordinates();
		SC.ProcessRawCoordinates(rawBuildingCoordinates, 0);
		string[] rawPointCoordinates = SC.ReadSunCoordinates();
		SC.ProcessRawCoordinates(rawPointCoordinates, 1);
		SC.bisectionPoint = SC.Bisection();
		while (SC.bisectionPoint == -1)
		{
			WriteLine("Invalid Coordinates of the Sun ( It must be above the ground and not in between buildings) ");
			rawPointCoordinates = SC.ReadSunCoordinates();
			SC.ProcessRawCoordinates(rawPointCoordinates, 1);
			SC.bisectionPoint = SC.Bisection();
		}	
		
		float result = SC.CalculateSurface();
		WriteLine("Output (Length of surface exposed to sunlight) : " + result);
		
	}

	string[] ReadBuildingCoordinates()
	{
		string userInput;
		WriteLine("Enter n x 4 x 2 array consisting the coordinates of n buildings in 2-D, where n is number of buildings");
		userInput = ReadLine();
		userInput = userInput.Replace("]], [[", " ").Replace("]],", " ").Replace("],[", " ").Replace("[", "").Replace("]", "").Trim();
		userInput = Regex.Replace(userInput, @"\s+", " ");
		string[] buildingDataRaw = userInput.Split(' ');
		while (!(buildingDataRaw.Length % 4 == 0) && !(buildingDataRaw.Length == 0))
		{
			WriteLine("Wrong number of coordinates or wrong format, please try again. ( FORMAT: [[[4,0],[4,-5],[7,-5],[7,0]], [[0.4,-2],[0.4,-5],[2.5,-5],[2.5,-2]]] )");
			WriteLine("Enter n x 4 x 2 array consisting the coordinates of n buildings in 2-D, where n is number of buildings");
			userInput = ReadLine();
			userInput = userInput.Replace("]], [[", " ").Replace("],[", " ").Replace("[", "").Replace("]", "").Trim();
			userInput = Regex.Replace(userInput, @"\s+", " ");
			buildingDataRaw = userInput.Split(' ');
		}
		return buildingDataRaw;

	}

	string[] ReadSunCoordinates()
	{
		string userInput;
		WriteLine("Enter the coordinates of source of light in 2-D");
		userInput = ReadLine();
		string[] pointCoordinates = userInput.Split(',');
		while (!(pointCoordinates.Length == 2))
		{

			WriteLine("Wrong number of coordinates or wrong format, please try again. ( FORMAT: [1,1] )");
			WriteLine("Enter the coordinates of source of light in 2-D");
			userInput = ReadLine();
			pointCoordinates = userInput.Split(',');
		}
		return pointCoordinates;
	}

	void ProcessRawCoordinates(string[] rawString, int mode)
	{
		switch (mode)
		{
			case (0):
				numOfBuildings = rawString.Length / 4;
				BuildingCoordinates(rawString);
				break;

			case (1):
				float coordX,
				coordY;
				coordX = Convert.ToSingle(rawString[0]);
				coordY = Convert.ToSingle(rawString[1]);
				sunCoordinates = new Vector2(coordX, coordY);
				break;
		}
	}

	void BuildingCoordinates(string[] buildingDataRaw)
	{
		buildingData = new Vector2[numOfBuildings, 4];
		int k = 0;
		for (int i = 0; i < numOfBuildings; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				string[] temp = buildingDataRaw[k].Split(',');
				k++;
				float tempX = Convert.ToSingle(temp[0]);
				float tempY = Convert.ToSingle(temp[1]);
				buildingData[i, j] = new Vector2(tempX, tempY);
			}
		}
		SortBuildingData();
	}
	void SortBuildingData()
	{
		int n = numOfBuildings,
		i,
		j,
		flag;
		float val;
		Vector2[] valVec = new Vector2[4];
		for (i = 1; i < n; i++)
		{
			val = buildingData[i, 0].X;

			for (int k = 0; k < 4; k++)
			{
				valVec[k] = buildingData[i, k];
			}
			flag = 0;
			for (j = i - 1; j >= 0 && flag != 1;)
			{
				if (val < buildingData[j, 0].X)
				{
					for (int k = 0; k < 4; k++)
						buildingData[j + 1, k] = buildingData[j, k];
					j--;
					for (int k = 0; k < 4; k++)
						buildingData[j + 1, k] = valVec[k];
				}
				else flag = 1;
			}
		}
	}

	int Bisection()
	{
		for (int i = 0; i < numOfBuildings; i++)
		{
			if (sunCoordinates.X < buildingData[i, 0].X)
			{
				if (sunCoordinates.Y >= buildingData[i, 1].Y)
				{
					sunRightmost = false;
					onTop = false;
					return i;
				}
				else
				{
					return -1;
				}
			}
			else if (sunCoordinates.X >= buildingData[i, 0].X && sunCoordinates.X <= buildingData[i, 3].X)
			{
				if (sunCoordinates.Y > buildingData[i, 0].Y)
				{
					sunRightmost = false;
					onTop = true;
					return i;
				}
				else
				{
					return -1;
				}
			}
		}
		if (sunCoordinates.Y > buildingData[0, 1].Y)
		{
			onTop = false;
			sunRightmost = true;
			return numOfBuildings - 1;
		}
		else
		{
			return -1;
		}
	}

	float FindSlope(float x1, float x2, float y1, float y2)
	{
		return (y2 - y1) / (x2 - x1);
	}

	float FindDistance(Vector2 V1, Vector2 V2)
	{
		return Convert.ToSingle(Math.Sqrt(Math.Pow(Convert.ToDouble(V2.Y - V1.Y), 2) + (Math.Pow(Convert.ToDouble(V2.X - V1.X), 2))));
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
	float CalculateSurface()
	{
		float surfaceValue = 0f;
		float m1 = 0f,
		m2 = 0f,
		m3 = 0f,
		m4 = 0f; ;
		float c;
		Vector2 partialPoint;
		Vector2 sCor = sunCoordinates;
		bool partial = false;
		bool sunLow;

		if (onTop)
		{

			surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);
		}
		if (bisectionPoint != 0 || sunRightmost) // START OF LEFT <<<<<<<<<<<<<<------------------------------------------------------------------------------------------
		{
			sunLow = false;

			float maxSlope = float.NegativeInfinity;
			float minSlope = float.NegativeInfinity;
			float maxheight = float.NegativeInfinity;

			bool previousFlag = false;
			int maxSlopeIndex = bisectionPoint - 1;

			if (onTop)
			{
				m1 = FindSlope(sCor.X, buildingData[bisectionPoint, 0].X, sCor.Y, buildingData[bisectionPoint, 0].Y);
				minSlope = m1;
				maxSlopeIndex = bisectionPoint;
				maxheight = buildingData[bisectionPoint, 3].Y;
			}
			else if (bisectionPoint > 0 && !sunRightmost)
			{
				if (!onTop && sCor.Y > buildingData[bisectionPoint - 1, 0].Y) //Sun is above the next building//////
				{
					surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 0]);
					surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);
					m1 = FindSlope(sCor.X, buildingData[bisectionPoint - 1, 0].X, sCor.Y, buildingData[bisectionPoint - 1, 0].Y);
					minSlope = m1;
					maxSlopeIndex = bisectionPoint - 1;
					maxheight = buildingData[bisectionPoint - 1, 3].Y;

				}
				else if (!onTop && sCor.Y <= buildingData[bisectionPoint - 1, 0].Y) // Sun is below the next building
				{
					surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);

					m3 = FindSlope(sCor.X, buildingData[bisectionPoint - 1, 3].X, sCor.Y, buildingData[bisectionPoint - 1, 3].Y);
					maxSlope = m3;
					maxSlopeIndex = bisectionPoint - 1;
					previousFlag = true;
					sunLow = true;
					maxheight = buildingData[bisectionPoint - 1, 3].Y;
				}

			}
			else if (sunRightmost)
			{
				if (!onTop && sCor.Y > buildingData[bisectionPoint, 0].Y) //Sun is above the next building//////
				{

					surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);

					surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 2]);

					m1 = FindSlope(sCor.X, buildingData[bisectionPoint, 0].X, sCor.Y, buildingData[bisectionPoint, 0].Y);
					minSlope = m1;
					maxSlopeIndex = bisectionPoint;
					maxheight = buildingData[bisectionPoint, 3].Y;

				}
				else if (!onTop && sCor.Y <= buildingData[bisectionPoint, 0].Y) // Sun is below the next building
				{

					surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 2]);

					m3 = FindSlope(sCor.X, buildingData[bisectionPoint, 3].X, sCor.Y, buildingData[bisectionPoint, 3].Y);
					maxSlope = m3;
					maxSlopeIndex = bisectionPoint;
					previousFlag = true;
					sunLow = true;
					maxheight = buildingData[bisectionPoint, 3].Y;
				}
			}

			for (int i = (onTop || sunRightmost) ? bisectionPoint - 1 : bisectionPoint - 2; i >= 0; i--)
			{

				m3 = FindSlope(sCor.X, buildingData[i + 1, 3].X, sCor.Y, buildingData[i + 1, 3].Y); // slope of next building
				if (m3 < maxSlope)
				{
					maxSlope = m3;
					maxSlopeIndex = i + 1;
				}
				m4 = FindSlope(sCor.X, buildingData[i + 1, 0].X, sCor.Y, buildingData[i + 1, 0].Y);
				if (m4 < minSlope)
				{
					minSlope = m4;
				}
				if (maxheight < buildingData[i + 1, 3].Y)
				{
					maxheight = buildingData[i + 1, 3].Y;
				}

				if (buildingData[i, 0].Y < sCor.Y && sunLow == false) // next  building is shorter than the sun's height
				{

					for (int j = 2; j != 1; j = (j + 1) % 4)
					{
						m2 = FindSlope(sCor.X, buildingData[i, j].X, sCor.Y, buildingData[i, j].Y);

						if (j == 2) //for 2,3 |
						{

							if (m2 <= minSlope) //if slope of vertex 2 of building is less than minslope, then that vertex along with the line segment can be lit
							{

								surfaceValue += FindDistance(buildingData[i, 2], buildingData[i, 3]);

								partial = false;

							}
							else
							{
								partial = true;

							}

						}
						else if (j == 3) // for 3,0 -
						{
							if (m2 <= minSlope && partial == true)
							{
								m3 = FindSlope(sCor.X, buildingData[i + 1, 0].X, sCor.Y, buildingData[i + 1, 0].Y);
								c = FindYIntercept(sCor.X, sCor.Y, minSlope);
								partialPoint.X = buildingData[i, 3].X;
								partialPoint.Y = FindY(buildingData[i, 3].X, minSlope, c);

								surfaceValue += FindDistance(buildingData[i, 3], partialPoint);

								surfaceValue += FindDistance(buildingData[i, 3], buildingData[i, 0]);

								partial = false;
							}
							else if (m2 <= minSlope && partial == false)
							{
								surfaceValue += FindDistance(buildingData[i, 3], buildingData[i, 0]);

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
								m3 = FindSlope(sCor.X, buildingData[i + 1, 0].X, sCor.Y, buildingData[i + 1, 0].Y);
								c = FindYIntercept(sCor.X, sCor.Y, minSlope);
								partialPoint.Y = buildingData[i, 0].Y;
								partialPoint.X = FindX(buildingData[i, 0].Y, minSlope, c);

								surfaceValue += FindDistance(buildingData[i, 0], partialPoint);

								partial = false;
							}
							else
							{
								partial = false;
							}
						}
					}
				}
				else if (buildingData[i, 0].Y >= sCor.Y || sunLow == true) // next building is taller than the sun's height
				{
					sunLow = true;
					if (buildingData[i, 0].Y <= maxheight) //building is shorter than max height
					{
						continue;
					}
					else
					{
						if (buildingData[maxSlopeIndex, 0].Y > sCor.Y) //> sCor.Y) // previous building is taller than sun
						{
							m3 = FindSlope(sCor.X, buildingData[i + 1, 3].X, sCor.Y, buildingData[i + 1, 3].Y);
							m2 = FindSlope(sCor.X, buildingData[i, 3].X, sCor.Y, buildingData[i, 3].Y);

							if (m2 > maxSlope) continue;
							c = FindYIntercept(sCor.X, sCor.Y, maxSlope);
							partialPoint.X = buildingData[i, 3].X;
							partialPoint.Y = FindY(buildingData[i, 3].X, maxSlope, c);
							surfaceValue += FindDistance(buildingData[i, 3], partialPoint);

							previousFlag = true;
						}
						else if (!previousFlag)
						{ // previous building is shorter than sun
							m2 = FindSlope(sCor.X, buildingData[i, 2].X, sCor.Y, buildingData[i, 2].Y);
							m3 = FindSlope(sCor.X, buildingData[i + 1, 0].X, sCor.Y, buildingData[i + 1, 0].Y);
							if (m2 <= minSlope)
							{

								surfaceValue += FindDistance(buildingData[i, 3], buildingData[i, 2]);

							}
							else
							{

								c = FindYIntercept(sCor.X, sCor.Y, minSlope);
								partialPoint.X = buildingData[i, 3].X;
								partialPoint.Y = FindY(buildingData[i, 3].X, minSlope, c);
								surfaceValue += FindDistance(buildingData[i, 3], partialPoint);

							}
						}
					}
				}

				m1 = FindSlope(sCor.X, buildingData[i, 0].X, sCor.Y, buildingData[i, 0].Y);

			}

		} // END OF LEFT <<<<<<<<<<<<<<------------------------------------------------------------------------------------------
		if ((bisectionPoint < numOfBuildings - 1 || !onTop) && !sunRightmost) //START for right bisection ----------------------------------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>
		{
			sunLow = false;

			float maxSlope = float.NegativeInfinity;
			float minSlope = float.NegativeInfinity;
			float maxheight = float.NegativeInfinity;

			bool previousFlag = false;
			int maxSlopeIndex = bisectionPoint;

			if (onTop)
			{

				m1 = FindSlope(sCor.X, buildingData[bisectionPoint, 3].X, sCor.Y, buildingData[bisectionPoint, 3].Y);
				minSlope = m1;
				maxheight = buildingData[bisectionPoint, 3].Y;
				maxSlopeIndex = bisectionPoint;
			}
			else if (!onTop && sCor.Y > buildingData[bisectionPoint, 0].Y) //sun above
			{
				surfaceValue += FindDistance(buildingData[bisectionPoint, 3], buildingData[bisectionPoint, 0]);

				surfaceValue += FindDistance(buildingData[bisectionPoint, 0], buildingData[bisectionPoint, 1]);

				m1 = FindSlope(sCor.X, buildingData[bisectionPoint, 3].X, sCor.Y, buildingData[bisectionPoint, 3].Y);
				minSlope = m1;
				maxSlopeIndex = bisectionPoint - 1;
				maxheight = buildingData[bisectionPoint, 3].Y;
			}
			else if (!onTop && sCor.Y <= buildingData[bisectionPoint, 0].Y) //sun below
			{
				surfaceValue += FindDistance(buildingData[bisectionPoint, 0], buildingData[bisectionPoint, 1]);

				m3 = FindSlope(sCor.X, buildingData[bisectionPoint, 0].X, sCor.Y, buildingData[bisectionPoint, 0].Y);
				maxSlope = m3;
				maxSlopeIndex = bisectionPoint;
				previousFlag = true;
				sunLow = true;
				maxheight = buildingData[bisectionPoint, 3].Y;
			}

			for (int i = bisectionPoint + 1; i < numOfBuildings; i++)
			{

				m3 = FindSlope(sCor.X, buildingData[i - 1, 0].X, sCor.Y, buildingData[i - 1, 0].Y); // slope of next building
				if (m3 > maxSlope)
				{
					maxSlope = m3;
					maxSlopeIndex = i - 1;
				}
				m4 = FindSlope(sCor.X, buildingData[i - 1, 3].X, sCor.Y, buildingData[i - 1, 3].Y);
				if (m4 > minSlope)
				{
					minSlope = m4;
				}
				if (maxheight < buildingData[i - 1, 3].Y)
				{
					maxheight = buildingData[i - 1, 3].Y;
				}

				if (buildingData[i, 0].Y < sCor.Y && sunLow == false) // next  building is shorter than the sun's height
				{

					for (int j = 1; j != 2; j = (j != 0) ? (j - 1) % 4 : (j + 3)) // for j to loop as 1,0,3 and terminate at 2
					{
						m2 = FindSlope(sCor.X, buildingData[i, j].X, sCor.Y, buildingData[i, j].Y);

						if (j == 1) // for 1,0 |
						{
							if (m2 >= minSlope) //if slope of vertex 1 of building is less than slope of 3th vertex of current building, then that vertex along with the line segment can be lit
							{
								surfaceValue += FindDistance(buildingData[i, 1], buildingData[i, 0]);

								partial = false;
							}
							else
							{
								partial = true;
							}

						}
						else if (j == 0) // for 0,3 - 
						{
							if (m2 >= minSlope && partial == true)
							{

								c = FindYIntercept(sCor.X, sCor.Y, minSlope);
								partialPoint.X = buildingData[i, 0].X; // |||||
								partialPoint.Y = FindY(buildingData[i, 0].X, minSlope, c);
								surfaceValue += FindDistance(buildingData[i, 0], partialPoint);

								surfaceValue += FindDistance(buildingData[i, 0], buildingData[i, 3]);

								partial = false;
							}
							else if (m2 >= minSlope && partial == false)
							{
								surfaceValue += FindDistance(buildingData[i, 0], buildingData[i, 3]);

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

								c = FindYIntercept(sCor.X, sCor.Y, minSlope);
								partialPoint.Y = buildingData[i, 3].Y;
								partialPoint.X = FindX(buildingData[i, 3].Y, minSlope, c);
								surfaceValue += FindDistance(buildingData[i, 3], partialPoint);
								partial = false;
							}
							else
							{
								partial = false;
							}
						}
					}
				}
				else if (buildingData[i, 0].Y >= sCor.Y || sunLow == true) // building is taller than the sun's height
				{

					sunLow = true;
					if (buildingData[i, 0].Y <= maxheight) // building is shorter than previous building
					{

						continue;
					}
					else
					{
						if (buildingData[maxSlopeIndex, 0].Y > sCor.Y) // previous building is taller than sun ( case  I ) 
						{

							m2 = FindSlope(sCor.X, buildingData[i, 0].X, sCor.Y, buildingData[i, 0].Y); // slope of current building
							if (m2 < maxSlope) continue; //right
							c = FindYIntercept(sCor.X, sCor.Y, maxSlope);
							partialPoint.X = buildingData[i, 0].X;
							partialPoint.Y = FindY(partialPoint.X, maxSlope, c);
							surfaceValue += FindDistance(buildingData[i, 0], partialPoint);

							previousFlag = true;

						}
						else if (!previousFlag) // previous building is shorter than sun (Case II)
						{
							m2 = FindSlope(sCor.X, buildingData[i, 1].X, sCor.Y, buildingData[i, 1].Y);
							if (m2 >= minSlope)
							{
								surfaceValue += FindDistance(buildingData[i, 0], buildingData[i, 1]);

							}
							else
							{
								c = FindYIntercept(sCor.X, sCor.Y, minSlope);
								partialPoint.X = buildingData[i, 0].X;
								partialPoint.Y = FindY(partialPoint.X, minSlope, c);
								surfaceValue += FindDistance(buildingData[i, 0], partialPoint);

							}
						}
					}

					m1 = FindSlope(sCor.X, buildingData[i, 0].X, sCor.Y, buildingData[i, 0].Y);
				}

			}

		} //END for right bisection ----------------------------------------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>>>>// 
		return surfaceValue;
	}
}
