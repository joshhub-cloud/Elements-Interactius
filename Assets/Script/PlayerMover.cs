using System.Collections;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public bool canMove = true;

    public Vector3 externalMoveSpeed;

    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private bool Grounded;

    [SerializeField] private AudioSource walk;

    private float _maxFallSpeed = 20f;
    private float _currentFallSpeed = 0f;

    private CharacterController _controller;
 public Vector3 _moveDirection = Vector3.zero;
    public float _vertical;
    public float _horizontal;

    public bool inWater;

    private float _coyoteTime = 0.4f;
    private float _jumpingTime = 1.40f;
    private float _timeSinceGrounded;
    private bool slope = false;

    public float input;
    private bool jumping;



    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        jumping = false;

    }

    private void FixedUpdate()
    {
        Grounded = _controller.isGrounded;

        _vertical = _joystick.Vertical;// + Input.GetAxis("Vertical");
        _horizontal = _joystick.Horizontal;
        input = Mathf.Abs(_vertical) + Mathf.Abs(_horizontal);

        //Vector3 cameraForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        //Vector3 moveDirection = _vertical * cameraForward + _horizontal * _cameraTransform.right;
        Vector3 moveDirection = _vertical * _cameraTransform.forward + _horizontal * _cameraTransform.right;
       // _moveDirection.x = moveDirection.x * _moveSpeed; _moveDirection.z = moveDirection.z * _moveSpeed;

        if(canMove)
        {
            _moveDirection.x = moveDirection.x * _moveSpeed;
            _moveDirection.z = moveDirection.z * _moveSpeed;

        }

        if (_controller.isGrounded)
        {
            if (_jumpingTime < 0) jumping = false;
            _currentFallSpeed = 0f;
            _timeSinceGrounded = 0f;

        }
        else
        {
            _timeSinceGrounded += Time.deltaTime;
            _moveDirection.x *= 0.7f; _moveDirection.z *= 0.7f;

            if (_moveDirection.y < -_maxFallSpeed) _moveDirection.y = -_maxFallSpeed;
            else
            {
                _currentFallSpeed += Time.deltaTime * 20f;
                _moveDirection.y -= _currentFallSpeed * Time.deltaTime;
            }
        }

        if ((_horizontal != 0 || _vertical != 0))
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(_moveDirection.x, 0, _moveDirection.z));
        }

        //Fall out with angle
        /*if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.normal.y < Mathf.Cos(50 * Mathf.Deg2Rad))
            {
                var slideDirection = new Vector3(hit.normal.x, -hit.normal.y, hit.normal.z);
                slope = true;
                _controller.Move(slideDirection * Time.fixedDeltaTime);
            }
            else
            {
                slope = false;
            }
        }*/
        if(!_animator.GetBool("Jumping"))
            _controller.Move(_moveDirection * Time.deltaTime + externalMoveSpeed * Time.deltaTime);

       // if (transform.position.y < -100) Control.Death();

    }
    private void Update()
    {
        if(canMove)
        {
            if(_jumpingTime > 0) _jumpingTime -= Time.deltaTime;
        }

        if (_jumpingTime > 0) _jumpingTime -= Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.Space) && !jumping) jump();
        if (Input.GetKeyDown(KeyCode.Space) && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Jump") jump();
        foreach (Touch t in Input.touches) if (t.tapCount == 2 && _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Jump") jump();
       //CheckPlayerTouch();
    }

    private void LateUpdate()
    {
        animate();
    }
    private void animate()
    {

        _animator.SetBool("Grounded", !jumping && (_controller.isGrounded || _timeSinceGrounded < _coyoteTime));
        //_animator.SetBool("InWater", inWater);

        if (_controller.isGrounded)
        {

            //if (_jumpingTime < 0) _animator.SetBool("Jumping", false);

            if (_horizontal != 0 || _vertical != 0)
            {
                _animator.speed = Mathf.Min(Mathf.Max(Mathf.Abs(_vertical), Mathf.Abs(_horizontal)) + 0.1f, 1);
                _animator.SetBool("Walking", true);
            }
            else
            {
                _animator.speed = 1;
                _animator.SetBool("Walking", false);
            }

        }
        else
        {
            _animator.speed = 1;
            //jumping = true;
        }
    }
    private void jump()
    {

        if (slope) return;
        //if (inWater) return;
        if (_timeSinceGrounded > _coyoteTime) return;
            StartCoroutine(JumpDelay());
        //GetComponent<Animator>().Play("Jumping", -1, 0);
        jumping = true;
        _timeSinceGrounded = 1;
        _currentFallSpeed = 3;
        //_jumpingTime = 0.5f;
        //AudioManager.PlayJump();
        _animator.SetBool("Jumping", true);
        StartCoroutine(StopJumping());
    }
    public IEnumerator StopJumping()
    {
        yield return new WaitForSeconds(0.7f);
        _animator.SetBool("Jumping", false);
        _moveDirection.y = _jumpForce;
        yield return new WaitForSeconds(_jumpingTime - 0.75f);
        jumping = false;

    }

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(0.8f);
        _moveDirection.y = _jumpForce;

    }
}