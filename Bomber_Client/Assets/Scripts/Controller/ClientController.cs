using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientController : MonoBehaviour
{
    [SerializeField] private PlayerController playerControllerPrefab;
    [SerializeField] private CoinController coinController;
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

        coinController.OnUpdate(model);
    }

    public void OnUpdatePlayerModel(UpdatePlayerModel model)
    {
        scoreText.text = $"Score : {model.Score}";
    }
}