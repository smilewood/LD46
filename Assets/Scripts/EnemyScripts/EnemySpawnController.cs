using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void RestartCallback();
public class EnemySpawnController : MonoBehaviour
{
    public List<GameObject> Waves;
    public int CurrentWave;
    public GameObject PlayerGO;
    public GameObject WinScreen;
    public MenuFunctions menuFunc;
    private static EnemySpawnController instance;
    public static EnemySpawnController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.childCount == 0)
        {
            SpawnNextWave();
        }
    }

    private void SpawnNextWave()
    {
        if(CurrentWave == Waves.Count)
        {
            Debug.Log("All Waves Complete");
            StartCoroutine(WinDelay());
            return;
        }
        Instantiate(Waves[CurrentWave], this.transform, true);

        foreach(ShootTowordPlayer shooter in this.transform.GetComponentsInChildren<ShootTowordPlayer>())
        {
            shooter.PlayerGO = PlayerGO;
        }

        ++CurrentWave;
    }

    private IEnumerator WinDelay()
    {
        yield return new WaitForSeconds(1f);
        WinScreen.SetActive(true);
        menuFunc.PauseGame();
    }

    public void DelayMyStart(GameObject target, float delayTime, RestartCallback onRestart )
    {
        target.SetActive(false);
        StartCoroutine(DelayStart(delayTime, target, onRestart));
    }

    IEnumerator DelayStart( float delay, GameObject target, RestartCallback onRestart )
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(true);
        onRestart();
    }
}
