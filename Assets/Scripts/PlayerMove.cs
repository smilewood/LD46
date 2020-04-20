using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private Vector3 targetVelocity;
    private Vector3 velocity;
    private Vector3 targetFacing;
    private Transform playerBody;
    private Camera mainCamera;
    private AudioSource AbsorbSound;
    private LaserBeam laserControl;
    private FollowerFollow follower;


    public float MaxSpeed;
    public float downwordSpeed;
    public float rotationDampening;

    [Header("Charge")]
    public float charge;
    public float ChargeFromShots;
    public float chargeFromEnemies;
    public float dischargeSpeed;
    public Text chargeText;
    private bool firing;

    [Header("Edge Avoidance")]
    public Vector2 CameraBorder;
    public float EdgeForce;
    public float EdgePower;
    

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerBody = GetComponent<Transform>();
        AbsorbSound = GetComponent<AudioSource>();
        laserControl = GetComponentInChildren<LaserBeam>();
        chargeText.text = string.Format("Charge: {0:0.0}", charge);
        follower = GameObject.Find("Follower").GetComponent<FollowerFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;

        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");

        
        targetFacing = move;
        

        move.z = move.z > 0 ? move.z : move.z - downwordSpeed;

        targetVelocity = move * MaxSpeed;

        Vector3 viewpointPos = mainCamera.WorldToViewportPoint(playerBody.position);
        if(viewpointPos.x > 1 - CameraBorder.x)
        {
            targetVelocity.x = Mathf.Clamp(targetVelocity.x - Mathf.Pow((viewpointPos.x - (1 - CameraBorder.x)) + 1, EdgePower) * EdgeForce, -MaxSpeed, MaxSpeed);
        }
        if (viewpointPos.x < CameraBorder.x)
        {
            targetVelocity.x = Mathf.Clamp(targetVelocity.x + Mathf.Pow(Mathf.Abs((viewpointPos.x - (CameraBorder.x)) - 1), EdgePower) * EdgeForce, -MaxSpeed, MaxSpeed);
        }
        if (viewpointPos.y > 1 - CameraBorder.y)
        {
            targetVelocity.z = Mathf.Clamp(targetVelocity.z - Mathf.Pow((viewpointPos.y - (1 - CameraBorder.y)) + 1, EdgePower) * EdgeForce, -MaxSpeed, MaxSpeed);
        }
        if (viewpointPos.y < CameraBorder.y)
        {
            targetVelocity.z = Mathf.Clamp(targetVelocity.z + Mathf.Pow(Mathf.Abs((viewpointPos.y - (CameraBorder.y)) - 1), EdgePower) * EdgeForce, -MaxSpeed, MaxSpeed);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (laserControl.StartFiring(charge/dischargeSpeed))
            {
                Debug.Log("startedToFire");
                firing = true;
            }
            
        }
        if (Input.GetButton("Fire1") && firing)
        {
            if(charge > 0)
            {
                charge -= Time.deltaTime*dischargeSpeed;
            }
            if(charge <= 0)
            {
                charge = 0;
                firing = false;
            }

            chargeText.text = string.Format("Charge: {0:0.0}", charge);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            laserControl.EndFiring();
            firing = false;
        }
    }

    private void FixedUpdate()
    {
        velocity = targetVelocity;

        Vector3 deltaPosition = velocity * Time.deltaTime;

        Vector3 targetPosition = playerBody.position + deltaPosition;

        if (targetFacing != Vector3.zero)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(targetFacing.normalized, Vector3.up), Time.deltaTime * rotationDampening);
        }

        playerBody.position = targetPosition;
    }
    private void OnCollisionEnter( Collision collision )
    {
        if(collision.gameObject.tag == "DamageEntity")
        {
            AbsorbSound.Play();
            if (!firing)
            {
                if (collision.gameObject.name == "EnemyShotObject(Clone)")
                {
                    AddCharge(ChargeFromShots);
                }
                else
                {
                    AddCharge(chargeFromEnemies);
                }
                chargeText.text = string.Format("Charge: {0:0.0}", charge);
            }
        }
    }

    private void AddCharge(float ammount )
    {
        if (follower.AddHealth(ammount))
        {
            charge += ammount;
        }
    }
}
