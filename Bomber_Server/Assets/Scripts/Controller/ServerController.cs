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
    private BombController bombController;

    [SerializeField]
    private SpawnArea spawnArea;

    private Dictionary<int, PlayerController> playerControllers = new Dictionary<int, PlayerController>();
    private List<int> playerRemoveIds = new List<int>();
    private List<int> playerDeathIds = new List<int>();

    public Vector3 RandomSpawnPoint() => spawnArea.RandomSpawnPoint();

    public void CreatePlayer(PeerConnection peerConnection)
    {
        var playerController = Instantiate(playerControllerPrefab);
        var id = peerConnection.Id;
        playerController.Setup(id, server);
        playerControllers.Add(id, playerController);
        peerConnection.AddPlayer(playerController);
    }

    public void Remove(PlayerController playerController)
    {
        var playerId = playerController.Id;
        playerRemoveIds.Add(playerId);
        playerControllers.Remove(playerId);
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

        model.PlayerRemoveIds.AddRange(playerRemoveIds);
        playerRemoveIds.Clear();
        model.PlayerDeathIds.AddRange(playerDeathIds);
        playerDeathIds.Clear();

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

        foreach (var bomb in bombController.newBombs)
        {
            var bombModel = new BombModel
            {
                CurrnetTime = bomb.CurrentTime,
                Position = bomb.transform.position
            };
            model.NewBombs.Add(bombModel);
        }
        bombController.ResetNewBomb();

        server.SendAll(model);
    }

    public void Death(PlayerController playerController)
    {
        var id = playerController.Id;
        playerControllers.Remove(id);
        playerDeathIds.Add(id);
    }
}