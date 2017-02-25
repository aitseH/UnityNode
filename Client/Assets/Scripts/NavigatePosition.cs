using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigatePosition : MonoBehaviour {

    NavMeshAgent agent;

	void Awake () {
        agent = GetComponent<NavMeshAgent>();
	}

    void Update ()
    {
        GetComponent<Animator>().SetFloat("Distance", agent.remainingDistance);
    }
	
    public void NavigateTo (Vector3 position) {
        agent.SetDestination(position);
	}
}
