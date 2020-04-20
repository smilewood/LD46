using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private GameObject player;
    public AudioSource charge;
    private enum BossState
    {
        target, charge, fire
    }
    private BossState state;

    private float stateTimer;

    public float chargeTime;
    public float lockonTime;
    public float FireTime;
    public LaserBeam beam;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        state = BossState.target;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case (BossState.target):
            {
                float distance = Vector3.Distance(this.transform.position, player.transform.position);
                Vector3 faceDirection = (player.transform.position - this.transform.position) / distance;
                this.transform.rotation = Quaternion.LookRotation(faceDirection, Vector3.up);

                if(stateTimer > lockonTime)
                {
                    state = BossState.charge;
                    stateTimer = 0;
                    charge.Play();
                }

                break;
            }
            case BossState.charge:
            {
                if(stateTimer > chargeTime)
                {
                    state = BossState.fire;
                    stateTimer = 0;
                    beam.StartFiring(FireTime);
                }
                break;
            }
            case BossState.fire:
            {
                if(stateTimer > FireTime)
                {
                    stateTimer = 0;
                    state = BossState.target;
                }
                break;
            }
        }
        stateTimer += Time.deltaTime;
    }

    private void FacePlayer()
    {
       
    }
}
