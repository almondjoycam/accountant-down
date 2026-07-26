using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    // input stuff
    InputActionMap playerControlMap;
    InputAction move;
    InputAction look;
    InputAction interact;

    Vector2 moveInput;
    Vector3 movement;
    [SerializeField] float moveSpeed = 5.0f;

    Vector2 lookInput;
    Vector3 baseOffset;
    Vector3 newOffset;
    [SerializeField] float rotSpeed = 5.0f;
    [SerializeField] float maxRotAngle = 30;

    CharacterController character;
    SpriteRenderer sprite;
    CinemachineTransposer vcamFollow;

    // interacting
    float raycastInterval = 0.5f;
    float raycastTime = 0;
    bool canInteract = false;
    RaycastHit raycastResult;
    GameObject prompt;
    Animator promptProgress;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControlMap = InputSystem.actions.FindActionMap("Player");
        move = playerControlMap.FindAction("Move");
        look = playerControlMap.FindAction("Look");
        interact = playerControlMap.FindAction("Interact");

        move.performed += OnMove;
        move.canceled += OnMove;
        look.performed += OnLook;
        look.canceled += OnLook;
        interact.started += OnInteractBegin;
        interact.performed += OnInteract;
        movement = Vector3.zero;

        character = GetComponent<CharacterController>();
        sprite = GetComponent<SpriteRenderer>();
        vcamFollow = ((CinemachineVirtualCamera)
            Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera)
            .GetCinemachineComponent<CinemachineTransposer>();
        baseOffset = vcamFollow.m_FollowOffset;
        newOffset = baseOffset;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        prompt = transform.Find("Prompt").gameObject;
        promptProgress = prompt.GetComponentInChildren<Animator>();
        prompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        movement = transform.right * moveInput.x;
        movement += transform.forward * moveInput.y;
        if (!character.isGrounded)
        {
            movement += Physics.gravity;
        }
        character.Move(movement * moveSpeed * Time.deltaTime);
        transform.Rotate(0, Mathf.Lerp(0, lookInput.x * rotSpeed,
                                       Time.deltaTime), 0);
        newOffset = vcamFollow.m_FollowOffset + (Vector3.up * -lookInput.y * Time.deltaTime);
        vcamFollow.m_FollowOffset = Vector3.RotateTowards(
            vcamFollow.m_FollowOffset,
            newOffset,
            Time.deltaTime,
            Time.deltaTime
        );
    }

    void FixedUpdate()
    {
        raycastTime += Time.deltaTime;
        if (raycastTime >= raycastInterval)
        {
            raycastTime = 0;
            canInteract = Physics.Raycast(
                transform.position,
                vcamFollow.transform.forward,
                out raycastResult,
                5,
                1 << 3,
                QueryTriggerInteraction.UseGlobal
            );
            prompt.SetActive(canInteract);
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput.x < 0)
        {
            sprite.flipX = false;
        }
        else if (moveInput.x > 0)
        {
            sprite.flipX = true;
        }
    }

    void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void OnInteractBegin(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            promptProgress.SetTrigger("Hold");
        }
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            Debug.Log(raycastResult.transform.name);
            raycastResult.transform.GetComponent<IInteractable>()?.Interact();
            prompt.SetActive(false);
        }
    }


}
