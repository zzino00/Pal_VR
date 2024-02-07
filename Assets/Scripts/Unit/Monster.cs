using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
   public int myId = 1;
   public Pal_Stat myStat;
   public PalState palState = PalState.Wild;
    public enum PalState
    {
        Wild,
        Catched
    }
    void Start()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (palState == PalState.Wild && collision.gameObject.tag == "Pal_Ball")
        {
            this.gameObject.SetActive(false);
        }
    }
}
