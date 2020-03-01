using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVisible : MonoBehaviour
{
    public bool bVisible = false;

    void OnBecameInvisible()
    {
        bVisible = false;

    }

    void OnBecameVisible()
    {
        bVisible = true;
    }
}
