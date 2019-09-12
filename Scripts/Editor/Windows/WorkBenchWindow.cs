using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WorkBenchWindow : EndlessWindow
{
    

    [MenuItem("Tools/wwf/WorkBench")]
    private static void OpenWindow()
    {
        EndlessWindow window = GetWindow<WorkBenchWindow>();
        window.titleContent = new GUIContent("WorkBench");
    }

    public override void OnDragPerform(Event e)
    {
        if (DragAndDrop.objectReferences.Length > 0)
        {
            foreach (Object obj in DragAndDrop.objectReferences)
            {
                if (obj is GameObject)
                {
                    GameObject go = obj as GameObject;
                    if (go.scene.name != null)
                        continue;
                }

                items.Add(new EndlessWindowItem(obj, ConvertScreenCoordsToZoomCoords(e.mousePosition)));

                Debug.Log("DragPerform " + e.mousePosition);
                Debug.Log(obj.name + " " + obj.GetType() + " " + obj);
                Debug.Log(EditorGUIUtility.ObjectContent(obj, obj.GetType()).image);

            }
        }
    }
}
