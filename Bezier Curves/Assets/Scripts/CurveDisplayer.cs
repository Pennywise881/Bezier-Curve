using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveDisplayer : MonoBehaviour
{
	[HideInInspector]
	public List<Vector3> points;

	[HideInInspector]
	public bool looped;

	[HideInInspector]
	public bool createdFirstSegment;

	public void initializePoints()
	{
		//Set up the first segment of the curve in the editor.
		points = new List<Vector3>()
		{
			new Vector3(-10,0,0),
			new Vector3(-5,0,5),
			new Vector3(5,0,-5),
			new Vector3(10,0,0)
		};

		createdFirstSegment = true;
	}

	private void OnDrawGizmos()
	{
		//Indicate 2 corresponding control points
		Gizmos.color = Color.white;
		for (int i = 0; i < points.Count; i++)
		{
			if (i == 0) Gizmos.DrawLine(points[0], points[1]);
			else if (i == points.Count - 1) Gizmos.DrawLine(points[points.Count - 1], points[points.Count - 2]);
			else if (i % 3 == 0)
			{
				Gizmos.DrawLine(points[i], points[i + 1]);
				Gizmos.DrawLine(points[i], points[i - 1]);
			}
		}

		//Indicate all the control/anchor points on the curve.
		for (int i = 0; i < points.Count; i++)
		{
			//Anchor point
			if (i % 3 == 0) Gizmos.color = Color.green;

			//Control point;
			else Gizmos.color = Color.red;

			Gizmos.DrawSphere(points[i], 0.5f);

			//If the curve is closed, indicate the starting/ending point on the curve.
			if (looped && i == points.Count - 1)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawSphere(points[i], 0.5f);
			}
		}
	}

	public void addPoints(Vector3 newAnchorPointPosition)
	{

		if (looped)
		{
			//If the curve is closed, add 3 new points in the place of the points that were used to loop the curve.
			//This created a new segment and removes the loop
			looped = false;
			points[points.Count - 3] = points[points.Count - 3 - 1] + (points[points.Count - 3 - 1] - points[points.Count - 3 - 2]);
			points[points.Count - 2] = (points[points.Count - 2 - 1] + newAnchorPointPosition) / 2;
			points[points.Count - 1] = newAnchorPointPosition;
			return;
		}

		//Add first new control point
		points.Add(points[points.Count - 1] + (points[points.Count - 1] - points[points.Count - 2]));

		//Add the second new control point
		points.Add((points[points.Count - 1] + newAnchorPointPosition) / 2);

		//Add the new anchor point
		points.Add(newAnchorPointPosition);
	}

	public void movePoints(int index, Vector3 movePosition)
	{
		float dist;
		Vector3 dir;
		Vector3 prevPointPosition = points[index];
		points[index] = movePosition;

		if (index % 3 == 0)
		{
			//Used for moving anchor points
			if (looped && (index == 0 || index == points.Count - 1))
			{
				//Moving the last anchor point moves the anchor first point and vice versa
				points[0] = points[points.Count - 1];
				points[1] += movePosition - prevPointPosition;
				points[points.Count - 2] += movePosition - prevPointPosition;
			}
			else
			{
				if (index + 1 < points.Count) points[index + 1] += movePosition - prevPointPosition;
				if (index - 1 >= 0) points[index - 1] += movePosition - prevPointPosition;
			}
		}
		else
		{
			//Used for moving control points
			if (looped && (index == 1 || index == points.Count - 2))
			{
				//The first and last control points act like corresponding anchor points to each other.
				dist = (points[0] - points[index]).magnitude;
				dir = (points[0] - points[index]).normalized;
				if (index == 1) points[points.Count - 2] = points[0] + (dir * dist);
				else points[1] = points[0] + (dir * dist);
			}
			else
			{
				if ((index + 1) % 3 == 0 && index + 2 < points.Count - 1)
				{
					dist = (points[index] - points[index + 1]).magnitude;
					dir = (points[index + 1] - points[index]).normalized;
					points[index + 2] = points[index + 1] + (dir * dist);
				}
				else if ((index - 1) % 3 == 0 && index - 2 > 0)
				{
					dist = (points[index] - points[index - 1]).magnitude;
					dir = (points[index - 1] - points[index]).normalized;
					points[index - 2] = points[index - 1] + (dir * dist);
				}
			}
		}
	}

	//This function is used to close the curve and create a loop
	public void createLoop()
	{
		//If the curve is already looped and the user presses 'enter' return.
		if (looped) return;
		looped = true;


		float dist = (points[points.Count - 1] - points[points.Count - 2]).magnitude;
		Vector3 dir = (points[points.Count - 1] - points[points.Count - 2]).normalized;

		//1st point: Control point which is set in the opposite direction of the current last control point and acts becomes it's corresponding control point
		points.Add(points[points.Count - 1] + (dir * dist));

		dist = (points[0] - points[1]).magnitude;
		dir = (points[0] - points[1]).normalized;

		//2nd point: Control point which acts as the corresponding control point to the first control point set in it's opposite direction.
		points.Add(points[0] + (dir * dist));


		//3rd point: Last anchor point on the curve, it position is set in the place of the first anchor point to create a closed curve/loop.
		points.Add(points[0]);
	}
}
