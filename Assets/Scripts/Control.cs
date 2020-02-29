using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    private GameObject cameraObj;
    private GameObject lightObj;

    // Start is called before the first frame update
    void Start()
    {
        cameraObj = Instantiate(Camera.getInstance().GetCamera()) as GameObject;
        lightObj = Instantiate(Light.getInstance().GetLight()) as GameObject;
        List<string> modelNames = Models.getInstance().GetModelNames();
        for(int i=0; i < modelNames.Count; i++)
        {
            GameObject temp = Instantiate(Models.getInstance().GetModelsByName(modelNames[i])) as GameObject;
            temp.transform.localScale = new Vector3(100, 100, 100);
            temp.transform.Translate(new Vector3(i+1,0,0));
            temp.transform.parent = this.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
