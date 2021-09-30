using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject toy;
    
    public void InstanceBall() {
        Instantiate(toy, transform.position, new Quaternion());
    }
}