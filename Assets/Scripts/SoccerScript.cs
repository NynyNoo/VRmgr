using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

public class SoccerScript : MonoBehaviour
{
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject GoalPoint;
    [SerializeField] TextMeshProUGUI ButtonText;

    public Vector3 minGoalBounds;
    public Vector3 maxGoalBounds;

    public Vector3 minSpawnBounds;
    public Vector3 maxSpawnBounds;

    public float ballSpeed = 500f;
    float timer = 0;
    int seconds = 0;

    bool gameRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        GetGoalArea();
        GetSpawningArea();
    }
    void Update()
    {
        if (gameRunning)
        {
            timer += Time.deltaTime;
            if (timer > 2.5)
                RespawnBall();
        }
    }
    void RespawnBall()
    {
        float randomSpawnX = UnityEngine.Random.Range(minSpawnBounds.x, maxSpawnBounds.x);
        float randomSpawnY = 0.1f;
        float randomSpawnZ = UnityEngine.Random.Range(minSpawnBounds.z, maxSpawnBounds.z);

        float randomTargetX = UnityEngine.Random.Range(minGoalBounds.x, maxGoalBounds.x);
        float randomTargetY = UnityEngine.Random.Range(minGoalBounds.y, maxGoalBounds.y);
        float randomTargetZ = UnityEngine.Random.Range(minGoalBounds.z, maxGoalBounds.z);

        Vector3 spawnPosition = new Vector3(randomSpawnX, randomSpawnY, randomSpawnZ);
        Vector3 targetPostion = new Vector3(randomTargetX, randomTargetY, randomTargetZ);
        Vector3 direction = targetPostion - spawnPosition;
        Quaternion rotation = Quaternion.LookRotation(direction);

        GameObject ball = Instantiate(Ball, spawnPosition, rotation);
        //AddRigidbodyWithSlip(ball);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.AddForce(direction*ballSpeed);
        timer = 0;
    }

    private void GetSpawningArea()
    {
        Transform cubeTransform = SpawnPoint.GetComponent<Transform>();

        // Pobierz pozycjê, skalê i rotacjê szeœcianu
        Vector3 cubePosition = cubeTransform.position;
        Vector3 cubeScale = cubeTransform.localScale;

        // Oblicz granice obszaru boiska
        minSpawnBounds = cubePosition - cubeScale / 2f;
        maxSpawnBounds = cubePosition + cubeScale / 2f;
    }
    private void GetGoalArea()
    {
        Transform cubeTransform = GoalPoint.GetComponent<Transform>();

        // Pobierz pozycjê, skalê i rotacjê szeœcianu
        Vector3 cubePosition = cubeTransform.position;
        Vector3 cubeScale = cubeTransform.localScale;

        // Oblicz granice obszaru boiska
        minGoalBounds = cubePosition - cubeScale / 2f;
        maxGoalBounds = cubePosition + cubeScale / 2f;
    }
    void AddRigidbodyWithSlip(GameObject obj)
    {
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
    public void StartStopGame()
    {
        if (gameRunning == false)
        { 
            ButtonText.text = "Stop Game";
            gameRunning = true;
        }
        else
        {
            ButtonText.text = "Start Game";
            gameRunning = false;
        }
}
}
