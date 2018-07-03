using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class InGameScreenshot : MonoBehaviour
{

    // Capture frames as a screenshot sequence. Images are
    // stored as PNG files in a folder - these can be combined into
    // a movie using image utility software (eg, QuickTime Pro).
    // The folder to contain our screenshots.
    // If the folder exists we will append numbers to create an empty folder.
    string folder = @"/ScreenshotFolder";
    DirectoryInfo _screenFolder;
    [SerializeField] KeyCode keyCode = KeyCode.F12;
    //int frameRate = 25;

    void Start()
    {
        // Set the playback framerate (real time will not relate to game time after this).
        //Time.captureFramerate = frameRate;

        // Create the folder
        _screenFolder = Directory.CreateDirectory(new DirectoryInfo(Application.dataPath).Parent.FullName + folder);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(keyCode))
        {
            string formats = "yyyy-MM-dd-HH-mm-ss-ff";
            string shotName = string.Format("{0}/{1}.png", _screenFolder, DateTime.Now.ToString(formats));

            // Capture the screenshot to the specified file.
            ScreenCapture.CaptureScreenshot(shotName);

            Debug.Log("保存截图: " + name);
        }
#endif
    }
}
