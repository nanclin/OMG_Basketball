using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScreenCaptureController : MonoBehaviour {

    public int ResolutionFactor = 5;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".png", ResolutionFactor);
            UnityEditor.AssetDatabase.Refresh();
        }
    }
}
