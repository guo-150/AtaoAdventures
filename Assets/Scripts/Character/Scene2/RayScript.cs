using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayScript :Reference
{
   // public GameObject rope;
    public Image Pot;
    public GameObject Player;

    void Start()
    {
        
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 , 0));
        if (Physics.Raycast(ray,out hit,10))
        {
            Pot.color = Color.green;
        if (Input.GetKeyDown(KeyCode.R))
               {
                print("233");
             Player.transform.position += (hit.point-transform.position)*20 * Time.deltaTime;
                }
        }
        else
        {
            Pot.color = Color.white;
        }
    }
}
