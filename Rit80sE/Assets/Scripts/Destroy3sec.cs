using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy3sec : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 3);
    }
}
