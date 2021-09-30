using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnToy : MonoBehaviour
{
    public GameObject toy;
    private Vector3 spawnPos = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke ("Reset", 5.0f);
    }

    
}