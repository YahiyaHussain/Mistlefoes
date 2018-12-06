using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TurkeyAI))]
public class TukeyEditor : ArcherEditor {

    private void OnSceneGUI()
    {
        TurkeyAI t = (TurkeyAI)target;
        Handles.color = Color.red;
        Handles.DrawWireDisc(t.transform.position, Vector3.forward, t.ShootRange);
    }
}
