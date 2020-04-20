using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTowordPlayer : ShooterComponent
{
    public GameObject PlayerGO;
    public GameObject ShotPrefab;
    public float ShotDelay;
    public float InitialCooldown;
    public float ShotSpeed;
    public bool playSound;

    private Transform TurretTransform;
    private AudioSource shootSoundPlayer;

    private void OnEnable()
    {
        if (playSound)
            shootSoundPlayer = this.GetComponent<AudioSource>();

        

        StartCoroutine(Shoot());
        TurretTransform = this.transform.FirstChildOrDefault(( Transform child ) => { return child.name == "Turret"; });
        
    }

    private void Start()
    {
        if (PlayerGO is null)
        {
            PlayerGO = GameObject.Find("Player");
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(InitialCooldown);
        while (true)
        {
            GameObject newShot = Instantiate(ShotPrefab, this.transform.position, this.transform.rotation);
            Vector3 targetDir = (PlayerGO.transform.position - this.transform.position) / Vector3.Distance(this.transform.position, PlayerGO.transform.position);
            newShot.GetComponent<BulletControl>().Initialize(targetDir, ShotSpeed);

            if(!(TurretTransform is null))
                TurretTransform.rotation = Quaternion.LookRotation(targetDir, Vector3.up);

            shootSoundPlayer?.Play();
            yield return new WaitForSeconds(ShotDelay);
        }
    }
}
