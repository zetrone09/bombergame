using LiteNetLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ModelToObjectMapper
{
    private ServerController serverController;
    private Dictionary<string, Action<PeerConnection, JObject>> Deserializes;

    private Dictionary<string, Action<PeerConnection, JObject>> CreateDeserializes() => new Dictionary<string, Action<PeerConnection, JObject>>
    {
        //TODO [Model.CLASS_NAME] = callFunction,
        [MovePlayerModel.CLASS_NAME] = OnMovePlayer,
        [LayBombModel.CLASS_NAME] = OnLayBomb,
    };

    private void OnMovePlayer(PeerConnection peerConnection, JObject jObject)
    {
        var model = jObject.ToObject<MovePlayerModel>();
        //Debug.Log($"{model.ClassName} : ( x : {model.Target.X}, y : {model.Target.Z} )");
        peerConnection.Player.Move(model.Target.ToUnityVector3());
    }

    private void OnLayBomb(PeerConnection peerConnection, JObject jObject)
    {
        var model = jObject.ToObject<LayBombModel>();
        if (model != null)
        {
            peerConnection.Player.LayBomb();
        }
    }

    public ModelToObjectMapper(ServerController serverController)
    {
        Deserializes = CreateDeserializes();
        this.serverController = serverController;
    }

    public void DeserializeToFunction(NetPeer peer, string json)
    {
        var jObject = JObject.Parse(json);
        if (jObject.TryGetValue("ClassName", out var value))
        {
            var className = value.ToString();
            var peerConnection = peer.Tag as PeerConnection;
            Deserializes[className](peerConnection, jObject);
        }
    }
}