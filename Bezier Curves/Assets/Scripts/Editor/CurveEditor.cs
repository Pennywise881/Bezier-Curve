using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CurveDisplayer))]
public class CurveEditor : Editor
{
	CurveDisplayer displayer;
	int num;
	Event guiEvent;

	private void OnSceneGUI()
	{
		guiEvent = Event.current;

		drawBezierCurve();
		setPoints();
		movePoints();
		createLoop();
	}

	void createLoop()
	{
		//If the user presses enter close the curve and create a 'loop'
		if (guiEvent.type == EventType.KeyDown && guiEvent.keyCode == KeyCode.Return)
		{
			Undo.RecordObject(displayer, "Created Loop");
			displayer.createLoop();
		}
	}

	void movePoints()
	{
		//This is used to move the points on the curve with the mouse.
		for (int i = 0; i < displayer.points.Count; i++)
		{
			//Get the point on the scene view of the specific point on the curve (control or anchor point)
			Vector3 pointMovePosition = Handles.FreeMoveHandle(displayer.points[i], Quaternion.identity, 0, Vector3.zero, Handles.CylinderHandleCap);

			//Move the point on the curve to the new position
			if (displayer.points[i] != pointMovePosition)
			{
				Undo.RecordObject(displayer, "Points Moved");
				displayer.movePoints(i, pointMovePosition);
			}
		}
	}

	void setPoints()
	{
		//Use the equiations below to calculate where user clicked in the scene view. 
		//Upon clicking on the plane in the scene view, a new segement in the curve will be created

		if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1 && guiEvent.shift)
		{
			/**Equiation to find the mouse cursor position in the scene view.*/
			Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
			float drawPlaneHeight = 0;
			float dstToPlane = (drawPlaneHeight - ray.origin.y) / ray.direction.y;
			Vector3 mousePosition = ray.GetPoint(dstToPlane);
			/** */

			Undo.RecordObject(displayer, "New Segment Added");
			displayer.addPoints(mousePosition);
		}
	}

	void drawBezierCurve()
	{
		//Using unity helper function, the curve is created.
		//The first and the last points on each segment are anchor points
		//The anchor points are indexed in the Vector3 list in multiples of 3. e.g list[index % 3 == 0] contains anchor points.
		//The control/tangent points lie between the two anchor points of each segment.
		num = 0;
		for (int i = 0; i < displayer.points.Count - 1;)
		{
			Handles.DrawBezier(displayer.points[i], displayer.points[(num * 3) + 3], displayer.points[i + 1], displayer.points[i + 2], Color.yellow, null, 2);
			num++;
			i = num * 3;
		}
	}

	private void OnEnable()
	{
		//Initialize the script and set up the first segment in the curve.
		displayer = target as CurveDisplayer;
		if (!displayer.createdFirstSegment) displayer.initializePoints();
	}
}
