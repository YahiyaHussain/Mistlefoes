using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArcherBase))]
public class ArcherEditor : Editor {
    private void OnSceneGUI()
    {
        ArcherBase b = (ArcherBase)target;
        Handles.color = Color.red;
        //Handles.DrawWireDisc(b.transform.position, Vector3.forward, b.range);
        Handles.DrawWireDisc(b.transform.position, Vector3.forward, b.range);
    }
}
