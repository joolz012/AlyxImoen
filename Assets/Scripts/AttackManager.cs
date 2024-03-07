using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public Animator attackAnim;
    public GameObject pooInstantiate;
    public float instantiatePerSec;//asd
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Instantiate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Instantiate()
    {
        attackAnim.SetBool("Poo", true);
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            attackAnim.SetBool("Poo", false);
            Instantiate(pooInstantiate, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(instantiatePerSec);
            Debug.Log("Poo");
            attackAnim.SetBool("Poo", true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
