using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] LayerMask groundLayerMask;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        RTSInputController.OnActInput += ActHandler;
        agent = GetComponent<NavMeshAgent>();
    }

    private void ActHandler()
    {
        if(this.enabled == false) return;
        RaycastHit hit;
        Debug.Log(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayerMask));
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundLayerMask))
        {
            
            agent.SetDestination(hit.point);
        }
    }

    
}
