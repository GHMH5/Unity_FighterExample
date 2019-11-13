using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    private float HSpeed = 10f;
    private float moveHorizontal;

    [SerializeField]
    UIManager uiManager;

    //private bool facingRight = true;

    [SerializeField]
    float CurrentHealth;

    //Used for flipping Character Direction
    public static Vector3 theScale;

    private float punchTime;

    private float kickTime;

    public EnemyCharacter enemyCharacter;


    [SerializeField]
    Image EnergyIcon;

    [SerializeField]
    Image HealthIcon;

    public BoxCollider2D PlayerCollider;
    public Rigidbody2D PlayerRigidBody;
    public SpriteRenderer PlayerSprite;
    public Animator PlayerAnimator;

    public BoxCollider2D PunchCollider;

    public BoxCollider2D KickCollider;

    [SerializeField]
    float CurrentEnergy;


    public Transform groundCheck;
    public LayerMask whatIsGround;
    private bool grounded = false;
    private float groundRadius = 0.15f;
    private float jumpForce = 14f;

    // Use this for initialization
    void Start()
    {
        CurrentHealth = 100;

        PlayerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        PlayerAnimator.SetBool("ground", grounded);

        
    }


    void Update()
    {


        moveHorizontal = Input.GetAxis("Horizontal");

        if ((grounded) && Input.GetButtonDown("Jump"))
        {
            PlayerAnimator.SetBool("ground", false);

            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.y, jumpForce);
        }


        Vector2 movement = new Vector2(moveHorizontal, 0);

        GetComponent<Rigidbody2D>().velocity = new Vector2((moveHorizontal * HSpeed), GetComponent<Rigidbody2D>().velocity.y);

        //PlayerRigidBody.transform.Translate(movement * HSpeed);
        //HSpeed = movement.x;

        PlayerAnimator.SetFloat("HSpeed", Mathf.Abs(moveHorizontal * HSpeed));
        PlayerAnimator.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        if (Input.GetButtonDown("Punch") && (grounded))
        {
            Punch();

            

            
        }

        if (Input.GetButtonDown("Kick") && (grounded))
        {
            Kick();
        }

        //Prevents non-stop punching
        punchTime += 1 * Time.deltaTime;

        if (punchTime >= 2)
        {
            PunchCollider.enabled = false;
            punchTime = 0;
        }

        //Prevents non-stop kicking
        kickTime += 1 * Time.deltaTime;

        if (kickTime >= 2)
        {
            KickCollider.enabled = false;
            kickTime = 0;

            //PlayerAnimator.SetBool("ground", true);

        }

        if (Input.GetButton("Fire2"))
        {
            PlayerAnimator.SetBool("Sprint", true);
            HSpeed = 14f;
        }
        else
        {
            PlayerAnimator.SetBool("Sprint", false);
            HSpeed = 10f;
        }

        //Flipping direction character is facing based on players Input
        if (moveHorizontal > 0)
        {
            PlayerSprite.flipX = false;
        }
        else if (moveHorizontal < 0)
        {
            PlayerSprite.flipX = true;
        }

        if (CurrentEnergy <= 100)
        {
            UpdateMeter();
        }

        if (CurrentHealth <= 100)
        {
            UpdateHealth();
        }

    }

    public void UpdateMeter()
    {
        EnergyIcon.fillAmount = CurrentEnergy * .01f;


        if (CurrentEnergy < 100)
        {
            CurrentEnergy += 5f * Time.deltaTime;
        }
        else
        {
            CurrentEnergy = 100;
        }
    }

    public void UpdateHealth()
    {

        HealthIcon.fillAmount = CurrentHealth / 100;

        CheckHealth();
    }

    public void TakeDamage(float DamageToBeDealt)
    {
        CurrentHealth = CurrentHealth - DamageToBeDealt;

        UpdateHealth();


        Debug.Log(DamageToBeDealt);


    }



    void Punch()
    {

        if (CurrentEnergy >= 10)
        {
            PlayerAnimator.SetTrigger("Punch");

            CurrentEnergy = CurrentEnergy - 10;

            PunchCollider.enabled = true;

            punchTime = 0;
        }

    
    }

    void Kick()
    {
        if (CurrentEnergy >= 20)
        {

            PlayerAnimator.SetTrigger("Kick");

            CurrentEnergy = CurrentEnergy - 20;

         
            KickCollider.enabled = true;

            kickTime = 0;
        }
        
     
    }

    void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            gameObject.SetActive(false);

            uiManager.VictoryCondition("ENEMY CHARACTER WINS!");
        }
        
    }
}

