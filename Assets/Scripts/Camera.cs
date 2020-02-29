using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Camera
{
    #region songleton
    private static Camera instance = null;

    private Camera()
    {
        init();
    }

    public static Camera getInstance()
    {
        if (instance == null)
        {
            instance = new Camera();
        }
        return instance;
    }
    #endregion

    private string cameraPath;
    private void init()
    {
        cameraPath = Path.Combine("Prefabs", "camera", "Main_Camera");
    }

    public Object GetCamera()
    {
        return Resources.Load(cameraPath);
    }
}

