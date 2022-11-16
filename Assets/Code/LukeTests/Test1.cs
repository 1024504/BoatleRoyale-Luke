using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
	/// <summary>
	/// Findings:
	/// transform.forward == transform.rotation*(0,0,1)
	/// transform.rotation.eulerAngles == (transform.rotation*Quaternion.Euler(Vector3.zero)).eulerAngles
	/// transform.rotation.eulerAngles+eulerAngles == (transform.rotation*Quaternion.Euler(eulerAngles)).eulerAngles
	/// </summary>
	
    void Start()
    {
        
    }

    void Update()
    {
	    // Debug.Log(transform.rotation*new Vector3(0,1,0));
	    // Debug.Log(transform.up);
	    // Vector3 eulerAngles = new Vector3(0, 30, 0);
	    // Debug.Log((transform.rotation*Quaternion.Euler(eulerAngles)).eulerAngles);
	    // Debug.Log(transform.rotation.eulerAngles+eulerAngles);
	    Debug.Log(transform.TransformDirection(new Vector3(8, 1, 5)) * 5f);
	    Debug.Log(transform.TransformDirection(new Vector3(8, 1, 5)*5f));
    }
}
