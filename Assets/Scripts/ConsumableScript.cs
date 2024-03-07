using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableScript : MonoBehaviour
{
    public GameObject consumablesText;
    public AudioSource audioSource;
    public AudioClip clip;

    public GameObject image;
    private Collider2D col;
    


    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("JvLz"))
        {
            Debug.Log("Get");
            image.SetActive(false);
            col.enabled = false;
            StartCoroutine(VfxTrigger());
        }
    }

    IEnumerator VfxTrigger()
    {
        consumablesText.SetActive(true);
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(3f);
        consumablesText.SetActive(false);
        Destroy(gameObject);
        yield break;
    }
}
