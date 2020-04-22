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

    private string skyboxPath;
    List<string> skyboxFiles;
    private void init()
    {
        string rootPath = Path.Combine(Application.dataPath, "Resources", "Prefabs", "skybox");
        skyboxFiles = GetFilesFromDir(rootPath);
    }

    public Object GetSkyBoxs()
    {
        int tag = Random.Range(0, skyboxFiles.Count);
        skyboxPath = Path.Combine("Prefabs", "skybox", skyboxFiles[tag]);

        return Resources.Load(skyboxPath);
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
