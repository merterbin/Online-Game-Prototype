using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WsPlayer 
{
    public double[] Position = new double[3];

    public void setPosition(Vector3 pos)
    {
        Position[0] = pos.x;
        Position[1] = pos.y;
        Position[2] = pos.z;
    }

}
