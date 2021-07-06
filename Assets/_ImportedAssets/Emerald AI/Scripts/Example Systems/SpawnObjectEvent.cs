using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EmeraldAI.Example
{
    public class SpawnObjectEvent : MonoBehaviour
    {
        //The object to spawn
        public GameObject ObjectToSpawn;

        //Call this function through an Emerald AI Event to spawn an object when the chosen event is called.
        //This script needs to be attached to your AI.
        public void SpawnObject()
        {
            Instantiate(ObjectToSpawn, transform.position, Quaternion.identity);
        }
    }
}