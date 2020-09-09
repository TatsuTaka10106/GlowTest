using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using MADGazeSDK;

public class CubeScripts : MonoBehaviour
{
    
    public Material[] cubeMaterials;
    Animator anim;
    MeshRenderer meshRenderer;
    int index;

    void Start () {
      	 anim = gameObject.GetComponent<Animator>();
      	 meshRenderer = GetComponent<MeshRenderer>();
         
         index = 0;

        if(cubeMaterials!=null && cubeMaterials.Length > 0){
            meshRenderer.material = cubeMaterials[index];
        }
        
        MADUnityIntegrator.EventClick += this.OnMADHGClickEvent;
    }

    void OnDestroy()
    {
        //unreg click event 
        MADUnityIntegrator.EventClick -= this.OnMADHGClickEvent;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            triggerClick();
        }
    }

     private void triggerClick(){

        if(anim!=null){
                anim.SetTrigger("shock");
        }


        if(cubeMaterials!=null && cubeMaterials.Length > 0 ){
            index++;
            if(index >= cubeMaterials.Length){
                index = 0;
            }
            meshRenderer.material = cubeMaterials[index];
        }
    }


    //Click event callback
    void OnMADHGClickEvent(Click click)
    {
        Debug.Log("CubeScripts: onClick: hand[" + click.index + "] position[" + click.x + ", " + click.y + "]");

        triggerClick();
    }
}
