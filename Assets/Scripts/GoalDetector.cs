using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI EventTextArea;
    bool isTextShowed= false;
    float timer = 0;
    private void Start()
    {
        EventTextArea.color = Color.red;
    }
    private void Update()
    {
        if (isTextShowed)
        {
            timer += Time.deltaTime;
            if (timer > 1.5f)
            {
                ResetText();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Soccer Ball(Clone)")
        {
            other.gameObject.GetComponent<Ball>().RemoveBall();
            ResetText();
            Warning();
        }
    }
    private void ResetText()
    {
        isTextShowed = false;
        EventTextArea.enabled = false;
        timer = 0;
    }
    private void Warning()
    {
        isTextShowed = true;
        EventTextArea.enabled = true;
    }
}
