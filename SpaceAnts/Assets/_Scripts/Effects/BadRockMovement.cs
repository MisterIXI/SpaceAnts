using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadRockMovement : MonoBehaviour
{
    private GameObject mothership;
    private Vector3 startPosition; 
    private float finishTime;
    private float delayTime;
    // Start is called before the first frame update
    void Start()
    {
        mothership = GameObject.FindGameObjectWithTag("HomeBase");
        finishTime = (Random.Range ( 0.5f, 1f))*50;
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        delayTime += Time.deltaTime/ finishTime;
        transform.position = Vector3.Lerp(startPosition,mothership.transform.position,delayTime);
    }
}
