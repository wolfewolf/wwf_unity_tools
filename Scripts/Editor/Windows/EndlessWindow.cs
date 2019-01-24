using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EndlessWindow : EditorWindow
{

    public float _zoom = 1f;
    private const float kZoomMin = 0.1f;
    private const float kZoomMax = 10.0f;
    public Vector2 _zoomCoordsOrigin = Vector2.zero;
    Rect _zoomArea;

    private Vector2 lastMouse = Vector2.zero;
    private Vector2 newMouse = Vector2.zero;

    private static void OpenWindow()
    {
        EndlessWindow window = GetWindow<EndlessWindow>();
        window.titleContent = new GUIContent("EndlessWindow");
    }

    private void OnGUI()
    {
        _zoomArea = new Rect(0.0f, 0, Screen.width, Screen.height);
        
        EditorZoomArea.Begin(_zoom, _zoomArea);
        {

            GUI.Box(new Rect(0.0f + _zoomCoordsOrigin.x, 0.0f + _zoomCoordsOrigin.y, 100.0f, 25.0f), "Zoomed Box");

            // You can also use GUILayout inside the zoomed area.
            GUILayout.BeginArea(new Rect(300.0f + _zoomCoordsOrigin.x, 70.0f + _zoomCoordsOrigin.y, 130.0f, 50.0f));
            GUILayout.Button("Zoomed Button 1");
            GUILayout.Button("Zoomed Button 2");
            GUILayout.EndArea();

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            ProcessEvents(Event.current);
        }

        EditorZoomArea.End();
        if (GUI.changed) Repaint();
    }

    private void DrawDebug()
    {
        Handles.color = Color.red;
        //Debug.Log(lastMouse);
        Handles.DrawLine(lastMouse, newMouse);

        //lastMouse
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(Screen.width / _zoom / gridSpacing);
        int heightDivs = Mathf.CeilToInt(Screen.height / _zoom / gridSpacing);

        Handles.BeginGUI();
        {
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
            Vector2 offset = new Vector2(_zoomCoordsOrigin.x % gridSpacing, _zoomCoordsOrigin.y % gridSpacing);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(
                    (new Vector2(gridSpacing * i, -gridSpacing) + offset),
                    (new Vector2(gridSpacing * i, Screen.height / _zoom + gridSpacing) + offset)
                    );
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(
                    (new Vector2(-gridSpacing, gridSpacing * j) + offset),
                    (new Vector2(Screen.width / _zoom + gridSpacing, gridSpacing * j) + offset)
                    );
            }

            Handles.color = Color.white;
        }
        Handles.EndGUI();
    }

    private void ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    //ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    //ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if ((Event.current.button == 0 && Event.current.modifiers == EventModifiers.Alt) || Event.current.button == 2)
                {
                    Vector2 delta2 = Event.current.delta;
                    _zoomCoordsOrigin += delta2;

                    Event.current.Use();
                }
                break;

            case EventType.ScrollWheel:
                Vector2 screenCoordsMousePos = Event.current.mousePosition;
                Vector2 delta = Event.current.delta;
                Vector2 zoomCoordsMousePos = screenCoordsMousePos;
                float zoomDelta = -delta.y / 50f;
                float oldZoom = _zoom;
                _zoom += zoomDelta;
                _zoom = Mathf.Clamp(_zoom, kZoomMin, kZoomMax);
                _zoomCoordsOrigin -= zoomCoordsMousePos - (oldZoom / _zoom) * zoomCoordsMousePos;
                Event.current.Use();
                break;
        }
    }

    private Vector2 ConvertScreenCoordsToZoomCoords(Vector2 screenCoords)
    {
        return (screenCoords - _zoomArea.TopLeft()) / _zoom - _zoomCoordsOrigin;
    }
}