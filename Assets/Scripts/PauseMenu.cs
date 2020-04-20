using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameObject menu;
    public MenuFunctions menuFunctions;
    // Start is called before the first frame update
    void Start()
    {
        menu = this.transform.Find("PausePanel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);
                menuFunctions.ResumeGame();
            }
            else
            {
                menu.SetActive(true);
                menuFunctions.PauseGame();
            }
        }
    }
}
