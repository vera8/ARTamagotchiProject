using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tombstone : MonoBehaviour
{
    public Text tombstoneText;

    public TextMeshProUGUI message;
    // Start is called before the first frame update
    void Start()
    {
        BlobData blobData = DataManager.Load();
        tombstoneText.text = "Here lies\n"+ blobData.name + "\nCause of death\nLoneliness";
        message.text =
            "Your beloved" + blobData.name + "has patiently waited for you to return, but here is only so long an innocent Blob can survive on its own...";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}