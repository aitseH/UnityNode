using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {

    static SocketIOComponent socket;

    public GameObject PlyerPrefab;

	void Start () {
        socket = GetComponent<SocketIOComponent>();
        socket.On("connect", OnConnected);
        socket.On("spawn", OnSpawned);
	}

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("coonected");
        socket.Emit("move");
    }

    void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("spawned");
        Instantiate(PlyerPrefab);
    }
}
