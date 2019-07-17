using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameMain : MonoBehaviour {

    public GameObject bird;
    public GameObject readyPic;
    public GameObject tipPic;
    public GameObject scoreMgr;
    public GameObject pipeSpawner;
    public GameObject gameoverPic;

    private bool gameStarted = false;
    private bool gameEnded = false;

	// Use this for initialization
	void Start () {
        gameoverPic.SetActive(false);
	    switch(GameValues.Difficulty)
        {
            case GameValues.Difficulties.Easy:
                Debug.Log("easy");
                break;
            case GameValues.Difficulties.Normal:
                Debug.Log("normal");
                break;
            case GameValues.Difficulties.Challenge:
                Debug.Log("challenge");
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameStarted && Input.GetButtonDown("Fire1"))
        {
            gameStarted = true;
            StartGame();
        }
        BirdControl control = bird.GetComponent<BirdControl>();
        if(control.dead == true)
        {
            gameoverPic.SetActive(true);
            if (Input.GetButtonDown("Fire1"))
            {
                Application.LoadLevel(0);
            }
        }
    }

    private void StartGame()
    {
        BirdControl control = bird.GetComponent<BirdControl>();
        control.inGame = true;
        control.JumpUp();

        readyPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);
        tipPic.GetComponent<SpriteRenderer>().material.DOFade(0f, 0.2f);

        scoreMgr.GetComponent<ScoreMgr>().SetScore(0);
        pipeSpawner.GetComponent<PipeSpawner>().StartSpawning();
    }
}
