using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [Header("Player Setting")]
    private Rigidbody rb;
    public float walkspeed, runspeed, jumppower, fallspeed, airMultiplier, HitPoints = 100f;
    public Transform PlayerOrientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    bool grounded = true, aerialboost = true, TPSMode = true, AimMode = false;
    public Animator anim;
    public CameraLogic camLogic;

    [Header("SFX")]
    public AudioClip ShootAudio;
    public AudioClip StepAudio;
    AudioSource PlayerAudio;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        PlayerAudio = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        
        Movement();
        AimModeAdjuster();
        ShootLogic();

        if (HitPoints > 0)
            {
                Jump();
            }

        if (Input.GetKey(KeyCode.F))
            {
                PlayerGetHit(100f);
            }
        
    }

    public void step()
    {
        Debug.Log("step");
        PlayerAudio.clip = StepAudio;
        PlayerAudio.Play();
    }

    private void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = PlayerOrientation.forward * verticalInput + PlayerOrientation.right * horizontalInput;

        if (grounded && moveDirection != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                rb.AddForce(moveDirection.normalized * runspeed * 10f, ForceMode.Force);
            }
            else
            {
                anim.SetBool("Walk", true);
                anim.SetBool("Run", false);
                rb.AddForce(moveDirection.normalized * walkspeed * 10f, ForceMode.Force);
            }
        }   
        else
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
        }
    }    

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumppower, ForceMode.Impulse);
            grounded = false;
            anim.SetBool("Jump", true);
        }
        else if (!grounded)
        {
            rb.AddForce(Vector3.down * fallspeed * rb.mass, ForceMode.Force);
            if (aerialboost)
            {
                rb.AddForce(moveDirection.normalized * walkspeed * 10f * airMultiplier, ForceMode.Impulse);
                aerialboost = false;
            }    
        }
    }    

    public void groundedchanger()
    {
        grounded = true;
        aerialboost = true;
        anim.SetBool("Jump", false);
    }

    public void AimModeAdjuster()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("mouse1");
            if (AimMode)
            {
                TPSMode = true;
                AimMode = false;
                anim.SetBool("AimMode", false);
            }
            else if (TPSMode)
            {
                TPSMode = false;
                AimMode = true;
                anim.SetBool("AimMode", true);
            }
            camLogic.CameraModeChanger(TPSMode, AimMode);
        }
    }

    private void ShootLogic()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerAudio.clip = ShootAudio;
            PlayerAudio.Play();
            if (moveDirection.normalized != Vector3.zero)
            {
                anim.SetBool("WalkShoot", true);
                anim.SetBool("IdleShoot", false);
            }    
            else
            {
                anim.SetBool("IdleShoot", true);
                anim.SetBool("WalkShoot", false);
            }
        }
        else
        {
            anim.SetBool("WalkShoot", false);
            anim.SetBool("IdleShoot", false);
        }
    }

    public void PlayerGetHit(float damage)
    {
        Debug.Log("Player Receive Damage - " + damage);
        HitPoints = HitPoints - damage;

        if (HitPoints <= 0f)
        {
            anim.SetBool("Death", true);
        }    
    }
}
