using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask layerMask;
    private int id;
    private Client client;
    private bool isCurrentPlayer;

    public int Id => id;

    internal void Setup(CreatePlayerModel model, Client client)
    {
        id = model.Id;
        transform.position = model.Position;
        this.client = client;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (isCurrentPlayer && Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, int.MaxValue, layerMask))
            {
                //Debug.Log(hit.point);
                var target = new VectorXZModel { X = hit.point.x, Z = hit.point.z };
                client.Send(new MovePlayerModel { Target = target });
            }
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            client.Send(new LayBombModel());
        }
    }

    public void SetCurrentPlayer()
    {
        isCurrentPlayer = true;
    }

    public void Move(Vector3 position)
    {
        transform.position = position;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}