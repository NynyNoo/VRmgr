using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BallLevel : MonoBehaviour
{
    [SerializeField] GameObject RedBall;
    [SerializeField] GameObject BlueBall;
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] TextMeshProUGUI RedText;
    [SerializeField] TextMeshProUGUI BlueText;
    [SerializeField] TextMeshProUGUI MissText;
    Transform SpawnPointTransform;

    float timer=0;
    int seconds=0;
    List<GameObject> balls;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPointTransform= SpawnPoint.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        seconds = Convert.ToInt32(timer % 60);
        if (seconds > 3)
            SpawnBall();
    }
    void SpawnBall()
    {
        var random= UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            GameObject ball=Instantiate(RedBall, SpawnPointTransform.position, UnityEngine.Random.rotation, SpawnPointTransform);
            Vector3 velocity = new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(-6.0f, -1.0f));
            ball.GetComponent<Rigidbody>().velocity = velocity;
        }
        else
        {
            GameObject ball = Instantiate(BlueBall, SpawnPointTransform.position, UnityEngine.Random.rotation, SpawnPointTransform);
            Vector3 velocity = new Vector3(0.0f, 0.0f, UnityEngine.Random.Range(-6.0f, -1.0f));
            ball.GetComponent<Rigidbody>().velocity = velocity;
        }
        timer = 0;
    }
}
