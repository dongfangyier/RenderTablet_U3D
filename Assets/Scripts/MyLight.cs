using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyLight
{
    #region songleton
    private static MyLight instance = null;

    private MyLight()
    {
        init();
    }

    public static MyLight getInstance()
    {
        if (instance == null)
        {
            instance = new MyLight();
        }
        return instance;
    }
    #endregion

    private string lightPath;
    private void init()
    {
        lightPath = Path.Combine("Prefabs", "light", "Directional_Light");
    }

    public Object GetLight()
    {
        return Resources.Load(lightPath);
    }
}
