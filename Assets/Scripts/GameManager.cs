using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Blob blob;
    private static GameManager _instance;

    public static GameManager Instance
    {
        get => _instance;
    }

    private bool foodExists;
    private Vector3 foodPosition;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
    }

    void Start()
    {
        foodExists = false;
        blob.LoadBlob();
    }

    // Update is called once per frame
    void Update()
    {
        if (blob.GetMood() == Blob.Mood.Dead)
        {
            StartCoroutine(GameOver());
        }
    }

    public void NotifyFoodPlacement(Vector3 position)
    {
        Debug.Log("Food placed");
        foodExists = true;
        foodPosition = position;
    }

    public bool GetFoodExists()
    {
        return foodExists;
    }

    public void SetFoodExists(bool state)
    {
        foodExists = state;
    }

    public Vector3 GetFoodPosition()
    {
        return foodPosition;
    }
        IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Death Scene");
    }

    
}

