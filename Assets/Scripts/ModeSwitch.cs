using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitch : MonoBehaviour
{
     //if (bisectionPoint != 0) // Distance for left bisection
     //       {
     //           sunLow = false;
     //           //m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 0].x, sCor.y, buildingData[bisectionPoint, 0].y); ///try NOW
     //           float maxSlope = -Mathf.Infinity;
     //           float minSlope = -Mathf.Infinity;
     //           float maxheight = -Mathf.Infinity;
     //           bool previousFlag = false;
     //           int maxSlopeIndex =  bisectionPoint - 1 ;
     //           int minSlopeIndex= bisectionPoint - 1;
     //           if (onTop)
     //               {
     //                   m1 = FindSlope(sCor.x, buildingData[bisectionPoint, 0].x, sCor.y, buildingData[bisectionPoint, 0].y);
     //                   minSlope = m1;
     //                   minSlopeIndex = bisectionPoint;
     //                   maxSlopeIndex = bisectionPoint;     
     //                   maxheight = buildingData[bisectionPoint-1, 3].y;
     //               }
     //           else if (bisectionPoint > 0)
     //           {
     //               if (!onTop && sCor.y > buildingData[bisectionPoint - 1, 0].y) //Sun is above the next building//////
     //               {
     //                   surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3].x, buildingData[bisectionPoint - 1, 0].x, buildingData[bisectionPoint - 1, 3].y, buildingData[bisectionPoint - 1, 0].y);
     //                   DrawLine(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 0]);
     //                   surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3].x, buildingData[bisectionPoint - 1, 2].x, buildingData[bisectionPoint - 1, 3].y, buildingData[bisectionPoint - 1, 2].y);
     //                   DrawLine(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);
     //                   //if (bisectionPoint < numOfBuildings) //TryNOW
     //                   m1 = FindSlope(sCor.x, buildingData[bisectionPoint - 1, 0].x, sCor.y, buildingData[bisectionPoint - 1, 0].y);
     //                   minSlope = m1;
     //                   maxSlopeIndex = bisectionPoint - 1; 
     //                   minSlopeIndex = bisectionPoint - 1;
     //                   maxheight = buildingData[bisectionPoint-1, 3].y;
     //                   //else
     //                   //  m1 = FindSlope(sCor.x, buildingData[bisectionPoint - 2, 0].x, sCor.y, buildingData[bisectionPoint - 2, 0].y);///////
     //               }
     //               else if (!onTop && sCor.y <= buildingData[bisectionPoint - 1, 0].y) // Sun is below the next building
     //               {
     //                   surfaceValue += FindDistance(buildingData[bisectionPoint - 1, 3].x, buildingData[bisectionPoint - 1, 2].x, buildingData[bisectionPoint - 1, 3].y, buildingData[bisectionPoint - 1, 2].y);
     //                   DrawLine(buildingData[bisectionPoint - 1, 3], buildingData[bisectionPoint - 1, 2]);
     //                   m3 = FindSlope(sCor.x, buildingData[bisectionPoint - 1, 3].x, sCor.y, buildingData[bisectionPoint - 1, 3].y);
     //                   maxSlope = m3;
     //                   maxSlopeIndex = bisectionPoint - 1;
     //                   previousFlag = true;
     //                   sunLow = true;
     //                   maxheight = buildingData[bisectionPoint-1, 3].y;
     //               }
     //           }

     //           for (int i = onTop ? bisectionPoint - 1 : bisectionPoint - 2; i >= 0; i--)
     //           {
                    
     //               m3 = FindSlope(sCor.x, buildingData[i + 1, 3].x, sCor.y, buildingData[i + 1, 3].y);// slope of next building
     //               if (m3 < maxSlope)
     //               {
     //                   maxSlope = m3;
     //                   maxSlopeIndex = i + 1;
     //               }
     //               m4 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
     //               if (m4 < minSlope)
     //               {
     //                   minSlope = m4;
     //                   minSlopeIndex = i + 1;
     //               }
     //               if (maxheight < buildingData[i+1, 3].y)
     //               {
     //                   maxheight = buildingData[i+1, 3].y;
     //               }
     //               print(i + "st MAX HEIGHT"+maxheight);
     //              // Debug.Log(i + " M1: " + m1);
     //               if (buildingData[i, 0].y < sCor.y && sunLow == false)// next  building is shorter than the sun's height
     //               {

     //                   for (int j = 2; j != 1; j = (j + 1) % 4)
     //                   {
     //                       m2 = FindSlope(sCor.x, buildingData[i, j].x, sCor.y, buildingData[i, j].y);
     //                       //Debug.Log(i + " " + j + " M2: " + m2);
     //                       if (j == 2)//for 2,3 |
     //                       {
     //                            //Debug.Log(i + " " + j + " Minslope | M2 for j=2 " + minSlope +m2);
     //                           if (m2 <= minSlope) //if slope of vertex 2 of building is less than minslope, then that vertex along with the line segment can be lit
     //                           {   
                                    
     //                               surfaceValue += FindDistance(buildingData[i, 2].x, buildingData[i, 3].x, buildingData[i, 2].y, buildingData[i, 3].y);
     //                               DrawLine(buildingData[i, 2], buildingData[i, 3]);
     //                               //Debug.Log(i + " " + j + " 2,3|| " + m2);
     //                               partial = false;
     //                               //print(j + "Partial false");
     //                           }
     //                           else
     //                           {
     //                               partial = true;
     //                               //print(j + "Partial true");
     //                           }


     //                       }
     //                       else if (j == 3)// for 3,0 -
     //                       {
     //                           if (m2 <= minSlope && partial == true)
     //                           {
     //                               m3 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
     //                               c = FindYIntercept(sCor.x, sCor.y, minSlope);
     //                               partialPoint.x = buildingData[i, 3].x;
     //                               partialPoint.y = FindY(buildingData[i, 3].x,minSlope, c);
     //                               //Debug.Log("ParPoint: " + partialPoint);// ||||||||
     //                               surfaceValue += FindDistance(buildingData[i, 3].x, partialPoint.x, buildingData[i, 3].y, partialPoint.y);
     //                               DrawLine(buildingData[i, 3], partialPoint);
     //                               surfaceValue += FindDistance(buildingData[i, 3].x, buildingData[i, 0].x, buildingData[i, 3].y, buildingData[i, 0].y);
     //                               DrawLine(buildingData[i, 3], buildingData[i, 0]);
     //                               partial = false;
     //                           }
     //                           else if (m2 <= minSlope && partial == false)
     //                           {
     //                               surfaceValue += FindDistance(buildingData[i, 0].x, buildingData[i, 3].x, buildingData[i, 0].y, buildingData[i, 3].y);
     //                               DrawLine(buildingData[i, 3], buildingData[i, 0]);
     //                               //Debug.Log("YO ");// ||||||||
     //                           }
     //                           else if (m2 >= minSlope)
     //                           {
     //                               partial = true;
     //                           }
     //                       }
     //                       else if (j == 0)
     //                       {
     //                           if (m2 <= minSlope && partial == true)
     //                           {
     //                               m3 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
     //                               c = FindYIntercept(sCor.x, sCor.y, minSlope);
     //                               partialPoint.y = buildingData[i, 0].y;
     //                               partialPoint.x = FindX(buildingData[i, 0].y, minSlope, c);
     //                               //Debug.Log("ParPoint: " + partialPoint);// --------
     //                               surfaceValue += FindDistance(buildingData[i, 0].x, partialPoint.x, buildingData[i, 0].y, partialPoint.y);
     //                               DrawLine(buildingData[i, 0], partialPoint);
     //                               partial = false;
     //                           }
     //                           else
     //                           {
     //                               partial = false;
     //                           }
     //                       }
     //                   }
     //               }
     //               else if (buildingData[i, 0].y >= sCor.y || sunLow == true)// next building is taller than the sun's height
     //               {
     //                   sunLow = true;
     //               if (buildingData[i, 0].y <= maxheight)//building is shorter than max height
     //               {
     //                   print("------------------------------BUiLDING IGNORED"+i+" MAX HEIGHT-------------------");

     //                   continue;
     //               }
     //               else
     //               {
     //                   if (buildingData[maxSlopeIndex, 0].y > sCor.y)  //> sCor.y) // previous building is taller than sun
     //                   {
     //                       m3 = FindSlope(sCor.x, buildingData[i + 1, 3].x, sCor.y, buildingData[i + 1, 3].y);
     //                       m2 = FindSlope(sCor.x, buildingData[i, 3].x, sCor.y, buildingData[i, 3].y);
     //                       //print("TEST maxslope, m2 " + maxSlope + " " + m2);
     //                       if (m2 > maxSlope)
     //                           continue;
     //                       //print("HIYYYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAa");
     //                       c = FindYIntercept(sCor.x, sCor.y, maxSlope);
     //                       partialPoint.x = buildingData[i, 3].x;
     //                       partialPoint.y = FindY(buildingData[i, 3].x, maxSlope, c);
     //                       surfaceValue += FindDistance(buildingData[i, 3].x, partialPoint.x, buildingData[i, 3].y, partialPoint.y);
     //                       DrawLine(buildingData[i, 3], partialPoint);
     //                       previousFlag = true;
     //                   }
     //                   else if (!previousFlag)
     //                   {// previous building is shorter than sun
     //                       m2 = FindSlope(sCor.x, buildingData[i + 1, 1].x, sCor.y, buildingData[i + 1, 1].y); //CHECK TryNOW
     //                       if (m2 < minSlope)
     //                       {
     //                           surfaceValue += FindDistance(buildingData[i, 3].x, buildingData[i, 2].x, buildingData[i, 3].y, buildingData[i, 2].y);
     //                           DrawLine(buildingData[i, 3], buildingData[i, 2]);
     //                       }
     //                       else
     //                       {
     //                           m3 = FindSlope(sCor.x, buildingData[i + 1, 0].x, sCor.y, buildingData[i + 1, 0].y);
     //                           c = FindYIntercept(sCor.x, sCor.y, minSlope);
     //                           partialPoint.x = buildingData[i, 3].x;
     //                           partialPoint.y = FindY(buildingData[i, 3].x, minSlope, c);
     //                           surfaceValue += FindDistance(buildingData[i, 3].x, partialPoint.x, buildingData[i, 3].y, partialPoint.y);
     //                           DrawLine(buildingData[i, 3], partialPoint);
     //                       }
     //                   }
     //                   }
     //               }
                    
     //               m1 = FindSlope(sCor.x, buildingData[i, 0].x, sCor.y, buildingData[i, 0].y);
     //               //Debug.Log(i + " M1: " + m1);
     //           }

     //       }
}