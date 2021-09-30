using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public string nickname;
    public float hungerStatus = 100;
    private SkinnedMeshRenderer rend;
    private Material shellMaterial;
    public float hungerSpeed;
    public int sadnessTime;
    public int deathTimeHunger;
    public int deathTimeSadness;
    private float hungerCounter;
    private DateTime lastCuddle;
    private DateTime starvingSince;
    private DateTime sadSince;
    private bool isStarving;
    public float colorValue;

    public enum Mood {
        Happy,
        Sad,
        Hungry,
        Angry,
        Dead
    } 
    private Mood mood = Mood.Happy;
    // Start is called before the first frame update
    void Start()
    {
        rend = transform.Find("TurtleShell").gameObject.GetComponent<SkinnedMeshRenderer>();
        shellMaterial = rend.materials[0];
        hungerCounter = hungerSpeed;
        if (lastCuddle == DateTime.MinValue)
        {
            lastCuddle = DateTime.Now;
        }

        isStarving = false;
    }

    // Update is called once per frame
    void Update()
    {
        hungerCounter -= Time.deltaTime;
        if (hungerCounter < 0)
        {
            if (hungerStatus > 0)
            {
                hungerStatus -= 1;
                hungerCounter = hungerSpeed;
            }
        }

        if ((DateTime.Now - lastCuddle).TotalSeconds > sadnessTime)
        {
            mood = Mood.Sad;
            SadSinceNow();
        }

        if (mood == Mood.Sad)
        {
            float sadTime = (DateTime.Now - starvingSince).Seconds;
            if (sadTime > deathTimeSadness)
            {
                mood = Mood.Dead;
            }
        }
        if (isStarving)
        {
            float starvingTime = (DateTime.Now - starvingSince).Seconds;
            Debug.Log("Starving time:" + starvingTime);
            if (starvingTime > deathTimeHunger)
            {
                mood = Mood.Dead;
            }
            
        }
    }

    public void setNickname(string nickname) {
        this.nickname = nickname;
    }

    public void changeShellColor(float colorValue) {
        shellMaterial.SetColor("_Color", Color.HSVToRGB(colorValue, 1, 1));
        this.colorValue = colorValue;
    }
    public void Eat(int hungerPoints)
    {
        Debug.Log("Eat food: " + hungerPoints);
        hungerStatus = hungerStatus + hungerPoints > 100 ? 100 : hungerStatus + hungerPoints;
        isStarving = true;
    }
    
    public void LoadBlob() {
        BlobData data = DataManager.Load();
        this.nickname = data.name;
        this.changeShellColor(data.shellColorValue);
        this.hungerStatus = data.hungerStatus;
        this.mood = data.mood;
    }

    public float GetHunger() {
        return hungerStatus;
    }

    public Mood GetMood() {
        return mood;
    }

    public void SetLastCuddleNow()
    {
        lastCuddle = DateTime.Now;
    }
    
    public DateTime GetLastCuddle()
    {
        return lastCuddle;
    }

    public void StarvingSinceNow()
    {
        starvingSince = DateTime.Now;
    }

    public void SetIsStarving(bool starving)
    {
        isStarving = starving;
    }

    public bool GetIsStarving()
    {
        return isStarving;
    }

    public void SadSinceNow()
    {
        sadSince = DateTime.Now;
    }

    public void ChangeMood(Mood newMood)
    {
        if (mood != Mood.Dead)
        {
            mood = newMood;
        }
    }
}
