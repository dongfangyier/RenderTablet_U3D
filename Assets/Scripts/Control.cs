﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class Control : MonoBehaviour
{

    private bool bInit = false;
    private int fileId = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 每6秒 执行一次
        InvokeRepeating("RenderRandomModels", 1, 8);
    }

    private void RenderRandomModels()
    {
        if (bInit)
        {
            return;
        }
        bInit = true;

        // 加载模型文件
        // ------
        List<string> modelNames = Models.getInstance().GetModelNames();
        for (int i = 0; i < modelNames.Count; i++)
        {
            float z = Random.Range(-5.0f, 5.0f);
            float y = 3.0f;
            float x = Random.Range(-5.0f, 5.0f);

            // 添加组件
            GameObject temp = Instantiate(Models.getInstance().GetModelsByName(modelNames[i])) as GameObject;
            temp.AddComponent<MeshCollider>();
            temp.GetComponent<MeshCollider>().convex = true;
            temp.AddComponent<Rigidbody>();
            temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            temp.AddComponent<BVisible>();


            temp.transform.localScale = new Vector3(200.0f, 200.0f, 200.0f);
            temp.transform.Translate(new Vector3(x, y, z));
            temp.transform.parent = this.transform;
        }

        // 添加摄像机和灯光 并渲染图片
        // ------
        StartCoroutine(RenderPic());
    }

    #region functions

    private IEnumerator RenderPic()
    {
        // 2s后执行 因为有些力的关系需要计算清楚
        yield return new WaitForSeconds(4);

        // 冻结位置
        // ------
        float minX, maxX;
        float minZ, maxZ;
        FreezeRigidbody(out minX, out maxX, out minZ, out maxZ);

        // 加载摄像机
        // ------
        GameObject cameraObj;
        cameraObj = Instantiate(MyCamera.getInstance().GetCamera()) as GameObject;
        cameraObj.transform.position = new Vector3(Mathf.Round((maxX - minX) / 2), 5.0f, Mathf.Round((maxZ - minZ) / 2) - 15.0f);
        cameraObj.transform.Rotate(0.0f, -10f, 0.0f);

        // 加载灯光
        // ------
        GameObject lightObj;
        lightObj = Instantiate(MyLight.getInstance().GetLight()) as GameObject;
        lightObj.transform.Translate(new Vector3(Random.Range(-4.0f, 4.0f), 10.0f, 0.0f));
        lightObj.transform.Rotate(new Vector3(Random.Range(30.0f, 60.0f), 0.0f, 0.0f));

        // 1s后执行 是为了触发BVisible
        yield return new WaitForSeconds(1);

        // 存储物体坐标
        // ------
        SaveScreenCoordinate(ref cameraObj);

        // 等待渲染线程结束
        yield return new WaitForEndOfFrame();

        // 存储截屏
        // ------
        SaveScreenPic();

        // 销毁物体
        // ------
        DestoryModels(ref lightObj, ref cameraObj);
    }

    private void DestoryModels(ref GameObject lightObj, ref GameObject cameraObj)
    {
        Destroy(lightObj);
        Destroy(cameraObj);
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        bInit = false;
        fileId++;
    }

    // 冻结刚体，并返回此时物体的大致位置
    private void FreezeRigidbody(out float minX, out float maxX, out float minZ, out float maxZ)
    {
        minX = float.MaxValue; maxX = float.MinValue;
        minZ = float.MaxValue; maxZ = float.MinValue;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject temp = this.transform.GetChild(i).gameObject;
            temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            float x = temp.transform.position.x;
            float z = temp.transform.position.z;

            // 记录模型文件的大致范围x, z
            if (x < minX)
            {
                minX = x;
            }
            if (x > maxX)
            {
                maxX = x;
            }
            if (z < minZ)
            {
                minZ = z;
            }
            if (z > maxZ)
            {
                maxZ = z;
            }
        }
    }

    // 存储物体的屏幕坐标
    private void SaveScreenCoordinate(ref GameObject cameraObj)
    {
        string str = "";
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject tempObject = this.transform.GetChild(i).gameObject;
            if (!tempObject.GetComponent<BVisible>().bVisible)
            {
                Debug.Log("loss...");
                continue;
            }
            List<Vector3> screen_mesh_list = new List<Vector3>();
            foreach(Vector3 vector3 in MeshUtils.GetAllMeshes(tempObject))
            {
                screen_mesh_list.Add(Camera.main.WorldToScreenPoint(vector3));
            }
            str += tempObject.name;
            str += ",";
            foreach(Vector2 screens in MeshUtils.GetFourVertices_Screen(screen_mesh_list))
            {
                str += string.Format("({0},{1}),", screens.x, screens.y);
            }
            str += "\n";

        }
        File.WriteAllText(Path.Combine(Application.dataPath, "Result", "PosInfo_"+fileId.ToString()+".txt"), str);
        
    }

    private void SaveScreenPic()
    {
       ScreenCapture.CaptureScreenshot(Path.Combine(Application.dataPath, "Result", "ImgInfo_" + fileId.ToString() + ".png"));
    }

    #endregion
}
