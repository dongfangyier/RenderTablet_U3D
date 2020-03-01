using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyCamera
{
    #region songleton
    private static MyCamera instance = null;

    private MyCamera()
    {
        init();
    }

    public static MyCamera getInstance()
    {
        if (instance == null)
        {
            instance = new MyCamera();
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

