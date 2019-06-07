using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationManager : MonoBehaviour {

    public List<GameObject> Slides = new List<GameObject>();
    
    public enum TransitionStyle { None, Flicker, Drop}

    public List<TransitionStyle> transitions = new List<TransitionStyle>();

    public static int currentSlide;

    [Space (40)]
    [Header ("Flicker Stuff")]
    public Vector2 flashOffTimesBounds;
    public Vector2 flashOnTimesBounds;
    public Vector2 timeBetweenSetsBounds;
    public Vector2 flashPerSetBounds;
    public int SetAmount;
    [Space(10)]
    private float setTimer = 0;
    private float flashTimer = 0;
    private int flashCounter = 0;

    private float flashOffTimes;
    private float flashOnTimes;
    private float timeBetweenSets;
    private int flashPerSet;
    private int SetCounter;

    private bool midFlicker;

    // Use this for initialization
    void Start () {
        if (currentSlide == 4)
            currentSlide++;
        ChangeSlide();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown(0))
        {
            currentSlide++;
            if (currentSlide < transitions.Count)
            {
                if (transitions[currentSlide] == TransitionStyle.None)
                    ChangeSlide();
                if (transitions[currentSlide] == TransitionStyle.Flicker)
                    FlickerStart();
            }
            else
                ChangeSlide();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetMouseButtonDown(1))
        {
            currentSlide--;
            ChangeSlide();
        }

        if (midFlicker)
        {
            Flickering();
        }
    }

    void ChangeSlide ()
    {
        for (int i = 0; i < Slides.Count; i++)
        {
            if (i == currentSlide)
                Slides[i].SetActive(true);
            else
            {
                if (Slides[i] != Slides[currentSlide])
                    Slides[i].SetActive(false);
            }
        }
    }

    void FlickerStart ()
    {
        flashOffTimes = Random.Range(flashOffTimesBounds.x, flashOffTimesBounds.y);
        flashOnTimes = Random.Range(flashOnTimesBounds.x, flashOnTimesBounds.y);
        timeBetweenSets = Random.Range(timeBetweenSetsBounds.x, timeBetweenSetsBounds.y);
        flashPerSet = Mathf.RoundToInt(Random.Range(flashPerSetBounds.x, flashPerSetBounds.y));
        midFlicker = true;
    }

    void Flickering ()
    {
        if (setTimer > timeBetweenSets)
        {
            if (flashTimer < flashOffTimes)
            {
                Slides[currentSlide - 1].SetActive(false);
            }
            else
            {
                Slides[currentSlide - 1].SetActive(true);

                if (flashTimer > flashOffTimes + flashOnTimes)
                {
                    if (flashCounter < flashPerSet)
                    {
                        flashTimer = 0;
                        flashOffTimes = Random.Range(flashOffTimesBounds.x, flashOffTimesBounds.y);
                        flashOnTimes = Random.Range(flashOnTimesBounds.x, flashOnTimesBounds.y);
                        flashCounter++;
                    }
                    else
                    {
                        flashCounter = 0;
                        setTimer = 0;
                        SetCounter++;
                    }
                }
            }
        }
        else
        {
            flashTimer = 0;
        }
        if (SetCounter >= SetAmount)
        {
            midFlicker = false;
            SetCounter = 0;
            ChangeSlide();
        }
        setTimer += Time.deltaTime;
        flashTimer += Time.deltaTime;
    }
}
