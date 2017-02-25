using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour {

    static SocketIOComponent socket;

    public GameObject PlyerPrefab;

    public GameObject myPlayer;

    Dictionary<string, GameObject> players;

	void Start () {
        socket = GetComponent<SocketIOComponent>();
        socket.On("connect", OnConnected);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisConnected);
        socket.On("requestPosition", OnRequestPostion);
        socket.On("updatePosition", OnUpdatePosition);

        players = new Dictionary<string, GameObject>();
	}

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("coonected");
    }

    void OnSpawned(SocketIOEvent e)
    {
        var player = Instantiate(PlyerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        if(e.data["x"])
        {
            var movePosition = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "z"));

            var navigatePos = player.GetComponent<NavigatePosition>();

            navigatePos.NavigateTo(movePosition);

            Debug.Log("move position " + movePosition);

        }
           
        players.Add(e.data["id"].str, player);

        Debug.Log("players count: " + players.Count);

    }

    void OnMove (SocketIOEvent e)
    {

        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "z"));

        var id = e.data["id"].str;

        Debug.Log("player is moving: " + id);

        var player = players[id];

        var navigatePos = player.GetComponent<NavigatePosition>();

        navigatePos.NavigateTo(position);

    }


    void OnRequestPostion(SocketIOEvent e)
    {
        socket.Emit("updatePosition", new JSONObject(VectorToJson(myPlayer.transform.position)));
    }

    void OnUpdatePosition(SocketIOEvent e)
    {
        var position = new Vector3(GetFloatFromJson(e.data, "x"), 0, GetFloatFromJson(e.data, "z"));

        var player = players[e.data["id"].str];

        player.transform.position = position;
    }

    void OnDisConnected(SocketIOEvent e)
    {
        Debug.Log("player disconnected: " + e.data);

        var id = e.data["id"].str;

        var player = players[id];

        Destroy(player);

        players.Remove(id);
    }

    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"",""z"":""{1}""}}", vector.x, vector.z);
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].str.Replace("\""," "));
    }
}
