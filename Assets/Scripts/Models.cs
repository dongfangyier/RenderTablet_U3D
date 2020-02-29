using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Models
{
    #region songleton
    private static Models instance = null;

    private Models()
    {
        init();
    }

    public static Models getInstance()
    {
        if(instance == null)
        {
            instance = new Models();
        }
        return instance;
    }
    #endregion

    private Dictionary<string, string> modelPaths = new Dictionary<string, string>();
    private void init()
    {
        modelPaths.Clear();
        modelPaths.Add("circle00", Path.Combine("Prefabs", "circle", "circle00"));
        modelPaths.Add("capsule00", Path.Combine("Prefabs", "capsule", "capsule00"));
        modelPaths.Add("capsule01", Path.Combine("Prefabs", "capsule", "capsule01"));
        modelPaths.Add("capsule02", Path.Combine("Prefabs", "capsule", "capsule02"));
        modelPaths.Add("capsule03", Path.Combine("Prefabs", "capsule", "capsule03"));

    }

    public Object GetModelsByName(string name)
    {
        return Resources.Load(modelPaths[name]);
    }

    public List<string> GetModelNames()
    {
        return modelPaths.Keys.ToList<string>();
    }

}
