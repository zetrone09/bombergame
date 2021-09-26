using LiteNetLib;
using Newtonsoft.Json;

//for Dictionary
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour, INetEventListener
{
    [SerializeField] private ServerController serverController;
    [SerializeField] private CoinController coinController;
    public const short PORT = 9050;
    public const string KEY = "MYKEY";
    private NetManager server;
    private int clientCurrnetId = 0;
    private Dictionary<int, PeerConnection> peerConnections = new Dictionary<int, PeerConnection>();

    private ModelToObjectMapper modelToObjectMapper;

    public Dictionary<int, PeerConnection> PeerConnections => peerConnections;

    private void Awake()
    {
        Debug.Log("Awake");
        server = new NetManager(this);
        server.Start(PORT);
    }

    private void Start()
    {
        modelToObjectMapper = new ModelToObjectMapper(serverController);
    }

    private void Update()
    {
        server.PollEvents();
        serverController.UpdateData();
    }

    public void SendCreatePlayer(PlayerController playerController)
    {
        var model = new CreatePlayerModel { Id = playerController.Id, Position = playerController.Position };
        SendAll(model);
    }

    public void SendAll(BaseModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        foreach (var clientConnection in peerConnections.Values)
        {
            clientConnection.Send(json);
        }
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        Debug.Log("OnConnectionRequest");
        CreatePeerConnection(request.AcceptIfKey(KEY));
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError) => Debug.Log("OnNetworkError");

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
    {
        Debug.Log("OnNetworkReceive");
        modelToObjectMapper.DeserializeToFunction(peer, reader.GetString());
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType) => Debug.Log("OnNetworkReceiveUnconnected");

    private void CreatePeerConnection(NetPeer peer)
    {
        var id = clientCurrnetId++;
        var peerConnection = new PeerConnection(this, peer, id);
        peer.Tag = peerConnection;

        var model = new InitDataModel();
        foreach (var clientConnection in peerConnections.Values)
        {
            var player = clientConnection.Player;
            if (player != null)
            {
                //Debug.Log($"clientConnection {clientConnection.Id} player.Id {player.Id}");
                var createPlayerModel = new CreatePlayerModel { Id = player.Id, Position = player.Position };
                model.CreatePlayerModels.Add(createPlayerModel);
            }
        }

        foreach (var pair in coinController.Coins)
        {
            var coinId = pair.Key;
            var position = pair.Value.transform.position;
            model.CreateCoins.Add(coinId, new Vector3Model(position));
        }
        peerConnections.Add(id, peerConnection);
        serverController.CreatePlayer(peerConnection);

        var playerId = peerConnection.PlayerId;
        model.PlayerId = playerId;
        peerConnection.Send(model);
    }

    public void OnPeerConnected(NetPeer peer)
    {
        Debug.Log("OnPeerConnected");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) => Debug.Log("OnPeerDisconnected");

    public void Remove(PeerConnection peerConnection)
    {
        peerConnections.Remove(peerConnection.Id);
    }
}