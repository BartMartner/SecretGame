using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Damagable
{
    public static Player instance;

    [Header ("Player")]
    public LayerMask playerLayerMask;

    public GameObject cameraFocus;
    public float acceleration;
    public float maxVelocity = 10;
    private float _defaultAcceleration;
    private float _defaultMaxVelocity;
    private ProjectileStats _projectileStats;
    public SpriteRenderer playerRenderer;

    private Animator _animator;
    new public Rigidbody2D rigidbody2D;
    new public BoxCollider2D collider2D;

    private bool _disableMovement;

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
    private float _jumpTimeLimit = 0.45f;
    private float _maxVelocityY = 12.5f;

    private bool _looking;

    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);

    private bool _isHit;

    private Footsteps _footsteps;

    private float _xAxis;
    private float _yAxis;

    [Header("Jumping")]
    public LayerMask groundLayer;
    public AudioClip[] jumpSounds;
    public float jumpPower = 1.5f;
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

    [Header("Shooting")]
    public AudioClip[] attackSounds;
    public float aiming;
    public GameObject shootPoint;
    public GameObject shootPointUp;
    public GameObject shootPointDown;
    public bool attacking;

    [Header("Bombs")]
    public int currentBombs;
    public int maxBombs;

    [Header("Pogo")]
    public bool hasPogo;
    public bool pogo;

    [Header ("MorphBall")]
    public bool hasMorphBall;
    public bool morphBall;
    public PhysicsMaterial2D bounceBall;
    private Vector2 _originalSize;
    private Vector2 _ballBoundsSize;

    [Header("Suits")]
    public bool hasPowerSuit;
    public bool hasColdSuit;
    private Dictionary<string,Sprite> coldSuitSprites;
    private Dictionary<string, Sprite> powerSuitSprites;

    [Header("Player Sounds")]
    public AudioClip[] landingSounds;

    protected override void Awake()
    {
        instance = this;

        base.Awake();

        playerLayerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
        deathTime = 1.75f;

        gameObject.tag = "Player";
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        _halfHeight = collider2D.bounds.extents.y;
        _halfWidth = collider2D.bounds.size.x;
        //_leftOffset = Vector3.left * _halfWidth * 0.5f;
        //_rightOffset = Vector3.right * _halfWidth * 0.5f;

        _originalSize = collider2D.size;
        _ballBoundsSize = new Vector2(0.5f, 0.5f);

        _animator = playerRenderer.GetComponentInChildren<Animator>();
        _defaultAcceleration = acceleration;
        _defaultMaxVelocity = maxVelocity;

        _footsteps = GetComponentInChildren<Footsteps>();

        if (!groundedCheck)
        {
            groundedCheck = GetComponent<GroundedCheck>();
        }

        RefreshPowerUps();

        var coldSuit = Resources.LoadAll<Sprite>("ColdSuit");
        coldSuitSprites = new Dictionary<string, Sprite>();
        foreach (var sprite in coldSuit)
        {
            coldSuitSprites.Add(sprite.name, sprite);
        }


        var powerSuit = Resources.LoadAll<Sprite>("PowerSuit");
        powerSuitSprites = new Dictionary<string, Sprite>();
        foreach (var sprite in powerSuit)
        {
            powerSuitSprites.Add(sprite.name, sprite);
        }
    }

    private void FixedUpdate()
    {
        if (state != DamagableState.Alive)
        {
            return;
        }

        groundedCheck.UpdateRaycasts();

        if (_disableMovement)
        {
            UpdateAnimator();
            return;
        }

        var velocity = rigidbody2D.velocity;

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

            if (groundedCheck.onGround && _jumpHeld && _canJump && !attacking && ToggleMorphball(false))
            {
                if (jumpSounds.Length > 0)
                {
                    audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
                }
                
                rigidbody2D.gravityScale = 0;
                _canJump = false;
                _jumping = true;
                _jumpTimer = 0;
                groundedCheck.groundFrames = 0;
                velocity.y = 0;
            }
        }
        #endregion

        if(pogo && groundedCheck.onGround)
        {
            velocity.y += 3;
        }

        rigidbody2D.velocity = velocity;
    }

    protected override void Update()
    {
        base.Update();
        UpdateAnimator();
    }

    protected override void UpdateAlive()
    {
        base.UpdateAlive();

        _xAxis = Input.GetAxis("Horizontal");
        _yAxis = Input.GetAxis("Vertical");

        #region Looking
        if (Mathf.Abs(_yAxis) > 0.25f && Mathf.Abs(_xAxis) < 0.2f)
        {
            aiming = _yAxis;
            _looking = groundedCheck.onGround && !pogo && !morphBall && !attacking;

            if (_looking)
            {
                var camPos = cameraFocus.transform.localPosition;
                camPos.y = Mathf.Clamp(camPos.y + Time.deltaTime * Mathf.Sign(_yAxis), -1, 1);
                cameraFocus.transform.localPosition = camPos;
            }
        }
        else
        {
            aiming = 0;
            _looking = false;
        }
        #endregion

        if (!attacking && Input.GetButtonDown("Bomb"))
        {
            Debug.Log("Bomb Pressed");
            if (currentBombs > 0)
            {
                currentBombs--;
                ProjectileManager.instance.SpawnBomb(transform.position);
            }
        }

        if (!attacking && hasPogo && Input.GetButtonDown("Pogo"))
        {
            pogo = !pogo;
            ToggleMorphball(false);
        }

        if (!attacking && !morphBall && Input.GetButtonDown("Attack"))
        {
            if (ToggleMorphball(false))
            {
                StartCoroutine(Attack());
            }
        }

        if (!attacking && hasMorphBall && Input.GetButtonDown("MorphBall"))
        {
            ToggleMorphball(!morphBall);
        }
    }

    public void LateUpdate()
    {
        if (hasPowerSuit)
        {
            playerRenderer.sprite = powerSuitSprites[playerRenderer.sprite.name];
        }
        else if (hasColdSuit)
        {
            playerRenderer.sprite = coldSuitSprites[playerRenderer.sprite.name];
        }
    }

    public bool ToggleMorphball(bool value)
    {
        if (morphBall != value)
        {
            if(value == false)
            {
                var hit = Physics2D.Raycast(transform.position, Vector3.up, 0.75f, groundLayer);
                if(hit.collider)
                {
                    return false;
                }
            }

            morphBall = value;

            if(morphBall)
            {
                pogo = false;
                collider2D.sharedMaterial = bounceBall;
                collider2D.size = _ballBoundsSize;
                transform.position += Vector3.down * (_originalSize.y - _ballBoundsSize.y) * 0.5f;
            }
            else
            {
                collider2D.sharedMaterial = null;
                collider2D.size = _originalSize;
                transform.position += Vector3.up * (_originalSize.y - _ballBoundsSize.y) * 0.5f;
            }
        }

        return true;
    }

    public void UpdateAnimator()
    {        
        _animator.SetBool("Moving", _xAxis != 0);
        _animator.SetBool("Grounded", groundedCheck.onGround);
        _animator.SetBool("Shooting", attacking);
        _animator.SetFloat("VelocityY", rigidbody2D.velocity.y);
        _animator.SetBool("Pogo", pogo);
        _animator.SetBool("MorphBall", morphBall);
        _animator.SetFloat("Aiming", aiming);
        _animator.SetBool("LookingUp", _looking && aiming > 0);
        _animator.SetBool("LookingDown", _looking && aiming < 0);
    }

    private IEnumerator Attack()
    {
        pogo = false;
        attacking = true;
        if (aiming == 0)
        {
            ProjectileManager.instance.Shoot(_projectileStats, shootPoint.transform.position, transform.right);
        }
        else if(aiming > 0)
        {
            ProjectileManager.instance.Shoot(_projectileStats, shootPointUp.transform.position, transform.up);
        }
        else if (!groundedCheck.onGround)
        {
            ProjectileManager.instance.Shoot(_projectileStats, shootPointDown.transform.position, -transform.up);
        }
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

    public void OnDestroy()
    {
        instance = null;
    }

    public override bool Hurt(float damage, GameObject source = null, DamageType damageType = DamageType.Generic)
    {
        if (_aegisActive || state != DamagableState.Alive)
        {
            return false;
        }

        var result = base.Hurt(damage, source, damageType);

        if (result && health > 0)
        {
            var direction = transform.position.x > source.transform.position.x ? 1 : -1;
            Knockback(direction, 0.25f, 0.75f);
            StartCoroutine(OnHit());
        }

        return result;
    }

    public override void Die()
    {
        base.Die();
        animator.SetBool("Dead", true);
        cameraFocus.transform.parent = null;
        collider2D.enabled = false;
        rigidbody2D.velocity = Vector3.up * 5 + Vector3.right * 1.3f;
    }

    public void RefreshPowerUps()
    {
        _projectileStats = Constants.GreenBolts;
        bool hasPurple = false;
        foreach (var powerUp in SaveGameManager.instance.saveGameData.powerUpsCollected)
        {
            switch (powerUp)
            {
                case PowerUpID.PogoStick:
                    hasPogo = true;
                    break;
                case PowerUpID.MaruMari:
                    hasMorphBall = true;
                    break;
                case PowerUpID.PurpleLazer:
                    hasPurple = true;
                    _projectileStats = Constants.PurpleBolts;
                    break;
                case PowerUpID.RedLazer:
                    if (!hasPurple)
                    {
                        _projectileStats = Constants.RedBolts;
                    }
                    break;
            }
        }
    }
}