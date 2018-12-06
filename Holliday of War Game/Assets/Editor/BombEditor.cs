using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DeployBomb))]
public class BombEditor : Editor {

    // Use this for initialization
    private void OnSceneGUI()
    {
        DeployBomb d = (DeployBomb)target;
        Handles.color = Color.red;
        Handles.DrawWireCube(d.transform.position, d.bombRange *Vector3.one);
        
    }
}
