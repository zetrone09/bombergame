using LiteNetLib;
using LiteNetLib.Utils;
using Newtonsoft.Json;
using UnityEngine;

public class PeerConnection
{
    private PlayerController playerController;
    private Server server;
    private NetPeer peer;
    private int id;

    public int Id => id;
    public PlayerController Player => playerController;
    public int PlayerId => playerController.Id;

    public PeerConnection(Server server, NetPeer peer, int id)
    {
        this.server = server;
        this.peer = peer;
        this.id = id;
    }

    public void Send(BaseModel model) => Send(JsonConvert.SerializeObject(model));

    public void Send(string json) => peer.Send(NetDataWriter.FromString(json), DeliveryMethod.ReliableOrdered);

    public void AddPlayer(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Disconnected()
    {
        if (playerController != null)
        {
            Player?.Remove();
            server.Remove(this);
        }
    }
}