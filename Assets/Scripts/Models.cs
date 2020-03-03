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

    // type
    private string[] tabletType =  {"circle", "capsule","oval","special"};
    private int[] tabletCount = {1, 4, 3, 1};
    private Dictionary<string, string> modelPaths = new Dictionary<string, string>();
    private void init()
    {
        string rootPath = Path.Combine(Application.dataPath, "Resources", "Prefabs");
        modelPaths.Clear();


        // 随机抽取（每次10个药片？？）
        // ------
        for(int i = 0; i < tabletType.Length; i++)
        {
            List<string> files = GetFilesFromDir(Path.Combine(rootPath, tabletType[i]));
            int needCount = tabletCount[i];
            int index = 0;
            while (needCount > 0)
            {
                int tag = Random.Range(0, files.Count);
                string key = files[tag];
                // 防止 key 重复
                if (modelPaths.ContainsKey(key))
                {
                    index++;
                    key += ("_" + index.ToString());
                }

                modelPaths.Add(files[tag], Path.Combine("Prefabs", tabletType[i], files[tag]));
                needCount--;
            }

        }

    }

    public Object GetModelsByName(string name)
    {
        return Resources.Load(modelPaths[name]);
    }

    public List<string> GetModelNames()
    {
        return modelPaths.Keys.ToList<string>();
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
