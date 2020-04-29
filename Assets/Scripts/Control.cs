using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class Control : MonoBehaviour
{
    public GameObject _Board;
    private bool bInit = false;
    private int fileId = 0;

    private GameObject SkyBoxObj = null;
    private readonly string OutputPath = Path.Combine(System.Environment.CurrentDirectory, "Result");

    // Start is called before the first frame update
    void Start()
    {   
        // test 
        // RenderRandomModels();

        if (!Directory.Exists(OutputPath))
        {
            Directory.CreateDirectory(OutputPath);
        }

        // 每10秒 执行一次
        InvokeRepeating("RenderRandomModels", 1, 10);
    }

    private void RenderRandomModels()
    {
        if (bInit)
        {
            return;
        }
        bInit = true;

        // 地板材质
        // ------
        Texture2D texture = MyBoardTexture.getInstance().GetTexture() as Texture2D;
        for (int i = 0; i < _Board.transform.childCount; i++)
        {
            GameObject tempObject = _Board.transform.GetChild(i).gameObject;
            tempObject.GetComponent<MeshRenderer>().material.SetTexture("_BaseColorMap", texture);
            tempObject.GetComponent<MeshRenderer>().material.SetFloat("_Smoothness", Random.Range(0.1f, 1.0f));
        }
        texture = null;

        // 天空盒
        // ------
        SkyBoxObj = Instantiate(MySkyBoxs.getInstance().GetSkyBoxs()) as GameObject;


        // 加载模型文件
        // ------
        List<string> modelNames = Models.getInstance().GetModelNames();
        for (int i = 0; i < modelNames.Count; i++)
        {
            float z = Random.Range(-20.0f, 20.0f);
            float y = 40.0f;
            float x = Random.Range(-20.0f, 20.0f);

            GameObject temp = Instantiate(Models.getInstance().GetModelsByName(modelNames[i])) as GameObject;

            // 随机位置
            temp.transform.localScale = new Vector3(500.0f, 500.0f, 500.0f);
            temp.transform.position = new Vector3(x, y, z);
            temp.transform.Rotate(new Vector3(Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f), Random.Range(0.0f, 90.0f)));
            temp.transform.parent = this.transform;

            // 添加组件
            temp.AddComponent<MeshCollider>();
            temp.GetComponent<MeshCollider>().convex = true;
            temp.AddComponent<Rigidbody>();
            temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            temp.AddComponent<BVisible>();
        }

        // 添加摄像机和灯光 并渲染图片
        // ------
        StartCoroutine(RenderPic());
    }

    #region functions

    private IEnumerator RenderPic()
    {
        // 5s后执行 因为有些力的关系需要计算清楚
        yield return new WaitForSeconds(5);

        // 冻结位置
        // ------
        FreezeRigidbody();

        // 加载摄像机
        // ------
        GameObject cameraObj;
        cameraObj = Instantiate(MyCamera.getInstance().GetCamera()) as GameObject;
        Vector3 cameraPos = new Vector3(Random.Range(-200.0f, 200.0f), Random.Range(20.0f, 160.0f), Random.Range(-200.0f, 200.0f));
        cameraObj.transform.position = cameraPos;
        Vector3 targetPos = new Vector3(0, -5, 0);
        cameraObj.transform.LookAt(targetPos);


        // 加载灯光
        // ------
        //GameObject lightObj;
        //lightObj = Instantiate(MyLight.getInstance().GetLight()) as GameObject;
        //lightObj.transform.position = new Vector3(Random.Range(-40.0f, 40.0f), 60.0f, 0.0f); // meanless
        //lightObj.transform.Rotate(new Vector3(Random.Range(20.0f, 60.0f), Random.Range(0.0f, 180.0f), 0.0f));

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
        DestoryModels(/*ref lightObj, */ref cameraObj);
    }

    private void DestoryModels(/*ref GameObject lightObj, */ref GameObject cameraObj)
    {
        //Destroy(lightObj);
        DestroyImmediate(SkyBoxObj);
        DestroyImmediate(cameraObj);
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        bInit = false;
        fileId++;
        Resources.UnloadUnusedAssets();
    }

    // 冻结刚体，并返回此时物体的大致位置
    private void FreezeRigidbody()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject temp = this.transform.GetChild(i).gameObject;
            temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            float x = temp.transform.position.x;
            float z = temp.transform.position.z;
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
        File.WriteAllText(Path.Combine(OutputPath, "PosInfo_"+fileId.ToString()+".txt"), str);
        
    }

    private void SaveScreenPic()
    {
       ScreenCapture.CaptureScreenshot(Path.Combine(OutputPath, "ImgInfo_" + fileId.ToString() + ".png"));
    }

    #endregion
}
