using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathBuilder))]
public class PathEditor : Editor
{

    private PathBuilder builder;
    private Path path;

    private void OnEnable()
    {
        builder = (PathBuilder) target;

        if (builder.path == null)
            builder.CreatePath();

        path = builder.path;
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    private void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type != EventType.MouseDown || guiEvent.button != 0 || !guiEvent.shift)
            return;

        Undo.RecordObject(builder, "Add Segment");
        path.AddSegment(mousePosition);
    }

    private void Draw()
    {
        Handles.color = Color.black;
        for(int i = 0; i < path.SegmentCount; i++)
        {
            Vector2[] points = path.GetPointsInSegment(i);
            Handles.DrawLine(points[0], points[1]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2f);
        }

        Handles.color = Color.red;
        for(int i = 0; i < path.PointCount; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(path[i], 0.1f, Vector3.zero, Handles.CylinderHandleCap);
            if (path[i] == newPos)
                continue;

            Undo.RecordObject(builder, "Move Point");
            path.MovePoint(i, newPos);
        }
    }

}
