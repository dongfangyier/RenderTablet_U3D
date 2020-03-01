using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Control : MonoBehaviour
{

    private bool bInit = false;

    // Start is called before the first frame update
    void Start()
    {
        // 每6秒 执行一次
        InvokeRepeating("RenderRandomModels", 1, 6);
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

            GameObject temp = Instantiate(Models.getInstance().GetModelsByName(modelNames[i])) as GameObject;
            temp.transform.localScale = new Vector3(200.0f, 200.0f, 200.0f);
            temp.transform.Translate(new Vector3(x, y, z));
            temp.transform.parent = this.transform;
        }



        // 添加摄像机和灯光 并渲染图片
        // ------
        Invoke("RenderPic", 3); // 3s后执行 因为有些力的关系需要计算清楚
    }

    #region functions

    private void RenderPic()
    {
        float minX, maxX;
        float minZ, maxZ;
        FreezeRigidbody(out minX, out maxX, out minZ, out maxZ);

        // 加载摄像机
        // ------
        GameObject cameraObj;
        cameraObj = Instantiate(Camera.getInstance().GetCamera()) as GameObject;
        cameraObj.transform.position = new Vector3(Mathf.Round((maxX - minX) / 2), 5.0f, Mathf.Round((maxZ - minZ) / 2) - 14.0f);
        cameraObj.transform.Rotate(0.0f, -10f, 0.0f);


        // 加载灯光
        // ------
        GameObject lightObj;
        lightObj = Instantiate(Light.getInstance().GetLight()) as GameObject;
        lightObj.transform.Translate(new Vector3(Random.Range(-4.0f, 4.0f), 10.0f, 0.0f));
        lightObj.transform.Rotate(new Vector3(40.0f, 0.0f, 0.0f));

        // 获取物体坐标
        GetScreenCoordinate();

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

    // TO DO:获取物体的屏幕坐标
    private void GetScreenCoordinate()
    {
        Dictionary<string, Vector2> screenCoordinate;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject temp = this.transform.GetChild(i).gameObject;
            Debug.Log(temp.transform.localPosition);
        }
    }

    #endregion
}
