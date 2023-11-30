using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float runningSpeed = 4.0f;
    [SerializeField] private ParticleSystem salpicadura;

    private Rigidbody2D _rigidbody2d;
    private const float _distanceRaycast = 1.0f;
    private Animator _animatorPlayer;
    private bool _isRunning = false;
    private Vector3 startPosition;

    private readonly int _animIDisAlive = Animator.StringToHash("isAlive");
    private readonly int _animIDisHited = Animator.StringToHash("isHited");
    private readonly int _animIDisRunning = Animator.StringToHash("isRunning");
    private readonly int _animIDisGrounded = Animator.StringToHash("isGrounded");
    private readonly int _animIDisFalling = Animator.StringToHash("isFalling");

    public static PlayerController sharedInstance;
    public float distanceTravelled = 0;
    private int healthPlayer;
    private bool invulnerability;

    private AudioSource _audioSource;

	public bool canMove = true;
	[SerializeField] private Vector2 speedBounce;

    public Color defaultColor;
    public Color newColor;

    [SerializeField] private AudioSource _alarma;

    public void CollectHealth(int objectValue)
    {
        if (healthPlayer < 6)
        {
            healthPlayer += objectValue;
            UpdateGameCanvas.sharedInstance.AddHealth(healthPlayer - 1);
            //Debug.Log("Puntos de vida: " + healthPlayer);
        }
    }

    IEnumerator TirePlayer()
    {
        while (_animatorPlayer.GetBool(_animIDisAlive))
        {
            if (healthPlayer > 0)
            {
                jumpForce = 8f;
                this.GetComponent<SpriteRenderer>().color = defaultColor;
                _alarma.Stop();
                yield return new WaitForSeconds(5f);
                UpdateGameCanvas.sharedInstance.TakeHealth(healthPlayer - 1);
                healthPlayer--;
            } else 
            {
                yield return new WaitForSeconds(1f);
                runningSpeed = 4f;
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
                    if (_rigidbody2d.velocity.x < runningSpeed)
                    {
                        _rigidbody2d.velocity = new Vector2(runningSpeed, _rigidbody2d.velocity.y);
                    }
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
                if (_isRunning == false)
                {
                    _isRunning = true;
                    _animatorPlayer.SetBool(_animIDisRunning, _isRunning);
                    StartCoroutine("TirePlayer");
                }
                else
                {
                    if (isOnTheFloor())
                    {
                        Jump();
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
            _rigidbody2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool isOnTheFloor()
    {
        bool isOnTheFloor = false;
        if (Physics2D.Raycast(this.transform.position, Vector2.down, _distanceRaycast, groundLayerMask.value))
        {
            isOnTheFloor = true;
        }

        return isOnTheFloor;
    }

    public void KillPlayer()
    {
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
                Debug.Log("Puntos de vida restantes: " + healthPlayer);
                Invoke("DelayInvulnerability", 1.5f);
            }
        }
    }

    public void DelayInvulnerability()
    {
        invulnerability = false;
    }

    public void Ralentizar()
    {
        runningSpeed = 2.0f;
    }
    
    public void Acelerar()
    {
        runningSpeed = 4.0f;
    }
}
