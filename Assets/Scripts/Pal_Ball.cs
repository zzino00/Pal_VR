using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pal_Ball : MonoBehaviour
{
   public Player player;
    public BallState ballState;

    public enum BallState
    {
        Catch,
        Summon
    }
    private void Start()
    {
    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ballState ==BallState.Catch&& collision.gameObject.tag == "Monster")
        {
            Debug.Log("monster Captured");
            // player.myPalList.Add(collision.gameObject.GetComponent<Monster>().myId);
            float rand = Random.Range(0.0f, 1.0f);
            Debug.Log(rand);
            Debug.Log(rand <= collision.gameObject.GetComponent<Monster>().catchProb);
            if(rand<= collision.gameObject.GetComponent<Monster>().catchProb)
            {
                player.monsterGoList.Add(collision.gameObject);
                gameObject.SetActive(false);
                collision.gameObject.SetActive(false);
            }
           
           
        }

        //ToDO: ��ȯ�� �Ⱥ��� ��ȹ�� �Ⱥ� ��ư���� ���۰����ϰ� �ٲٱ�
        if (ballState == BallState.Summon&&collision.gameObject.tag =="Ground")
        {
            player.SummonPal();
            gameObject.SetActive(false);
            Debug.Log("Pal Spawned");
        }

    }

   
}
