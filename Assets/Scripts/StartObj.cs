using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartObj : MonoBehaviour
{
    public ActionManager actionManager;
   public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pal_Ball")
        {
            Debug.Log("LoadScene");
            actionManager.isStartScene = false;
            Destroy(player);
            SceneManager.LoadScene("InGame");
        }
    }


  
}
