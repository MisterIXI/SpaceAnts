using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Asteroids : MonoBehaviour
{

    public enum DropDown
    {
        Air,
        Energy,
        Water,
        Biodiversity
    }
    // CREATE ASTEROID // 
    public DropDown drop = DropDown.Air;
    public int value;
    public void CollectResource()
    {

    }
    public void PutRecource( Resourcetypes _resource, int value)
    {
        
    }
    public class Resourcetypes
    {
        private int air{get;set;}
        private int energy{get;set;}
        private int water{get;set;}
        private int biodiversity{get;set;}
    }
    
}
