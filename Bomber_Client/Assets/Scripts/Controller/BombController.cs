using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private Bomb bombPrefab;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Create(BombModel model)
    {
        var bomb = Instantiate(bombPrefab);
        bomb.Setup(model.CurrnetTime, model.Position);
    }
}