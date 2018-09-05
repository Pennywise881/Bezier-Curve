using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curve_Rider : MonoBehaviour
{

	private void Start()
	{
		StartCoroutine(movePlayer());
	}

	IEnumerator movePlayer()
	{
		//Wait until the list of points have been saved
		yield return new WaitForSecondsRealtime(2);

		//Get the number of points in the line renderer
		int numPointsOnCurve = BezierCurve._instance.lineRenderer.positionCount;
		int i = 0;

		while (i % numPointsOnCurve < numPointsOnCurve)
		{
			Vector3 pointOnLine = BezierCurve._instance.lineRenderer.GetPosition(i % numPointsOnCurve);
			//Set the rotation of the rider to that it looks like its facing where it is going.
			transform.LookAt(pointOnLine);
			transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

			//Set the position of the rider = to the point on the line rendered to move it along the curve.
			transform.position = new Vector3(pointOnLine.x, pointOnLine.y + 1, pointOnLine.z);
			i++;
			yield return new WaitForEndOfFrame();
		}
	}
}
