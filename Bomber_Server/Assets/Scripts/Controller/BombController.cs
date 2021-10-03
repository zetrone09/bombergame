using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private Bomb bombPrefab;
    private Dictionary<int, Bomb> bombs = new Dictionary<int, Bomb>();
    private int curentBombIndex = 0;

    public Dictionary<int, Bomb> Bombs => bombs;
    public List<Bomb> newBombs = new List<Bomb>();

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public Bomb Create()
    {
        var bomb = Instantiate(bombPrefab);
        var id = curentBombIndex++;
        bomb.Setup(this, id);
        bombs.Add(id, bomb);
        newBombs.Add(bomb);
        return bomb;
    }

    public void ResetNewBomb() => newBombs = new List<Bomb>();

    public void Delete(Bomb bomb)
    {
        bombs.Remove(bomb.Id);
        Destroy(bomb.gameObject);
    }
}