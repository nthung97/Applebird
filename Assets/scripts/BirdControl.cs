using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Linq;

public class BirdControl : MonoBehaviour {

	public int rotateRate = 10;
	public float upSpeed = 10;
    public GameObject scoreMgr;

    public AudioClip jumpUp;
    public AudioClip hit;
    public AudioClip score;

    public bool inGame = false;

	public bool  dead = false;
	private bool landed = false;

    private bool crazyMode = false;
    private SpriteRenderer spriteRender;
    private Sequence birdSequence;

    public float speed = 1.0f;

    // Use this for initialization
    void Start () {
        float birdOffset = 0.05f;
        float birdTime = 0.3f;
        float birdStartY = transform.position.y;

        birdSequence = DOTween.Sequence();

        birdSequence.Append(transform.DOMoveY(birdStartY + birdOffset, birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY - 2 * birdOffset, 2 * birdTime).SetEase(Ease.Linear))
            .Append(transform.DOMoveY(birdStartY, birdTime).SetEase(Ease.Linear))
            .SetLoops(-1);
    }
	
	// Update is called once per frame
	void Update () {
        if (!inGame)
        {
            return;
        }
        birdSequence.Kill();

		if (!dead)
		{
			if (Input.GetButtonDown("Fire1"))
			{
                JumpUp();
			}
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-speed, 0));
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0));
            }
            if(Input.GetKey(KeyCode.Space))
            {
                crazyMode = true;
                transform.GetComponent<SpriteRenderer>().color = new Color(255.0f, 153.0f, 51.0f, 125.0f);

            }
        }

		if (!landed)
		{
			float v = transform.GetComponent<Rigidbody2D>().velocity.y;			
			float rotate = Mathf.Min(Mathf.Max(-90, v * rotateRate + 60), 30);		
			transform.rotation = Quaternion.Euler(0f, 0f, rotate);
		}
		else
		{
			transform.GetComponent<Rigidbody2D>().rotation = -90;
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
        switch(GameValues.Difficulty)
        {
            case GameValues.Difficulties.Easy:
                if (other.name == "pipe_up" || other.name == "pipe_down")
                {
                    if (!dead)
                    {
                        Debug.Log(other.name);
                        GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");

                        foreach (GameObject g in objs)
                        {
                            if (objs.Contains(g))
                                g.BroadcastMessage("GameOver");
                        }

                        GetComponent<Animator>().SetTrigger("die");
                        AudioSource.PlayClipAtPoint(hit, Vector3.zero);
                    }
                }

                if (other.name == "land")
                {
                    scoreMgr.GetComponent<ScoreMgr>().AddScore();
                    transform.GetComponent<Rigidbody2D>().gravityScale = 0;
                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                }

                if (other.name.Contains("Enemy"))
                {
                    scoreMgr.GetComponent<ScoreMgr>().SubtractScore();
                    AudioSource.PlayClipAtPoint(hit, Vector3.zero);
                }
                break;

            case GameValues.Difficulties.Normal:
                if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down")
                {
                    if (!dead)
                    {
                        Debug.Log(other.name);
                        GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");

                        foreach (GameObject g in objs)
                        {
                            if (objs.Contains(g))
                                g.BroadcastMessage("GameOver");
                        }

                        GetComponent<Animator>().SetTrigger("die");
                        AudioSource.PlayClipAtPoint(hit, Vector3.zero);
                    }

                    if (other.name == "land")
                    {
                        transform.GetComponent<Rigidbody2D>().gravityScale = 0;
                        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        landed = true;
                    }
                
                }
                if (other.name.Contains("Enemy"))
                {
                    scoreMgr.GetComponent<ScoreMgr>().SubtractScore();
                    AudioSource.PlayClipAtPoint(hit, Vector3.zero);
                }
                break;

            case GameValues.Difficulties.Challenge:
                if (other.name == "land" || other.name == "pipe_up" || other.name == "pipe_down" 
                    || other.name.Contains("Enemy"))
                {
                    if (!dead)
                    {
                        Debug.Log(other.name);
                        GameObject[] objs = GameObject.FindGameObjectsWithTag("movable");
                        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");

                        GameObject[] allObjs = objs.Concat(enemy).ToArray();

                        foreach (GameObject g in allObjs)
                        {
                            if (objs.Contains(g))
                                g.BroadcastMessage("GameOver");
                        }

                        GetComponent<Animator>().SetTrigger("die");
                        AudioSource.PlayClipAtPoint(hit, Vector3.zero);
                    }

                    if (other.name == "land")
                    {
                        transform.GetComponent<Rigidbody2D>().gravityScale = 0;
                        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                        landed = true;
                    }

                  
                }
                break;
        }

        
        if (other.name == "pass_trigger")
        {
            scoreMgr.GetComponent<ScoreMgr>().AddScore();
            AudioSource.PlayClipAtPoint(score, Vector3.zero);
        }


	}

    public void JumpUp()
    {
        transform.GetComponent<Rigidbody2D>().gravityScale = 1;
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(0, upSpeed);
        AudioSource.PlayClipAtPoint(jumpUp, Vector3.zero);
    }
	
	public void GameOver()
	{
		dead = true;
	}
}
