using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtils
{
    /// <summary>
    /// 得到mesh的顶点坐标
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
   public static List<Vector3> GetAllMeshes(GameObject gameObject)
    {
        List<Vector3> res = new List<Vector3>();
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        foreach (var vertex in mesh.vertices)
        {
            res.Add(gameObject.transform.TransformPoint(vertex));
        }
        return res;
    }

    /// <summary>
    /// 返回的顺序 右上 右下 左下 左上
    /// </summary>
    /// <param name="vector3s"></param>
    /// <returns></returns>
    public static List<Vector2> GetFourVertices_Screen(List<Vector3> vector3s)
    {
        Vector2 leftDown = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 rightTop = new Vector2(float.MinValue, float.MinValue);
        foreach(Vector3 vec3 in vector3s)
        {
            if(vec3.x < leftDown.x)
            {
                leftDown.x = vec3.x;
            }
            if (vec3.y < leftDown.y)
            {
                leftDown.y = vec3.y;
            }
            if (vec3.x > rightTop.x)
            {
                rightTop.x = vec3.x;
            }
            if (vec3.y > rightTop.y)
            {
                rightTop.y = vec3.y;
            }
        }

        return new List<Vector2> { rightTop, new Vector2(rightTop.x, leftDown.y), leftDown, new Vector2(leftDown.x, rightTop.y) };
    }
}
