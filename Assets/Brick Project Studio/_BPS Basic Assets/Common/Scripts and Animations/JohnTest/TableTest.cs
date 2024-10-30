using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TableTest : MonoBehaviour
{

    public KeyCode screenshotKey = KeyCode.Space;
    private int screenshotCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            CaptureScreenshot();
        }
    }

    void CaptureScreenshot()
    {
        screenshotCount++;
        string filename = $"Screenshot_{screenshotCount}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string path = Path.Combine(Application.persistentDataPath, filename);

        ScreenCapture.CaptureScreenshot(path);
        Debug.Log($"Screenshot saved: {path}");
    }

}
