using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    public Transform Totransform;
    public Transform pos1;
    public Transform pos2;
    public float multiplier=1f;
    private float timer=0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        Totransform.position=Vector3.Lerp(pos1.position,pos2.position,timer*multiplier);
        if(Vector3.Distance(Totransform.position,pos2.position)<0.1f)
        {
            Debug.Log("Switch");
            Totransform.position=pos2.position;
            var temp=pos1;
            pos1=pos2;
            pos2=temp;
            timer=0f;
            
        }   
        
    }
}
