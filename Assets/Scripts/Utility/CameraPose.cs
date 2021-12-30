using UnityEngine;
using System;

public class CameraPose : MonoBehaviour
{
	public float cameraTilt;
    public void placeCamera(Vector3[] points) {
        float fov = GetComponent<Camera>().fieldOfView;
		float bottomAngle = 90 - (fov/2) + cameraTilt;
		float topAngle    = 90 + (fov/2) + cameraTilt;
		float bottomSlope = (float)Math.Tan(bottomAngle * Math.PI/180);
		float topSlope    = (float)Math.Tan(topAngle    * Math.PI/180);
        float minBottomIntercept=0;
        float maxTopIntercept=0;
        float minX=0;
        float maxX=0;
        foreach (Vector3 point in points) {
            float bottomIntercept = point.z - point.y/bottomSlope;
            float topIntercept    = point.z - point.y/topSlope;
            if (minBottomIntercept == 0 || minBottomIntercept > bottomIntercept) minBottomIntercept = bottomIntercept;
            if (maxTopIntercept    == 0 || maxTopIntercept    < topIntercept)    maxTopIntercept    = topIntercept;
            if (minX == 0 || minX > point.x) minX = point.x;
            if (maxX == 0 || maxX < point.x) maxX = point.x;
        }
        float camX = (minX + maxX) / 2;
        float camZ = (bottomSlope * minBottomIntercept - topSlope * maxTopIntercept)/(bottomSlope - topSlope);
        float camY = (camZ - maxTopIntercept) * topSlope;
        transform.position = new Vector3(camX,camY,camZ);
        transform.rotation = Quaternion.Euler(90-cameraTilt,0,0);
    }
}