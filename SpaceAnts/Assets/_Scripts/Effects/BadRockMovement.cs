using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadRockMovement : MonoBehaviour
{
    private GameObject mothership;
    private Vector3 startPosition; 
    private float finishTime;
    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        mothership = ReferenceManager.homeBase.gameObject;
        finishTime = 10f;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(mothership.transform.position,startPosition,elapsedTime/finishTime);
    }
}
