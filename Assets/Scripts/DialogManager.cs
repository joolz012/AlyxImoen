using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public AudioClip[] manClips;
    public AudioClip[] womanClips;
    public int randomAudio;
    public GameObject playerCamera, defaultCamera;

    [Header("Text")]
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI nameText;
    public float textSpeed;
    public string[] characterNames;
    public string[] lines;

    [Header("Image")]
    public Texture2D[] images;
    public RawImage rawImage;

    public GameObject player, attackManager, controls;
    public GameObject dialogCanvas;
    public GameObject[] highlights;
    public bool moving;
    private bool cantClick = true;

    public int index;


    void Start()
    {
        controls.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        player.GetComponent<PlayerController>().enabled = false;
        attackManager.GetComponent<AttackManager>().enabled = false;
        dialogCanvas.SetActive(false);
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        dialogCanvas.SetActive(true);
        textComponent.text = string.Empty;
        nameText.text = string.Empty;
        StartDialogue();
        cantClick = false;
        yield break;
    }

    void Update()
    {
        if (!cantClick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (textComponent.text == lines[index])
                {
                    if(index == 1)
                    {
                        audioSource.PlayOneShot(clips[2]);
                        NextLine();
                    }
                    else if(index == 3)
                    {
                        audioSource.PlayOneShot(clips[0]);
                        NextLine();
                    }
                    else if (index == 6)
                    {
                        audioSource.volume = 0.3f;
                        audioSource.PlayOneShot(clips[1]);
                        NextLine();
                    }
                    else if (index == 7)
                    {
                        if (!moving)
                        {
                            dialogCanvas.SetActive(false);
                            attackManager.GetComponent<AttackManager>().enabled = true;
                            StopAllCoroutines();
                            StartCoroutine(CountdownPlay());
                        }
                    }
                    else if (index == 13)
                    {
                        audioSource.PlayOneShot(clips[1]);
                        NextLine();
                    }
                    else if (index == 14)
                    {
                        SceneManager.LoadScene("Credits");
                    }
                    else
                    {
                        randomAudio = Random.Range(0, manClips.Length);
                        audioSource.PlayOneShot(manClips[randomAudio]);
                        NextLine();
                    }
                }
                else if (!moving)
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
    }

    public void DisableControls()
    {
        player.GetComponent<PlayerController>().enabled = false;
        controls.SetActive(false);
    }

    void SfxContinue()
    {
        randomAudio = Random.Range(0, manClips.Length);
        audioSource.PlayOneShot(manClips[randomAudio]);
    }

    public void DelaySound()
    {
        Invoke(nameof(SfxContinue), 2);
    }

    void StartDialogue()
    {
        randomAudio = Random.Range(0, manClips.Length);
        audioSource.PlayOneShot(manClips[randomAudio]);
        index = 0;
        StartCoroutine(TypeLine());
        UpdateImage();
        UpdateName();
    }
    IEnumerator CountdownPlay()
    {
        yield return new WaitForSeconds(5f);
        playerCamera.SetActive(true);
        defaultCamera.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;
        controls.SetActive(true);
    }
    public IEnumerator CountdownBday()
    {
        dialogCanvas.SetActive(false);
        yield return new WaitForSeconds(2f); 
        DisableControls();
        dialogCanvas.SetActive(true);
        NextLine();
        moving = false;
    }
    public IEnumerator Countdown()
    {
        Debug.Log("Countdown");
        dialogCanvas.SetActive(false);
        yield return new WaitForSeconds(3);
        dialogCanvas.SetActive(true);
        NextLine();
        moving = false;
    }

    public void HighlightsDisable()
    {
        foreach (GameObject images in highlights)
        {
            images.SetActive(false);
        }
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
            UpdateImage();
            UpdateName();
        }
        else
        {
            index = 0; // Restart dialogue loop
            dialogCanvas.SetActive(false);
        }
    }

    void UpdateName()
    {
        if (index < characterNames.Length)
        {
            nameText.text = characterNames[index];
        }
    }

    void UpdateImage()
    {
        if (index < images.Length)
        {
            rawImage.texture = images[index];
        }
    }


}
