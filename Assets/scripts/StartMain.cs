using UnityEngine;
using System.Collections;
using DG.Tweening;

public class StartMain : MonoBehaviour {

    public GameObject bird;
    public GameObject land;
    public GameObject back_ground;
    public Sprite[] back_list;

    private GameObject nowPressBtn = null;
    private void Awake()
    {
        //Set screen size for Standalone
    #if UNITY_STANDALONE
            Screen.SetResolution(1080, 1920, false);
            Screen.fullScreen = false;
    #endif
    }
    // Use this for initialization
    void Start () {      

        // random background
        int index = Random.Range(0, back_list.Length);
        back_ground.GetComponent<SpriteRenderer>().sprite = back_list[index];

    }
	
	// Update is called once per frame
	void Update () {
        // Handle native touch events
        foreach (Touch touch in Input.touches)
        {
            HandleTouch(touch.fingerId, touch.position, touch.phase);
        }

        // Simulate touch events from mouse events
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch(10, Input.mousePosition, TouchPhase.Began);
            }
            if (Input.GetMouseButton(0))
            {
                HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0))
            {
                HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
            }
        }
    }

    private void HandleTouch(int touchFingerId, Vector2 touchPosition, TouchPhase touchPhase)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(touchPosition);
        Vector2 worldPos = new Vector2(wp.x, wp.y);
        switch (touchPhase)
        {
            case TouchPhase.Began:
                print(touchPosition);
                print(worldPos);

                foreach(Collider2D c in Physics2D.OverlapPointAll(worldPos))
                {
                    name = c.gameObject.name;
                    print(name);
                    if (name == "easy_btn" || name == "normal_btn" || name == "challenge_btn")
                    {
                        c.transform.DOMoveY(c.transform.position.y - 0.03f, 0f);
                        nowPressBtn = c.gameObject;
                    }
                }
                break;
            case TouchPhase.Moved:
                // TODO
                break;
            case TouchPhase.Ended:
                if (nowPressBtn)
                {
                    nowPressBtn.transform.DOMoveY(nowPressBtn.transform.position.y + 0.03f, 0f);

                    foreach (Collider2D c in Physics2D.OverlapPointAll(worldPos))
                    {
                        name = c.gameObject.name;
                        print(name);

                        if (name == nowPressBtn.name)
                        {
                            if (name == "easy_btn")
                            {
                                OnPressEasy();
                            }
                            if (name == "normal_btn")
                            {
                                OnPressNormal();
                            }
                            if (name == "challenge_btn")
                            {
                                OnPressChallenge();
                            }

                        }
                    }

                    nowPressBtn = null;
                }

                
                break;
        }
    }

    #region ButtonSelect
    private void OnPressEasy()
    {
        SetEasyDifficulty();
        Application.LoadLevel(1);
    }

    private void OnPressNormal()
    {
        SetNormalDifficulty();
        Application.LoadLevel(1);
    }
   
    private void OnPressChallenge()
    {
        SetChallengeDifficulty();
        Application.LoadLevel(1);
    }

    #endregion

    #region Difficulty
    public void SetEasyDifficulty()
    {
        GameValues.Difficulty = GameValues.Difficulties.Easy;
    }

    public void SetNormalDifficulty()
    {
        GameValues.Difficulty = GameValues.Difficulties.Normal;
    }

    public void SetChallengeDifficulty()
    {
        GameValues.Difficulty = GameValues.Difficulties.Challenge;
    }
    #endregion
}
