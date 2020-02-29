using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Light
{
    #region songleton
    private static Light instance = null;

    private Light()
    {
        init();
    }

    public static Light getInstance()
    {
        if (instance == null)
        {
            instance = new Light();
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
