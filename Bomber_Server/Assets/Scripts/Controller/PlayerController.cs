using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MIN_MOVE_DISTANCE = 0.0625f;
    private const float GRAVITY = 9.8f;

    [SerializeField] private CharacterController characterController;

    private int id;
    private Server server;
    private Vector3 target;

    private float speed = 1;
    private float gravitySpeed = 0;
    public int Id => id;

    public Vector3 Position => transform.position;
    public bool isUpdatePosition;

    // Start is called before the first frame update
    private void Start()
    {
        server.SendCreatePlayer(this);
    }

    // Update is called once per frame
    private void Update()
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

    internal void Setup(int id, Server server)
    {
        this.id = id;
        this.server = server;
    }

    internal void Move(Vector3 target)
    {
        this.target = target;
    }
}