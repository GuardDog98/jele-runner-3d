using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float laneWidth = 3.2f;
    public float jumpForce = 8.5f;
    public float gravity = 20f;

    private CharacterController controller;
    private Vector3 velocity;
    private float targetX;
    private bool isGrounded;
    private Renderer bodyRenderer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        bodyRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        targetX = transform.position.x;
        GameManager.Instance?.RegisterPlayer(this);
    }

    private void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            targetX -= laneWidth;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            targetX += laneWidth;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            velocity.y = jumpForce;
            isGrounded = false;
        }
    }

    private void MovePlayer()
    {
        float xTarget = Mathf.Clamp(targetX, -laneWidth * 2f, laneWidth * 2f);
        float currentX = Mathf.Lerp(transform.position.x, xTarget, Time.deltaTime * 12f);

        Vector3 move = new Vector3(currentX - transform.position.x, 0f, 0f);
        controller.Move(move + new Vector3(0f, 0f, moveSpeed * Time.deltaTime));

        if (controller.isGrounded)
        {
            velocity.y = -0.5f;
            isGrounded = true;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -4f, 1000f));
    }

    public void SetSkin(Color color)
    {
        if (bodyRenderer != null)
            bodyRenderer.material.color = color;
    }
}
