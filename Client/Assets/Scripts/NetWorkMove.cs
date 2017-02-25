using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetWorkMove : MonoBehaviour {

    public SocketIOComponent socket;

    public void OnMove(Vector3 positon)
    {
        socket.Emit("move", new JSONObject(Network.VectorToJson(positon)));
    }
}
