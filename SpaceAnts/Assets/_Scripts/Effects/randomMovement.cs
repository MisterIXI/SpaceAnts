using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomMovement : MonoBehaviour
{   
    private Vector3 pos;
    private Vector3 target;
    private bool moving = false;
    private float finishTime;
    private float delayTime;
    
    [SerializeField] private GameObject homebase;
    // Start is called before the first frame update
    private void getNewRandomPosition()
    {
        target = homebase.transform.position + new Vector3(Random.Range(50,200),Random.Range(-30,30),Random.Range(50,200) );
        pos = gameObject.transform.position;
        finishTime = (Random.Range ( 0f, 1f))*10;
        delayTime = 0f;
        moving = true;
    }
    private void movetoTarget()
    {
        delayTime += Time.deltaTime/ finishTime;
        gameObject.transform.position = Vector3.Lerp(pos,target, delayTime);
        
        if(delayTime > 0.9999f) 
            moving =false;
    }
    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            movetoTarget();

        }else{
            getNewRandomPosition();
        }

    }
}
