using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Line,
    SwoopRight,
    SwoopLeft,
    TowordsPlayer
}

public class EnemyMovementController : MonoBehaviour
{
    public MovementType movement;
    public float speed;
    public float startDelay;
    public float swoopSpeed;

    public Vector3 targetVelocity;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        EnemySpawnController.Instance.DelayMyStart(this.gameObject, startDelay, onStart);
    }

    private void onStart()
    {
        foreach (ShooterComponent shooter in this.transform.GetComponentsInChildren<ShooterComponent>())
        {
            shooter.enabled = true;
        }
        if(movement == MovementType.TowordsPlayer)
        {
            GameObject PlayerGO = GameObject.Find("Player");
            Vector3 targetDir = (PlayerGO.transform.position - this.transform.position) / Vector3.Distance(this.transform.position, PlayerGO.transform.position);
            this.transform.rotation = Quaternion.LookRotation(targetDir, Vector3.up);
        }
    }
    

    void Update()
    {
        targetVelocity = Vector3.zero;
        switch (movement)
        {
            case MovementType.TowordsPlayer:
            case MovementType.Line:
            {
                targetVelocity = this.transform.forward * speed;
                break;
            }
            case MovementType.SwoopRight:
            case MovementType.SwoopLeft:
            {
                targetVelocity = (Quaternion.Euler(0, swoopSpeed, 0) * this.transform.forward) * speed;
                break;
            }
        }

        Vector3 viewPoint = mainCamera.WorldToViewportPoint(this.transform.position);
        if (Math.Max(viewPoint.x, viewPoint.y) > 1.2f || Math.Min(viewPoint.x, viewPoint.y) < -.2f)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (targetVelocity == Vector3.zero)
            return;
        Vector3 deltaPosition = targetVelocity * Time.deltaTime;
        this.transform.rotation = Quaternion.LookRotation(targetVelocity.normalized, Vector3.up);
        this.transform.position = this.transform.position + deltaPosition;
    }
}
