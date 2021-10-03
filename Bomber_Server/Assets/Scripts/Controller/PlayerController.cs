using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MIN_MOVE_DISTANCE = 0.0625f;
    private const float GRAVITY = 9.8f;
    private const float LAY_BOMB_DELAY = 3f;

    [SerializeField] private CharacterController characterController;
    private BombController bombController;
    private int id;
    private int score = 0;
    private ServerController serverController;
    private Server server;
    private Vector3 target;

    private float speed = 1;
    private float gravitySpeed = 0;
    private bool isDeath = false;
    public int Id => id;
    public int Score => score;
    public bool IsDeath => isDeath;
    public Vector3 Position => transform.position;

    public bool isUpdatePosition;
    public bool isUpdateScore;
    public float lastLayBombTime;

    private void Awake()
    {
        serverController = FindObjectOfType<ServerController>();
        bombController = FindObjectOfType<BombController>();
    }

    public void GetCoin()
    {
        score++;
        isUpdateScore = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        server.SendCreatePlayer(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isDeath)
        {
            var v2Position = new Vector2(transform.position.x, transform.position.z);
            var v2Target = new Vector2(target.x, target.z);
            if (Vector2.Distance(v2Target, v2Position) > MIN_MOVE_DISTANCE)
            {
                var dir = target - transform.position;
                characterController.Move(dir.normalized * speed * Time.deltaTime);
                isUpdatePosition = true;
            }

            if (!characterController.isGrounded)
            {
                gravitySpeed += GRAVITY * Time.deltaTime;
                characterController.Move(Vector3.down * gravitySpeed * Time.deltaTime);
                isUpdatePosition = true;
            }
            else
            {
                gravitySpeed = 0;
            }
        }
    }

    public void Death()
    {
        isDeath = true;
        serverController.Death(this);
        Destroy(gameObject);
    }

    public void LayBomb()
    {
        if (!isDeath)
        {
            if (Time.time > lastLayBombTime + LAY_BOMB_DELAY)
            {
                var bomb = bombController.Create();
                var position = transform.position;
                bomb.transform.position = new Vector3(position.x, 0, position.z);
                lastLayBombTime = Time.time;
            }
        }
    }

    public void Remove()
    {
        serverController.Remove(this);
        Destroy(gameObject);
    }

    internal void Setup(int id, Server server)
    {
        this.id = id;
        this.server = server;
    }

    internal void Move(Vector3 target)
    {
        this.target = target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        Debug.Log(collision.gameObject.name);
    }
}