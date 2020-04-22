using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class MyBoardTexture
{
    #region songleton
    private static MyBoardTexture instance = null;

    private MyBoardTexture()
    {
        init();
    }

    public static MyBoardTexture getInstance()
    {
        if (instance == null)
        {
            instance = new MyBoardTexture();
        }
        return instance;
    }
    #endregion

    private string texturePath;
    List<string> textureFiles;
    private void init()
    {
        string rootPath = Path.Combine(Application.dataPath, "Resources", "Textures");
        textureFiles = GetFilesFromDir(rootPath);
    }

    public Object GetTexture()
    {
        int tag = Random.Range(0, textureFiles.Count);
        texturePath = Path.Combine("Textures", textureFiles[tag]);
        return Resources.Load(texturePath);

    }

    #region utils

    private List<string> GetFilesFromDir(string path, string type = "*.png")
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
