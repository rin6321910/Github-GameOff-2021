using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarSceneController : MonoBehaviour
{
    GameObject player;
    GameObject[] enemy;
    [SerializeField] private float PlayerVisionRange;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        
    }
    void Update()
    {
        playerVision();
        checkEnemyAIAgent();
    }
    void checkEnemyAIAgent(){
        foreach(GameObject enemy in enemy){
            if(!enemy || enemy.transform.childCount == 0){continue;} 
            
            if(enemy.GetComponent<AIAgent>().enabled){
                enemy.transform.GetChild(0).gameObject.SetActive(true);//hard code
                enemy.transform.GetChild(2).gameObject.SetActive(true);//hard code
            }
        }
    }
    void playerVision(){
        foreach(GameObject enemy in enemy){

            if(!enemy || enemy.transform.childCount == 0){continue;}

            if (Vector2.Distance(player.transform.position, enemy.transform.position) <= PlayerVisionRange){
                enemy.transform.GetChild(0).gameObject.SetActive(true);//hard code
                enemy.transform.GetChild(1).gameObject.SetActive(true);//hard code
                enemy.transform.GetChild(2).gameObject.SetActive(true);//hard code
            }
            else if(!enemy.GetComponent<AIAgent>().enabled){
                enemy.transform.GetChild(0).gameObject.SetActive(false);//hard code
                enemy.transform.GetChild(1).gameObject.SetActive(false);//hard code
                enemy.transform.GetChild(2).gameObject.SetActive(false);//hard code
            }
        }
    }
}
