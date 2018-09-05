using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
	public LineRenderer lineRenderer;
	CurveDisplayer displayer;

	Vector3[] pointsOnTheLine;

	public static BezierCurve _instance;

	private void Awake()
	{
		_instance = this;
		displayer = GetComponent<CurveDisplayer>();
	}

	private void Start()
	{
		drawBezierCurve();
	}

	void drawBezierCurve()
	{
		//Each segment in the curve will be represented by 100 points on the line renderer to create a smoothe curve.
		int numOfPts = 100;
		pointsOnTheLine = new Vector3[numOfPts];

		//Calculate the number of segments on the curve.
		int numberOfSegments = (displayer.points.Count / 3);

		//Set the size of the line renderer positions array.
		lineRenderer.positionCount = 100 * numberOfSegments;


		int anchorPt = 0;
		int lineIndex = 0;

		//For every segment on the curve, create 100 points for the line rendered.
		//Use the anchor points as starting and ending points to create a segment using the line renderer.
		for (int i = 0; i < numberOfSegments; i++)
		{
			for (int j = 0; j < 100; j++)
			{
				float t = j / (float)numOfPts;
				pointsOnTheLine[j] = calculateCubicBezierPoint(t, displayer.points[anchorPt], displayer.points[anchorPt + 1], displayer.points[anchorPt + 2], displayer.points[anchorPt + 3]);
				lineRenderer.SetPosition(j + lineIndex, pointsOnTheLine[j]);
			}
			anchorPt += 3;
			lineIndex += 100;
		}
	}

	private Vector3 calculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		//Equation for calculating a cubic bezier curve.
		//This returns 1 point on the line renderer for every 4 points on the curve.
		return Mathf.Pow((1 - t), 3) * p0 + 3 * Mathf.Pow((1 - t), 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
	}
}
