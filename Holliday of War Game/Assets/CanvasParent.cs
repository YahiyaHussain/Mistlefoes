using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasParent : MonoBehaviour {

    // Use this for initialization

    Transform BasePool;
	void Awake () {
        BasePool = GameObject.FindGameObjectWithTag("BasePool").transform;

        //assemble all bases and all canvases and match a canvas to every base
        Stack<Transform> BaseList = new Stack<Transform>();
        foreach (Transform t in BasePool)
        {
            BaseList.Push(t);
        }

        Stack<Transform> CanvasList = new Stack<Transform>();
        foreach (Transform t in transform)
        {
            CanvasList.Push(t);
        }

        Transform tempCanvas;
        Transform tempBase;
        while (BaseList.Count > 0)
        {
            //very satisfying data type
            tempCanvas = CanvasList.Pop();
            tempBase = BaseList.Pop();

            //put canvas onto the base
            tempCanvas.position = tempBase.position;
            tempCanvas.gameObject.SetActive(true);

            //tell the base which canvas it has been assigned
            if (tempBase.gameObject.GetComponent<ArcherBase>() != null)
            {
                tempBase.gameObject.GetComponent<ArcherBase>().myCanvas = tempCanvas;
                continue;
            }
            tempBase.gameObject.GetComponent<Base>().myCanvas = tempCanvas;
        }


	}


	
}
