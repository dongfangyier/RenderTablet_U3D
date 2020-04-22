using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyMaterials
{
    #region songleton
    private static MyMaterials instance = null;

    private MyMaterials()
    {
        init();
    }

    public static MyMaterials getInstance()
    {
        if (instance == null)
        {
            instance = new MyMaterials();
        }
        return instance;
    }
    #endregion

    private string materialPath;
    List<string> materialFiles;
    private void init()
    {
        string rootPath = Path.Combine(Application.dataPath, "Resources", "Materials");
        materialFiles = GetFilesFromDir(rootPath);
    }

    public Material GetMaterial()
    {
        int tag = Random.Range(0, materialFiles.Count);
        materialPath = Path.Combine("Materials", materialFiles[tag]);
        Material mat= (Material)Resources.Load(materialPath);
        mat.SetFloat("_Smoothness", Random.Range(0.1f, 1.0f));
        return mat;

    }

    #region utils

    private List<string> GetFilesFromDir(string path, string type = "*.mat")
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
