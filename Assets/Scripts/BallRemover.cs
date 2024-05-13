using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallRemover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Soccer Ball(Clone)")
        {
            other.gameObject.GetComponent<Ball>().RemoveBall();
        }
    }
}
