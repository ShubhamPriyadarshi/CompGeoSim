using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAdjust : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector2(this.transform.position.x,(ConsoleInputs.BuildingData.buildingData[0,1].y - 25f));
    }
}
