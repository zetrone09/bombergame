using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    [SerializeField]
    private Server server;

    [SerializeField]
    private PlayerController playerControllerPrefab;

    [SerializeField]
    private CoinController coinController;

    [SerializeField]
    private SpawnArea spawnArea;

    private Dictionary<int, PlayerController> playerControllers = new Dictionary<int, PlayerController>();

    public Vector3 RandomSpawnPoint() => spawnArea.RandomSpawnPoint();

    public void CreatePlayer(PeerConnection peerConnection)
    {
        var playerController = Instantiate(playerControllerPrefab);
        var id = peerConnection.Id;
        playerController.Setup(id, server);
        playerControllers.Add(id, playerController);
        peerConnection.AddPlayer(playerController);
    }

    public void UpdateData()
    {
        var model = new UpdateModel();

        foreach (var clientConnection in server.PeerConnections.Values)
        {
            var player = clientConnection.Player;
            if (player != null)
            {
                if (player.isUpdateScore)
                {
                    var playerModel = new UpdatePlayerModel();
                    playerModel.Score = player.Score;
                    clientConnection.Send(playerModel);
                    player.isUpdateScore = false;
                }

                if (player.isUpdatePosition)
                {
                    var playerPositionModel = new PlayerPositionModel { PlayerId = player.Id, Position = player.Position };
                    model.PlayerPositionModels.Add(playerPositionModel);
                    player.isUpdatePosition = false;
                }
            }
        }

        foreach (var pair in coinController.NewCoinPositions)
        {
            var id = pair.Key;
            var position = new Vector3Model(pair.Value);
            model.CreateCoins.Add(id, position);
        }
        coinController.ResetNewCoins();

        foreach (var coinId in coinController.DeletedCoinIds)
        {
            model.DeletedCoins.Add(coinId);
        }
        coinController.ResetDeletedCoins();

        server.SendAll(model);
    }
}