using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Serializable = System.SerializableAttribute;

[Serializable]
public class SpawnArea
{
    [SerializeField] private Vector2 size = new Vector2(8, 8);

    public Vector3 RandomSpawnPoint() => new Vector3(Random.Range(-size.x, size.x), 0, Random.Range(-size.y, size.y));
}