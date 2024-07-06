using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_boom : MonoBehaviour
{
        public float xAngle, yAngle, zAngle;
        private GameObject mast;

    void Start()
    {
        mast=GameObject.Find("Mast");
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)){
        mast.transform.rotation=Quaternion.Euler(0f,0f,-90f);
        }
        else if(Input.GetKey(KeyCode.Alpha2)){
        mast.transform.rotation=Quaternion.Euler(0f,0f,-135f);
        }
        else if(Input.GetKey(KeyCode.Alpha3)){
        mast.transform.rotation=Quaternion.Euler(0f,0f,-180f);
        }

// project settings > input manager

    }
}
