using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
   
    void Update()
    {
        if(this.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
