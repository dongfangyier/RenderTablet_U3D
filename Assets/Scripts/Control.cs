using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private GameObject cameraObj;
    private GameObject lightObj;
    private List<GameObject> modelObjs;

    // Start is called before the first frame update
    void Start()
    {
        cameraObj = Instantiate(Camera.getInstance().GetCamera()) as GameObject;
        lightObj = Instantiate(Light.getInstance().GetLight()) as GameObject;
        List<string> modelNames = Models.getInstance().GetModelNames();
        for(int i=0; i < modelNames.Count; i++)
        {
            GameObject temp = Instantiate(Models.getInstance().GetModelsByName(modelNames[i])) as GameObject;
            //modelObjs.Add(temp);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
