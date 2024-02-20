using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int myId;
    public Item_Stat myStat;
   


    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "RightHand")
        {

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
