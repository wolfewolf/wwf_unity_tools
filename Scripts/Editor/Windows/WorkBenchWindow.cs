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
        window.titleContent = new GUIContent("WorkBenchWindow");
    }


}
