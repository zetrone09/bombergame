using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private float rotateSpeed = 270;
    [SerializeField] private float coinHigh = 1.5f;
    [SerializeField] private float coinJumpHigh = 0.25f;
    [SerializeField] private float coinJumpSpeed = 4f;

    private void Start()
    {
    }

    private void Update()
    {
        var eulerAngles = model.transform.eulerAngles;
        var position = model.transform.position;

        eulerAngles.y = rotateSpeed * Time.time;
        position.y = coinHigh + Mathf.Sin(Time.time * coinJumpSpeed) * coinJumpHigh;

        model.transform.eulerAngles = eulerAngles;
        model.transform.position = position;
    }
}