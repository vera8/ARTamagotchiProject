using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BlobController : MonoBehaviour
{
    private Blob blob;
    
    public float speed;
    private Animator animator;
    private bool turnToCamera;

    public ParticleSystem[] loveSystem;
    private Timer loveTimer;
    private Timer touchTimer;
    private Ray raycast;
    private RaycastHit raycastHit;
    private Touch touch;
    private int i = 0;
    
    private AudioSource audioSource;
    public AudioClip movingAudio;
    public AudioClip munchingAudio;
    public AudioClip happyPurring;
    private bool soundPlaying;

    void Start()
    {
        blob = GetComponent<Blob>();
        animator = GetComponent<Animator>();
        turnToCamera = false;
        animator.Play("Idle");
        audioSource = GetComponent<AudioSource>();
        soundPlaying = false;
        loveTimer = new Timer(3);
        touchTimer = new Timer(1);
    }
    
    void Update() {
        if((Input.touchCount > 0)) {
            touch = Input.GetTouch(0);
            raycast = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(raycast, out raycastHit)) {
                if (raycastHit.collider.CompareTag("Blob")) {
                    if(touch.phase == TouchPhase.Began)
                    {
                        audioSource.clip = happyPurring;
                        audioSource.Play();
                        blob.SetLastCuddleNow();
                        blob.ChangeMood(Blob.Mood.Happy);
                        loveTimer.StartTimer();
                        touchTimer.StartTimer();
                    }
                }
            }
            if(touch.phase == TouchPhase.Ended) {
                touchTimer.Stop();
                touchTimer.Reset();

                loveTimer.Stop();
                loveTimer.Reset();
            } else if(touch.phase == TouchPhase.Stationary) {
                if(touchTimer.IsDone()) {
                    loveTimer.Stop();
                    loveTimer.Reset();
                }
            } else {
                if(loveTimer.IsDone()) {
                    foreach (var parSys in loveSystem) {
                        parSys.Play();
                    }
                    loveTimer.Stop();
                    loveTimer.Reset();
                }
            }
            i++;
        }
        loveTimer.Update(Time.deltaTime);
        touchTimer.Update(Time.deltaTime);

        foreach(var parSys in loveSystem) {
            parSys.transform.position = this.transform.position;
        }

        if (blob.GetIsStarving() && blob.GetMood() != Blob.Mood.Dead)
        {
            animator.Play("Dizzy");
        }

        if (blob.GetMood() == Blob.Mood.Dead)
        {
            animator.Play("Dying");
        }
    }
    
    void FixedUpdate()
    {
        if (VuforiaBehaviour.Instance == null) 
            return;
        if (GameManager.Instance.GetFoodExists())
        {
            // check if on same plane
            Plane blobPlane = new Plane(transform.up, transform.position);
            Vector3 foodPosition = GameManager.Instance.GetFoodPosition();
            if (blobPlane.GetDistanceToPoint(foodPosition) < 0.5f)
            {
                Vector3 direction = foodPosition - transform.position;
                float angle = Vector3.Angle(transform.forward, direction);
                if (angle > 1 && Vector3.Distance(transform.position, foodPosition) > 0.3f)
                {
                    Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2.0f);
                }
                else
                {
                    animator.Play("Moving");
                    if (!soundPlaying)
                    {
                        audioSource.clip = movingAudio;
                        audioSource.Play();
                        soundPlaying = true;
                    }
                    Debug.Log("Moving audio playing: " + audioSource.isPlaying);
                    Debug.Log("Audio clip: " + audioSource.clip);
                    float step =  speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, foodPosition, step);
                }
            }
        }

        if (turnToCamera)
        {
            audioSource.clip = movingAudio;
            if (!soundPlaying)
            {
                audioSource.Play();
                soundPlaying = true;
            }
            Vector3 cameraPos = VuforiaBehaviour.Instance.transform.position;
            Vector3 camDirection = cameraPos - transform.position;
            camDirection.y = 0;
            Quaternion rotation = Quaternion.LookRotation(camDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2.0f);
            if (Vector3.Angle(transform.forward, camDirection) < 0.1)
            {
                turnToCamera = false;
                audioSource.Stop();
                soundPlaying = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //animator.Play("Idle");
        audioSource.Stop();
        soundPlaying = false;
        if (other.gameObject.CompareTag("Food"))
        {
            // munch food
            StartCoroutine(EatFood(other.gameObject));
        }
    }

    IEnumerator EatFood(GameObject food)
    {
        GameManager.Instance.SetFoodExists(false);
        audioSource.clip = munchingAudio;
        audioSource.Play();
        Debug.Log("Audio clip: " + audioSource.clip);
        animator.Play("Moving");
        yield return new WaitForSeconds(3);
        audioSource.Stop();
        animator.Play("Idle");
        blob.Eat(food.GetComponent<FoodItemBehaviour>().hungerPoints);
        food.GetComponent<MeshRenderer>().enabled = false;
        food.GetComponent<Collider>().enabled = false;
        turnToCamera = true;
    }
    
}
