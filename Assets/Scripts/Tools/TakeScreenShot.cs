using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenShot : MonoBehaviour
{
    public string path;
    public string imageName;
    public bool takePicture;

    // Update is called once per frame
    void Update()
    {
        if (!takePicture) return;
        takePicture = false;

        ScreenCapture.CaptureScreenshot(path + "\\" + imageName + ".png");
    }
}
