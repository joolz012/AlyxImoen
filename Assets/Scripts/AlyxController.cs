using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlyxController : MonoBehaviour
{
    public Animator animator;
    public float wait;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Animate()
    {
        while (true)
        {
            animator.SetBool("Move", false);
            yield return new WaitForSeconds(wait);
            animator.SetBool("Move", true);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
