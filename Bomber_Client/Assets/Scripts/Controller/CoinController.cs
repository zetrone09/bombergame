using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Dictionary<int, Coin> coins = new Dictionary<int, Coin>();
    [SerializeField] private Coin coinPrefab;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnCreateCoins(Dictionary<int, Vector3Model> createCoins)
    {
        foreach (var pair in createCoins)
        {
            var id = pair.Key;
            var position = pair.Value.ToUnityVector3();
            var coin = Instantiate(coinPrefab);
            coin.transform.position = position;
            coins.Add(id, coin);
        }
    }

    public void OnUpdate(UpdateModel model)
    {
        foreach (var id in model.DeletedCoins)
        {
            if (coins.TryGetValue(id, out var coin))
            {
                coins.Remove(id);
                Destroy(coin.gameObject);
            }
        }

        OnCreateCoins(model.CreateCoins);
    }
}