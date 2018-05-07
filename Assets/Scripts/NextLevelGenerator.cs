using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelGenerator : MonoBehaviour {

    public GameObject bottom_left_edge_;
    public GameObject bottom_right_edge_;
    public GameObject top_left_edge_;
    public GameObject top_right_edge_;
    public GameObject ceiling_;
    public GameObject floor_;
    public GameObject left_wall_;
    public GameObject right_wall_;
    public GameObject left_platform_;
    public GameObject right_platform_;
    public GameObject platform_;

    public int min_height_;

    private int width_;
    private int height_;
    


    private void GenerateNewLevel()
    {
        System.Random randomizer = new System.Random();
        width_ = randomizer.Next(16, 32) * min_height_;
        height_ = randomizer.Next(2, 4) * min_height_;

        Instantiate(bottom_left_edge_, new Vector3(-1, 0, 0), Quaternion.identity);
        Instantiate(top_left_edge_, new Vector3(-1, min_height_ + 1, 0), Quaternion.identity);

        for (int i = 1; i < min_height_ + 1; ++i)
        {
            Instantiate(left_wall_, new Vector3(-1, i, 0), Quaternion.identity);
        }

        for (int i = 0; i < width_; ++i)
        {
            Instantiate(floor_,new Vector3(i, 0, 0), Quaternion.identity);
            Instantiate(ceiling_, new Vector3(i, min_height_ + 1, 0), Quaternion.identity);
        }

    }

	// Use this for initialization
	void Start ()
    {
        GenerateNewLevel();
        GetComponent<BoxCollider2D>().size = new Vector2(0.2f, min_height_);
        transform.position = new Vector3(width_ -0.5f, 3 - 0.5f, 0);      
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
