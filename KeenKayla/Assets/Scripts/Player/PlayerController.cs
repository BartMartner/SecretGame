using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public GameObject cameraFocus;
    public ProjectileStats projectileStats;
    public float acceleration;
    public float maxVelocity = 10;
    private float _defaultAcceleration;
    private float _defaultMaxVelocity;
    public SpriteRenderer playerRenderer;
    public GameObject shootPoint;

    private Animator _animator;
    new public Rigidbody2D rigidbody2D;
    new public BoxCollider2D collider2D;
    private Vector3 _playerPosition;

    private AudioSource _audiosource;    

    public AudioClip[] attackSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] landingSounds;

    private bool _disableMovement;

    [HideInInspector]
    public GroundedCheck groundedCheck;

    private float _halfHeight;
    public float halfHeight
    {
        get
        {
            return _halfHeight;
        }
    }
    private float _halfWidth;
    private float _jumpTimer;
    private float _jumpTimeLimit = 0.375f;
    private float _maxVelocityY = 12.25f;

    public float jumpPower = 1.4f;
    private float _jumpCeiling = 3f;
    private float? _jumpCap;
    private bool _canJump = true;
    private bool _jumping;
    private bool _jumpHeld;
    private float _airControlMod;
    public bool jumping
    {
        get
        {
            return _jumping;
        }
    }

    private bool _looking;

    public bool attacking;

    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);

    public LayerMask groundLayer;    
    public LayerMask enemyLayer;

    private bool _canAct = true;
    private bool _isHit;

    private Footsteps _footsteps;

    private float _xAxis;
    private float _yAxis;

    private void Awake()
    {
        instance = this;

        gameObject.tag = "Player";
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        _halfHeight = collider2D.bounds.extents.y;
        _halfWidth = collider2D.bounds.size.x;
        //_leftOffset = Vector3.left * _halfWidth * 0.5f;
        //_rightOffset = Vector3.right * _halfWidth * 0.5f;
        
        _animator = playerRenderer.GetComponentInChildren<Animator>();
        _defaultAcceleration = acceleration;
        _defaultMaxVelocity = maxVelocity;

        _footsteps = GetComponentInChildren<Footsteps>();

        if (!groundedCheck)
        {
            groundedCheck = GetComponent<GroundedCheck>();
        }
    }

    private void Start()
    {
        _playerPosition = transform.position;
        _audiosource = Player.instance.audioSource;
    }


    private void FixedUpdate()
    {
        if (Player.instance.state != DamagableState.Alive)
        {
            return;
        }

        _playerPosition = transform.position;

        groundedCheck.UpdateRaycasts(_playerPosition);

        if (_disableMovement)
        {
            UpdateAnimator();
            return;
        }

        var velocity = rigidbody2D.velocity;
        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");

        #region Looking
        if (Mathf.Abs(_yAxis) > 0.25f && Mathf.Abs(_xAxis) < 0.2f && groundedCheck.onGround)
        {
            _looking = true;
            var camPos = cameraFocus.transform.localPosition;
            camPos.y = Mathf.Clamp(camPos.y + Time.deltaTime * Mathf.Sign(_yAxis), -1, 1);
            cameraFocus.transform.localPosition = camPos;
            if (_yAxis > 0)
            {
                _animator.SetBool("LookingUp", true);
                _animator.SetBool("LookingDown", false);
            }
            else
            {
                _animator.SetBool("LookingDown", true);
                _animator.SetBool("LookingUp", false);
            }
        }
        else
        {
            _looking = false;
            _animator.SetBool("LookingDown", false);
            _animator.SetBool("LookingUp", false);
        }
        #endregion

        #region Movement
        if (!groundedCheck.onGround)
        {
            if (_airControlMod > 0.45f)
            {
                _airControlMod -= Time.fixedDeltaTime * 0.5f;
            }
        }
        else
        {
            _airControlMod = 1f;

            if (_xAxis == 0)
            {
                velocity.x = 0;
            }
        }

        if (!_looking && !_isHit && (!attacking || !groundedCheck.onGround))
        {
            if (_xAxis != 0)
            {
                rigidbody2D.drag = 0;
                velocity.x = velocity.x + acceleration * _xAxis;

                if (velocity.x < 2 && velocity.x > -2)
                {
                    velocity.x = _xAxis > 0 ? 2f : -2f;
                }
                else
                {
                    var axisMax = groundedCheck.onGround ? Mathf.Abs(_xAxis) * maxVelocity : Mathf.Abs(_xAxis) * maxVelocity * _airControlMod;
                    velocity.x = Mathf.Clamp(velocity.x, -axisMax, axisMax);
                }
            }
            else if (velocity.x != 0)
            {
                velocity.x = Mathf.Lerp(velocity.x, 0, 0.5f);
            }
        }
        else
        {
            velocity.x = 0;
        }
        #endregion

        #region Facing
        if (!attacking)
        {
            if (_xAxis < 0 && transform.rotation != _flippedFacing)
            {
                transform.rotation = _flippedFacing;
            }
            else if (_xAxis > 0 && transform.rotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.identity;
            }
        }
        #endregion

        #region Jumping
        _jumpHeld = Input.GetButton("Jump");

        if (_jumping)
        {
            if (velocity.y < _maxVelocityY && _jumpTimer < _jumpTimeLimit && (_jumpHeld || _jumpTimer < _jumpTimeLimit * 0.5f))
            {
                _jumpTimer += Time.fixedDeltaTime;
                velocity.y += Mathf.Lerp(jumpPower, 0, _jumpTimer / _jumpTimeLimit);
            }
            else
            {
                rigidbody2D.gravityScale = 1;
                _jumping = false;
                Debug.Log("Jump Height: " + (transform.position.y - (_jumpCap - _jumpCeiling)));
            }
        }
        else
        {
            if (velocity.y > 0)
            {
                velocity.y -= Time.fixedDeltaTime * velocity.y * 3f;
            }

            if (!_canJump)
            {
                _canJump = groundedCheck.nearGround && !_jumpHeld;
            }

            if (groundedCheck.onGround && _jumpHeld && _canJump && !attacking)
            {
                if (jumpSounds.Length > 0)
                {
                    _audiosource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
                }

                _jumpCap = transform.position.y + _jumpCeiling;
                rigidbody2D.gravityScale = 0;
                _canJump = false;
                _jumping = true;
                _jumpTimer = 0;
                groundedCheck.groundFrames = 0;
                velocity.y = 0;
            }
        }
        #endregion

        rigidbody2D.velocity = velocity;

        UpdateAnimator();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            if (_canAct && !attacking)
            {
                StartCoroutine(Attack());
            }
        }
    }

    public void UpdateAnimator()
    {
        _animator.SetBool("Moving", _xAxis != 0);
        _animator.SetBool("Grounded", groundedCheck.onGround);
        _animator.SetBool("Shooting", attacking);
        _animator.SetFloat("VelocityY", rigidbody2D.velocity.y);
    }

    private IEnumerator Attack()
    {
        attacking = true;
        ProjectileManager.instance.Shoot(projectileStats, shootPoint.transform.position, transform.right);
        yield return new WaitForSeconds(0.25f);
        attacking = false;
    }

    public void DisableMovement()
    {        
        rigidbody2D.velocity = Vector3.zero;
        _xAxis = 0;
        _disableMovement = true;
    }

    public void EnableMovement()
    {
        _disableMovement = false;
    }

    public IEnumerator OnHit()
    {
        _isHit = true;
        yield return new WaitForSeconds(0.2f);
        _isHit = false;
    }

    public void OnDeath()
    {
        cameraFocus.transform.parent = null;
        collider2D.enabled = false;
        //rigidbody2D.gravityScale = 0;
        rigidbody2D.velocity = Vector3.up * 5 + Vector3.right * 1.3f;
        //rigidbody2D.isKinematic = true;
    }

    public void OnDestroy()
    {
        instance = null;
    }
}