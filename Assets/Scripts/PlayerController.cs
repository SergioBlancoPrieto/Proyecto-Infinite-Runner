using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform floorController;
    [SerializeField] private Vector3 boxDimensions;
    [SerializeField] private float runningSpeed = 4.0f;
    [SerializeField] private float slowedSpeed = 4.0f;
    [SerializeField] private ParticleSystem salpicadura;

    private Rigidbody2D _rigidbody2d;
    private const float _distanceRaycast = 0f;
    private Animator _animatorPlayer;
    private bool _isRunning = false, saltoDoble = false, buff = false, _isOnTheFloor, _jump;
    private Vector3 startPosition;
    private float _currentRunningSpeed, _currentJumpForce;

    private readonly int _animIDisAlive = Animator.StringToHash("isAlive");
    private readonly int _animIDisHited = Animator.StringToHash("isHited");
    private readonly int _animIDisRunning = Animator.StringToHash("isRunning");
    private readonly int _animIDisGrounded = Animator.StringToHash("isGrounded");
    private readonly int _animIDisFalling = Animator.StringToHash("isFalling");

    public static PlayerController sharedInstance;
    public float distanceTravelled = 0, saltosExtra = 1, totalSaltos = 1;
    private int healthPlayer;
    private bool invulnerability = false;

    private AudioSource _audioSource;

	public bool canMove = true;
	[SerializeField] private Vector2 speedBounce;

    public Color defaultColor;
    public Color newColor;

    [SerializeField] private AudioSource _alarma;
    [SerializeField] private AudioSource _musicaBuff;

    public void CollectHealth(int objectValue)
    {
        if (healthPlayer < 6)
        {
            healthPlayer += objectValue;
            UpdateGameCanvas.sharedInstance.AddHealth(healthPlayer - 1);
        }
    }

    IEnumerator TirePlayer()
    {
        while (_animatorPlayer.GetBool(_animIDisAlive))
        {
            if (healthPlayer > 0)
            {
                _currentJumpForce = jumpForce;
                this.GetComponent<SpriteRenderer>().color = defaultColor;
                _alarma.Stop();
                yield return new WaitForSeconds(5f);
                UpdateGameCanvas.sharedInstance.TakeHealth(healthPlayer - 1);
                healthPlayer--;
            } else 
            {
                yield return new WaitForSeconds(1f);
                RestaurarSpeed();
                this.GetComponent<SpriteRenderer>().color = newColor;
                _alarma.Play();
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Pause"))
        {
            MenuPause.sharedInstance.Pausa();
        }
        //Solo corre si estamos en el estado inTheGame
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
			if (canMove) 
			{
				if (_isRunning)
                {
                    _rigidbody2d.velocity = new Vector2(_currentRunningSpeed, _rigidbody2d.velocity.y);
                }
			}
        }
        Movement();
    }

    private void Movement()
    {
        if (_jump)
        {
            if (_isOnTheFloor)
            {
                Jump();
            }
            else
            {
                if (saltoDoble && saltosExtra > 0)
                {
                    Jump();
                    saltosExtra--;
                }
            }
        }
    }

	public void Bounce(Vector2 pointHit)
	{
        if (invulnerability == false)
        {
		    _rigidbody2d.velocity = new Vector2(-Mathf.Abs(speedBounce.x * (pointHit.x - this.transform.position.x)), speedBounce.y);
        }
	}
    
    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(this);
        }
        else
        {
            sharedInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        startPosition = this.transform.position;
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animatorPlayer = GetComponent<Animator>();
        _animatorPlayer.SetBool(_animIDisAlive, true);
        _audioSource = GetComponent<AudioSource>();
    }
    
    // Start is called before the first frame update
    public void StartGame()
    {
        _animatorPlayer.SetBool(_animIDisAlive, true);
        this.transform.position = startPosition;
        distanceTravelled = 0;
        healthPlayer = 6;
    }

    // Update is called once per frame
    void Update()
    {
        //Solo salta si estamos en el estado inTheGame
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            _animatorPlayer.SetBool(_animIDisFalling, _rigidbody2d.velocity.y < 0.5f);
            _animatorPlayer.SetBool(_animIDisGrounded, isOnTheFloor());
            if (Input.GetButtonDown("Fire1")) 
            {
                if (_isOnTheFloor)
                {
                    saltosExtra = totalSaltos;
                }
                if (_isRunning == false)
                {
                    _isRunning = true;
                    _animatorPlayer.SetBool(_animIDisRunning, _isRunning);
                    StartCoroutine("TirePlayer");
                }
                else
                {
                    if (_isOnTheFloor || saltoDoble)
                    {
                        _jump = true;
                    }
                }
            
            }
        }
    }

    void Jump()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
        {
            _audioSource.Play();
            _rigidbody2d.velocity = new Vector2(0f, _currentJumpForce);
            _jump = false;
        }
    }

    bool isOnTheFloor()
    {
        _isOnTheFloor = Physics2D.OverlapBox(floorController.position, boxDimensions, 0f, groundLayerMask);

        return _isOnTheFloor;
    }

    public void KillPlayer()
    {
        _musicaBuff.Stop();
        _alarma.Stop();
        salpicadura.Play();
        GameManager.sharedInstance.GameOver();
        _animatorPlayer.SetBool(_animIDisAlive, false);
        SleepPlayer();
        if (PlayerPrefs.GetFloat("highscore", 0) < distanceTravelled)
        {
            PlayerPrefs.SetFloat("highscore", distanceTravelled);
        }
    }

    public void SleepPlayer()
    {
        this.GetComponent<Rigidbody2D>().Sleep();
    }

    public float GetDistanceTravelled()
    {
        distanceTravelled =
            Vector2.Distance(new Vector2(startPosition.x, 0), new Vector2(this.transform.position.x, 0));
        return distanceTravelled;
    }

    public void DamagePlayer()
    {
        if (invulnerability == false)
        {
            CinemachineCameraShake.sharedInstance.CameraShake(5, 5, 0.5f);
            if (healthPlayer > 0)
            {
                UpdateGameCanvas.sharedInstance.TakeHealth(healthPlayer - 1);
                healthPlayer--;
                invulnerability = true;
                Invoke("DelayInvulnerability", 1.5f);
            }
        }
    }

    public void DelayInvulnerability()
    {
        invulnerability = false;
    }

    public void RalentizarSpeed()
    {
        _currentRunningSpeed = slowedSpeed;
    }
    
    public void RestaurarSpeed()
    {
        _currentRunningSpeed = runningSpeed;
    }

    public void BuffPlayer()
    {
        StartCoroutine("DoBuff");
    }

    IEnumerator DoBuff()
    {
        var puntosBufoCogido = this.distanceTravelled;
        buff = true;
        while (this.distanceTravelled < (puntosBufoCogido + 50))
        {
            invulnerability = true;
            saltoDoble = true;
            _musicaBuff.Play();
            StartCoroutine(ChangeColor(puntosBufoCogido));
            yield return new WaitForSeconds(5f);
        }
        buff = false;
        invulnerability = false;
        saltoDoble = false;
        _musicaBuff.Stop();
    }

    IEnumerator ChangeColor(float puntos)
    {
        while (buff) 
        {
            this.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.5f);
        }
        this.GetComponent<SpriteRenderer>().color = defaultColor;
    }

    public bool IsInvulnerable()
    {
        return invulnerability;
    }

    public IEnumerator DesactivarCollision()
    {
        Physics2D.IgnoreLayerCollision(3, 7, true);
        Physics2D.IgnoreLayerCollision(3, 8, true);
        yield return new WaitForSeconds(5f);
        Physics2D.IgnoreLayerCollision(3, 7, false);
        Physics2D.IgnoreLayerCollision(3, 8, false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(floorController.position, boxDimensions);
    }
}
