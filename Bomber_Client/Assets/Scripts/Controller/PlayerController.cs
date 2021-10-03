using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask layerMask;
    private int id;
    private Client client;
    private bool isCurrentPlayer;
    private bool isDeath;
    private bool isMyPlayer;

    private List<float> distances = new List<float>();
    private Vector3 lastPosition;
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

        var distance = Vector3.Distance(lastPosition, transform.position);

        distances.Add(distance);
        if (distances.Count > 30)
        {
            distances.RemoveAt(0);
        }

        var sum = 0f;
        foreach (var d in distances)
        {
            sum += d;
        }
        if (sum / distances.Count > 0.125f * Time.deltaTime)
        {
            animator.SetBool("IsRun", true);
        }
        else
        {
            animator.SetBool("IsRun", false);
        }
        lastPosition = transform.position;

        if (isDeath)
        {
            animator.SetBool("IsDeath", true);
        }
    }

    public void SetCurrentPlayer()
    {
        isCurrentPlayer = true;
    }

    public void Move(Vector3 position)
    {
        var target = new Vector3(position.x, transform.position.y, position.z);
        if (Vector3.Distance(transform.position, target) > Time.deltaTime * 0.125f)
        {
            transform.LookAt(target, Vector3.up);
        }
        transform.position = position;
    }

    public void Remove()
    {
        if (isDeath && isMyPlayer)
        {
            SceneManager.LoadScene("GameOverScene");
        }
        Destroy(gameObject);
    }

    public void Death(bool isMyPlayer)
    {
        if (!isDeath)
        {
            this.isMyPlayer = isMyPlayer;
            isDeath = true;
            Invoke("Remove", 3f);
        }
    }
}