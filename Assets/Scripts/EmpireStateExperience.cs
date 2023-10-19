using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EmpireStateExperience : MonoBehaviour
{

    public GameObject[] Empire;
    public float sec = 10f;
    private float countdownTime = 10f; // Initial countdown time in seconds

    private int prev = 0;
    private int current = 1;

    public bool isCesiumGeoreference = false;

    public float waitTime = 30f;
    private bool startExp = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        countdownTime = sec;

        for (int i = 1; i < Empire.Length; i++)
        {
            Empire[i].transform.localScale = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {


        waitTime -= Time.deltaTime;

        if (waitTime <= 0f)
        {
            startExp = true;
            
        }

        if (!startExp)
            return;


        countdownTime -= Time.deltaTime; // Subtract the time passed since the last frame

        // Check if the countdown has reached or gone below 0
        if (countdownTime <= 0f)
        {
            // Perform your action here when the countdown reaches 0
            Debug.Log("Action performed!");


            if (current < Empire.Length)
            {
                
                if (isCesiumGeoreference)
                {
                    
                    Empire[prev].transform.DOScale(Vector3.zero, 1f).SetEase(Ease.OutQuad);
                    
                }
                prev = current;
                Empire[current].transform.DOScale(Vector3.one, 1f).SetEase(Ease.InQuad);
                current++;
            }
            else
            {
                current = 0;
            }





            // Reset the countdown to 10 seconds
            countdownTime = sec;
        }
    }
}
