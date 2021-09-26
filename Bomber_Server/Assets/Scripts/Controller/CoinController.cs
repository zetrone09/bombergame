using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private int coinAmount = 64;
    private Dictionary<int, Coin> coins = new Dictionary<int, Coin>();
    private Dictionary<int, Vector3> newCoinPositions = new Dictionary<int, Vector3>();
    private List<int> deletedCoinIds = new List<int>();

    private int coinCurrentId;
    private ServerController serverController;
    public Dictionary<int, Coin> Coins => coins;
    public Dictionary<int, Vector3> NewCoinPositions => newCoinPositions;
    public List<int> DeletedCoinIds => deletedCoinIds;

    public void ResetNewCoins() => newCoinPositions = new Dictionary<int, Vector3>();

    public void ResetDeletedCoins() => deletedCoinIds = new List<int>();

    private void Awake()
    {
        serverController = FindObjectOfType<ServerController>();
        CreateFillCoins();
    }

    private void CreateFillCoins()
    {
        while (coins.Count < coinAmount)
        {
            var id = coinCurrentId++;
            var coin = Instantiate(coinPrefab);
            coin.Setup(id, this);
            var position = serverController.RandomSpawnPoint();
            coin.transform.position = position;
            newCoinPositions.Add(id, position);
            coins.Add(id, coin);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        CreateFillCoins();
    }

    public void DeleteCoin(int id)
    {
        if (coins.ContainsKey(id))
        {
            coins.Remove(id);
            deletedCoinIds.Add(id);
        }
    }
}