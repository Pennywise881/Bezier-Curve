using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticBezierCurve : MonoBehaviour
{
	[SerializeField]
	LineRenderer lineRenderer;

	[SerializeField]
	public Transform point0, point1, point2;

	int numPoints;
	Vector3[] positions;

	private void Start()
	{
		numPoints = 50;
		positions = new Vector3[numPoints];
		lineRenderer.positionCount = numPoints;
		//drawLinearCurve();
		//drawQuadraticCurve();
	}

	private void Update()
	{
		drawQuadraticCurve();
	}

	//private void drawLinearCurve()
	//{
	//	for (int i = 0; i < numPoints; i++)
	//	{
	//		float t = i / (float)numPoints;
	//		positions[i] = calculateLinearBezierPoint(t, point0.position, point1.position);
	//	}
	//	lineRenderer.SetPositions(positions);
	//}

	private void drawQuadraticCurve()
	{
		for (int i = 0; i < numPoints; i++)
		{
			float t = i / (float)numPoints;
			positions[i] = calculateQuadraticBezierPoint(t, point0.position, point1.position, point2.position);
		}
		lineRenderer.SetPositions(positions);
	}

	//private Vector3 calculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
	//{
	//	return p0 + t * (p1 - p0);
	//}

	private Vector3 calculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		// (1-t)2 P0 + 2(1-t)tP1 + t2P2

		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;

		Vector3 p = uu * p0;
		p += 2 * u * t * p1;
		p += tt * p2;

		return p;
	}
}
