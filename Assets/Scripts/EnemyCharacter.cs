using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCharacter : MonoBehaviour
{
    public Transform target; //Transform to attempt to move toward each turn.

    [SerializeField]
    UIManager uiManager;

    public float HSpeed;

    private float movementSpeed = 0.5f;

    private bool facingRight = true;

    private float attackTime;

    public PlayerCharacter playerCharacter;

    public BoxCollider2D EnemyCollider;

    [SerializeField]
    float CurrentHealth;

    //Used for flipping Character Direction
    public static Vector3 theScale;

    private float punchTime;

    private float kickTime;

    public Rigidbody2D EnemyRigidBody2D;
    public SpriteRenderer EnemySprite;
    public Animator EnemyAnimator;

    public BoxCollider2D PunchCollider;

    public BoxCollider2D KickCollider;

    [SerializeField]
    float CurrentEnergy;

    [SerializeField]
    Image EnergyIcon;

    [SerializeField]
    Image HealthIcon;

    public Transform groundCheck;
    public LayerMask whatIsGround;
    private bool grounded = false;
    private float groundRadius = 0.15f;
    private float jumpForce = 14f;




    // Use this for initialization
    void Start()
    {
        playerCharacter = FindObjectOfType<PlayerCharacter>();
        CurrentHealth = 100;

        //Find the Player GameObject using it's tag and store a reference to its transform component.
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        EnemyAnimator.SetBool("ground", grounded);


        EnemyAnimator.SetFloat("HSpeed", Mathf.Abs(transform.position.x * HSpeed));
        EnemyAnimator.SetFloat("vSpeed", EnemyRigidBody2D.velocity.y);


        transform.position += (target.position - transform.position).normalized * movementSpeed * Time.deltaTime;

        attackTime += Time.deltaTime;

       


        if (attackTime >= Random.Range(1f, 2f))
        {
            float randomNumber = Random.Range(0, 1f);

            if (randomNumber >= 0.5)
            {
                Punch();

                punchTime = 0;
            }
            else
            {
                Kick();

                kickTime = 0;
            }

            attackTime = 0;

            
        }


        //Prevents non-stop punching
        punchTime += 1 * Time.deltaTime;

        if (punchTime >= 1)
        {
            PunchCollider.enabled = false;
            punchTime = 0;
        }

        if (kickTime <= 0 )
        {
            KickCollider.enabled = false;
            kickTime = 0;
        }

        //Prevents non-stop kicking
        kickTime += 1 * Time.deltaTime;

        //Flipping direction character is facing based on players Input
        if (playerCharacter.PlayerSprite.flipX == false)
        {
           
            EnemySprite.flipX = true;

         
        }
        else //Needs to be fixed because movementSpeed is never going to be less than 0
        {
            EnemySprite.flipX = false;

        
        }

        UpdateMeter();

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        HealthIcon.fillAmount = CurrentHealth * .01f;

        CheckHealth();
    }

    void Flip()
    {
        facingRight = !facingRight;
        theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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

    void Punch()
    {
        if (CurrentEnergy >= 10)
        {
            EnemyAnimator.SetTrigger("Punch");

            CurrentEnergy = CurrentEnergy - 10;

            PunchCollider.enabled = true;

            punchTime = 0;
        }

       
    }

    void Kick()
    {
        if (CurrentEnergy >= 20)
        {
            EnemyAnimator.SetTrigger("Kick");

            CurrentEnergy = CurrentEnergy - 20;

            KickCollider.enabled = true;

            kickTime = 0;
        }

        
    }

    public void TakeDamage(float DamageToBeDealt)
    {
        CurrentHealth = CurrentHealth - DamageToBeDealt;

        UpdateHealth();

       

    }


    void CheckHealth()
    {
        if (CurrentHealth <= 0)
        {
            gameObject.SetActive(false);

            uiManager.VictoryCondition("PLAYER CHARACTER WINS!");
        }
    }
}

