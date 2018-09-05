using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBezierCurve : MonoBehaviour
{
	[SerializeField]
	LineRenderer lineRenderer;

	Vector3[] positions;

	int numberOfPoints;

	[SerializeField]
	Transform point0, point1;

	private void Start()
	{
		numberOfPoints = 50;
		positions = new Vector3[50];
		initialize();
	}

	private void calculateInitialPoints()
	{
		for (int i = 0; i < 2; i++)
		{
			float t = i / (float)numberOfPoints;
			positions[i] = calculateBezierPoint(point0.position, point1.position, t);
		}
	}

	IEnumerator drawLinearCurve()
	{
		int i = 0;

		lineRenderer.SetPosition(i, positions[i]);
		lineRenderer.positionCount++;
		lineRenderer.SetPosition(++i, positions[i]);
		i++;

		while (i < numberOfPoints)
		{
			float t = i / (float)numberOfPoints;
			positions[i] = calculateBezierPoint(point0.position, point1.position, t);
			lineRenderer.positionCount++;
			lineRenderer.SetPosition(i, positions[i]);
			i++;
			yield return new WaitForEndOfFrame();
		}
	}

	private void initialize()
	{
		lineRenderer.positionCount = 1;
		calculateInitialPoints();
		StartCoroutine("drawLinearCurve");
	}

	private void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			StopCoroutine("drawLinearCurve");
			initialize();
		}
	}

	private Vector3 calculateBezierPoint(Vector3 p0, Vector3 p1, float t)
	{
		//formula for liner interpolation = p0 + t * (p1 - p0)
		return p0 + t * (p1 - p0);
	}
}
