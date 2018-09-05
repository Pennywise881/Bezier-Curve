using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicBezierCurve : MonoBehaviour
{
	[SerializeField]
	LineRenderer lineRenderer;

	[SerializeField]
	public Transform point0, point1, point2, point3;

	int numPoints;
	Vector3[] positions;

	private void Start()
	{
		numPoints = 100;
		positions = new Vector3[numPoints];
		lineRenderer.positionCount = numPoints;
	}

	private void Update()
	{
		drawCubicCurve();

		//if (Input.GetKeyDown("space")) Dummy._instance.foo(positions);
	}

	private void drawCubicCurve()
	{
		for (int i = 0; i < numPoints; i++)
		{
			float t = i / (float)numPoints;
			positions[i] = calculateCubicBezierPoint(t, point0.position, point1.position, point2.position, point3.position);
		}
		lineRenderer.SetPositions(positions);
	}

	private Vector3 calculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p3, Vector3 p2)
	{
		return Mathf.Pow((1 - t), 3) * p0 + 3 * Mathf.Pow((1 - t), 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
	}
}
