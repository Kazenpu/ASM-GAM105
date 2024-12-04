using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Audio;

public class Interact : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject winCanvas;
    private int score = 0;

    public AudioClip chest;
    private AudioSource audioSource;
    void Start()
    {   

        winCanvas.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ChestCount();
    }
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Chest"))
        {
            audioSource.PlayOneShot(chest);
            score += 1;
            ChestCount();

            if (score == 4)
            {
                ActiveWinCanvas();
            }
        }
    }
    void ChestCount()
    {
        scoreText.text = "" + score.ToString();
    }
    void ActiveWinCanvas()
    {
        winCanvas.SetActive(true);
        Time.timeScale = 0;
    }
}
