using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour, IClickable {

    public GameObject player;

    public void OnClick (RaycastHit hit) {
        var navPos = player.GetComponent<NavigatePosition> ();
        var natMove = player.GetComponent<NetWorkMove>();

        navPos.NavigateTo(hit.point);
        natMove.OnMove(hit.point);
	}
}
