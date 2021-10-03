using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{
    [SerializeField] private PlayerController playerControllerPrefab;
    [SerializeField] private CoinController coinController;
    [SerializeField] private BombController bombController;
    [SerializeField] private Client client;
    [SerializeField] private Text scoreText;
    private int playerId = -1;
    private Dictionary<int, PlayerController> playerControllers = new Dictionary<int, PlayerController>();

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnCreatePlayer(CreatePlayerModel model)
    {
        var playerController = Instantiate(playerControllerPrefab);
        var id = model.Id;
        playerController.Setup(model, client);
        playerControllers.Add(id, playerController);
        CheckIsCurrentPlayer(playerController);
    }

    public void OnInitData(InitDataModel model)
    {
        playerId = model.PlayerId;
        foreach (var playerModel in model.CreatePlayerModels)
        {
            OnCreatePlayer(playerModel);
        }

        OnCreateCoins(model.CreateCoins);

        foreach (var bomb in model.Bombs)
        {
            bombController.Create(bomb);
        }
    }

    private void OnCreateCoins(Dictionary<int, Vector3Model> createCoins)
    {
        coinController.OnCreateCoins(createCoins);
    }

    private void CheckIsCurrentPlayer(PlayerController playerController)
    {
        if (playerController.Id == playerId)
        {
            playerController.SetCurrentPlayer();
        }
    }

    public void UpdatePlayerModel(UpdateModel model)
    {
        foreach (var playerPositionModel in model.PlayerPositionModels)
        {
            var playerId = playerPositionModel.PlayerId;
            if (playerControllers.TryGetValue(playerId, out var player))
            {
                player.Move(playerPositionModel.Position);
            }
        }

        foreach (var removeId in model.PlayerRemoveIds)
        {
            if (playerControllers.TryGetValue(removeId, out var player))
            {
                player.Remove();
                playerControllers.Remove(removeId);
            }
        }

        foreach (var deathId in model.PlayerDeathIds)
        {
            if (deathId == playerId)
            {
                SceneManager.LoadScene("GameOverScene");
            }
            if (playerControllers.TryGetValue(deathId, out var player))
            {
                player.Remove();
                playerControllers.Remove(deathId);
            }
        }

        foreach (var bomb in model.NewBombs)
        {
            bombController.Create(bomb);
        }

        coinController.OnUpdate(model);
    }

    public void OnUpdatePlayerModel(UpdatePlayerModel model)
    {
        scoreText.text = $"Score : {model.Score}";
    }
}