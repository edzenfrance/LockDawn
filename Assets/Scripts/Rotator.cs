using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
  public float speed = 1;
  public float RotAngleY = 45;

  void Update()
  {
    float rY = Mathf.SmoothStep(0,RotAngleY,Mathf.PingPong(Time.time * speed,1));
    transform.rotation = Quaternion.Euler(0,rY,0);
  }
}
