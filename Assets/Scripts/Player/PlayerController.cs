using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;

    #region Variables: Movement
    [SerializeField] private float _moveSpeedBase;
    [SerializeField] private float _turnSpeed = 0.05f;

    public bool canMove  = true;
    private Vector2 _moveInput;
    private Vector3 _currentDirection;
    private float _currentVelocity;
    #endregion

    #region Variables: Gravity
    private float GRAVITY = -9.8f;
    [SerializeField] private float gravityMultiplyer;
    private float _velocity;
    #endregion

    private bool IsGrounded() => _characterController.isGrounded;

    #region Singleton
    public static PlayerController Instance;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    #endregion
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void LockPlayer()
    {
        this.canMove = false;
    }

    public void UnlockPlayer()
    {
        this.canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ApplyGravity();
        ApplyCharacterMovement();
        ApplyCharacterRotation();
    }

    private void ApplyCharacterMovement()
    {
        if (this.canMove)
        {
            _characterController.Move(
                _moveSpeedBase
                * Time.deltaTime
                * _currentDirection
            );
        }
    }

    private void ApplyCharacterRotation()
    {
        if (this.canMove)
        {
            if (_moveInput.sqrMagnitude == 0) return;

            float targetAngle = Mathf.Atan2(_currentDirection.x, _currentDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, _turnSpeed);

            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += GRAVITY * gravityMultiplyer * Time.deltaTime;
        }

        _currentDirection.y = _velocity;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _currentDirection = new Vector3(_moveInput.x, 0.0f, _moveInput.y);
    }
}
