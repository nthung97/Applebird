using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("movable"))
        {
            gameObject.SetActive(false);
        }
    }
  
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void GameOver()
    {
        gameObject.SetActive(false);
    }
}
