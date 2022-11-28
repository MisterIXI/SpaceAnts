using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public GameObject badRockPrefab;
    private GameObject currentAsteroid;
    private LineRenderer lineRenderer;
    private Vector3 laserPos = new Vector3(-9f,1.3f, 0.1f);
    private Vector3 asteroidPosOffset = new Vector3 (-6.9661f,1.89936f, 1.23468f);
    private float asteroidLife;
    private int asteroidLifeTime;
    private float strengh;
    
    // BADROCK VARIABLE
    // TRAILRENDERER
    // IS ALIVE AND LIFE > 0 
    // FUNCTION ASTEROID DESTROY
    // RESOURCE MANAGEMENT
    // UPDATE   // check astroid // Manage Resource // Shoot 

    //IF ASTEROID ALIVE SHOOT OTHERWHISE SPAWN AND LOOK FOR NEXT ASTEROID
    void Start()
    {
        currentAsteroid = GameObject.FindGameObjectWithTag("BadRock");
        lineRenderer = GetComponentInChildren<LineRenderer>();
        asteroidLife = 200f;
        asteroidLifeTime = 1;
    }         

    // Update is called once per frame
    private void AsteroidDestroy()
    {
        Destroy(currentAsteroid);
        //laser turn off
        MotherShipLaser(laserPos);
        // ADD MISSION PROGRESS +1 
        CreateNewAsteroid();
    }
    private void CheckAsteroid()
    {
        if(currentAsteroid == null)
        {
            CreateNewAsteroid();        
        }
    }
    private void PayResource()
    {
        // GET RESOURCE
        // CALCULATE LASERCOSTS OVER TIME
        // COSTS
        // define strengh
        // 5 Minerals, 2 Crystals, 1 Gas for 100% laserpower else 10%
        strengh =  1f;
    }
    private void MotherShipLaser(Vector3 pos)
    {
        lineRenderer.SetPosition(1, pos);
    }
    private void CreateNewAsteroid()
    {
        Vector3 newPos = new Vector3(Random.Range(0, 200), Random.Range(0, 200),Random.Range(0, 200));
        //  asteroid Script start

        // GET RANDOM POSITION
        // INSTANTIATE PREFAB
        currentAsteroid = Instantiate(badRockPrefab, newPos, Quaternion.identity);
        asteroidLife = 200f * asteroidLifeTime;

        // Init life+Lifetime
        // MOTHERSHIP LOOK TO NEW ASTEROID
        // MOTHERSHIP LASER START
        gameObject.transform.LookAt(currentAsteroid.transform);
        MotherShipLaser(currentAsteroid.transform.position + asteroidPosOffset);

    }
    private void Reducelife()
    {
        // reduce * time.deltatime   strengh per second
        asteroidLife -= strengh*Time.deltaTime;
    }
    void Update()
    {
       CheckAsteroid();
       PayResource();
       Reducelife();
       MotherShipLaser(currentAsteroid.transform.position + asteroidPosOffset);
    }
}
