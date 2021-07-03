using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementKB : MonoBehaviour
{
	public static float SPEED = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.eulerAngles= transform.eulerAngles + new Vector3(Input.GetAxis("Vertical") * SPEED * Time.deltaTime, Input.GetAxis("Horizontal") * SPEED * Time.deltaTime,0f);
	}
}
