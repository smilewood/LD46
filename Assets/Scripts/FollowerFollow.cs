using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowerFollow : MonoBehaviour
{
    public GameObject player;
    public HealthbarController Hitbar;
    private Transform myTransform;
    private Vector3 targetVelocity;
    private Vector3 faceDirection;
    private float currentHealth;
    private AudioSource deathSource;

    public float rubberBandDistance;
    public float rubberBandForce;

    public GameObject GameOverPopup;
    public MenuFunctions menuFuncs;

    [Header("Health")]
    public float MaxHealth;
    public float HealthLossPerHit;
    public float HealthGainMult;

    public int TimesHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = this.GetComponent<Transform>();
        //HitsText.text = string.Format("Times Hit: {0}", TimesHit);
        currentHealth = MaxHealth;
        Hitbar.SetHealthbar(currentHealth, MaxHealth);
        deathSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector3.zero;
        float distance = Vector3.Distance(myTransform.position, player.transform.position);
        
        faceDirection = (player.transform.position - myTransform.position) / distance;

        targetVelocity = faceDirection * ((distance*distance)-rubberBandDistance) * rubberBandForce;
    }

    private void FixedUpdate()
    {
        Vector3 deltaPosition = targetVelocity * Time.deltaTime;
        this.transform.rotation = Quaternion.LookRotation(faceDirection, Vector3.up);
        myTransform.position = myTransform.position + deltaPosition;
    }
    bool dieing = false;
    private void OnCollisionEnter( Collision collision )
    {
        if(collision.gameObject.tag == "DamageEntity")
        {
            ++TimesHit;
            
            Debug.Log("Life Lost");
            currentHealth -= HealthLossPerHit;
            if (currentHealth <= 0 && !dieing)
            {
                //Game Over
                dieing = true;
                Debug.Log("Game Over");
                StartCoroutine(GameOver());
            }
            Hitbar.SetHealthbar(currentHealth, MaxHealth);
        }
    }

    private IEnumerator GameOver()
    {
        deathSource.Play();
        Instantiate(Resources.Load("EnemyDeathEffect"), player.transform.position, Quaternion.identity);
        Instantiate(Resources.Load("EnemyDeathEffect"), player.transform.position, Quaternion.identity);
        Instantiate(Resources.Load("EnemyDeathEffect"), player.transform.position, Quaternion.identity);
        player.SetActive(false);
        
        yield return new WaitUntil(() => { return !deathSource.isPlaying; });
        GameOverPopup.SetActive(true);
        menuFuncs.PauseGame();
    }

    public bool AddHealth(float ammount )
    {
        currentHealth = Mathf.Min(MaxHealth, currentHealth + (ammount*HealthGainMult));
        Hitbar.SetHealthbar(currentHealth, MaxHealth);
        return currentHealth == MaxHealth;
    }
}
