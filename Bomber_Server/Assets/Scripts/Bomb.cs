using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private const float EXPLODE_TIME = 3f;
    private const float EXPLODE_DURATION = 0.1f;
    [SerializeField] private GameObject sphere;
    private float time = 0;
    private BombController bombController;
    private int id;
    public float CurrentTime => time;

    public int Id => id;

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void Setup(BombController bombController, int id)
    {
        this.bombController = bombController;
        this.id = id;
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if (time > EXPLODE_TIME)
        {
            GetComponent<Collider>().enabled = true;
            if (time - EXPLODE_TIME > EXPLODE_DURATION)
            {
                bombController.Delete(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        player!.Death();
    }
}