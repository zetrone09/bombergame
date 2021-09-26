using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private CoinController coinController;
    private int id;
    public int Id => id;

    public void Setup(int id, CoinController coinController)
    {
        this.id = id;
        this.coinController = coinController;
    }

    public void Delete()
    {
        coinController.DeleteCoin(id);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.GetCoin();
            Delete();
        }
    }
}