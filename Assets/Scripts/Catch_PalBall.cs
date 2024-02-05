using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Catch_PalBall : MonoBehaviour
{
   public Player player;
    private void Start()
    {
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            Debug.Log("monster Captured");
            player.myPalList.Add(collision.gameObject.GetComponent<Monster>().myId);
            player.monsterGoList.Add (collision.gameObject);
        }

    }

   
}
