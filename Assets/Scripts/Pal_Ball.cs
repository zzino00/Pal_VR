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
            float rand = Random.Range(0.0f, 1.0f);
       
            if(rand<= collision.gameObject.GetComponent<Monster>().catchProb)
            {
                player.monsterGoList.Add(collision.gameObject);
                player.summonedPal = player.monsterGoList[0];
                gameObject.SetActive(false);
                collision.gameObject.SetActive(false);
            }
           
           
        }

        //ToDO: 소환용 팔볼과 포획용 팔볼 버튼은로 조작가능하게 바꾸기
        if (ballState == BallState.Summon&&collision.gameObject.tag =="Ground")
        {
            player.SummonPal();
            gameObject.SetActive(false);
            Debug.Log("Pal Spawned");
        }

    }

   
}
