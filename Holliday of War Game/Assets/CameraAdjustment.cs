using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustment : MonoBehaviour {
    [Tooltip("Determines the degree to which sprites are scaled. Integer scaling only.")]
    public int ppuScale;
    [Tooltip("PPU for all sprites in the current scene.")]
    public float globalPPU;

     /// <summary>
     /// Current width of the game screen, in pixels.
     /// </summary>
    private int curWidth;

    /// <summary>
    /// Current height of the game screen, in pixels.
    /// </summary>
    private int curHeight;

    /// <summary>
    /// Reference to the current camera, because Unity is being stupid.  Might change this to a public attribute later.
    /// </summary>
    private Camera curCam = null;

	// Use this for initialization
	void Start () {
        // get reference to main object
        GameObject temp = GameObject.Find("Main Camera");

        // check if reference is null, if so, handle it properly
        if (temp == null)
        {
            Debug.Log("Current camera is null. Make sure you have an object named \"Main Camera\" in the scene.");
            return;
        } else
        {
            curCam = temp.GetComponent<Camera>();
        }

        // update curWidth and curHeight
        curWidth = curCam.pixelWidth;
        curHeight = curCam.pixelHeight;

        // set orthographic size of the camera
        curCam.orthographicSize = (curHeight / (ppuScale * globalPPU)) / 2f;
	}
	
	// Update is called once per frame
	void Update () {
        // in case our reference was null to begin with
        if (curCam == null)
            return;

		// detect if resolution has changed, and update orthographic size accordingly
        if (curWidth != curCam.pixelWidth &&
            curHeight != curCam.pixelHeight)
        {
            curWidth = curCam.pixelWidth;
            curHeight = curCam.pixelHeight;

            Camera.current.orthographicSize = (curHeight / (ppuScale / globalPPU)) / 2f;
        }
	}
}
