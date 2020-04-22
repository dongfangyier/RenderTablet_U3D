using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class MySkyBoxs
{
    #region songleton
    private static MySkyBoxs instance = null;

    private MySkyBoxs()
    {
        init();
    }

    public static MySkyBoxs getInstance()
    {
        if (instance == null)
        {
            instance = new MySkyBoxs();
        }
        return instance;
    }
    #endregion

    private string materialPath;
    List<string> materialFiles;
    private void init()
    {
        string rootPath = Path.Combine(Application.dataPath, "Resources", "Prefabs","SkyBoxs");
        materialFiles = GetFilesFromDir(rootPath);
    }

    public Object GetSkyBoxs()
    {
        int tag = Random.Range(0, materialFiles.Count);
        materialPath = Path.Combine("Prefabs", "SkyBoxs", materialFiles[tag]);

        return Resources.Load(materialPath);
    }

    #region utils

    private List<string> GetFilesFromDir(string path, string type = "*.prefab")
    {
        List<string> res = new List<string>();
        DirectoryInfo folder = new DirectoryInfo(path);


        foreach (FileInfo file in folder.GetFiles(type))
        {
            res.Add(file.Name.Split('.')[0]);
        }
        return res;
    }

    #endregion
}
