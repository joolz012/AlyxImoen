using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VidTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator VidTimer()
    {
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene("MainMenuScene");
    }
}
