using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelGenerator : MonoBehaviour {

    public GameObject[] avaible_background_;
    public GameObject[] unavaible_background_;

    public GameObject bottom_left_edge_;
    public GameObject bottom_right_edge_;
    public GameObject top_left_edge_;
    public GameObject top_right_edge_;
    public GameObject bottom_left_out_;
    public GameObject bottom_right_out_;
    public GameObject top_left_out_;
    public GameObject top_right_out_;
    public GameObject ceiling_;
    public GameObject floor_;
    public GameObject left_wall_;
    public GameObject right_wall_;
    public GameObject left_platform_;
    public GameObject right_platform_;
    public GameObject platform_;
    public GameObject left_stairs_;
    public GameObject right_stairs_;
    public GameObject next_level_;
    
    public int min_height_;

    private int width_;
    private int height_;

    
    private System.Random randomizer = new System.Random();

    private enum GeneratorTopState { Ceil, ShiftUp, ShiftDown };
    private GeneratorTopState top_state_ = GeneratorTopState.Ceil;
    private enum GeneratorBottomState { Floor, LeftStair, RightStair, ShiftUp, ShiftDown }
    private GeneratorBottomState bottom_state_ = GeneratorBottomState.Floor;

    private void GenerateNewLevel() 
    {

        width_ = randomizer.Next(16, 32) * min_height_;
        height_ = 5 * min_height_;

        Scene previous_scene = SceneManager.GetActiveScene();

        string scene_name;
        try
        {
            scene_name = ((Convert.ToInt32(previous_scene.name)) + 1).ToString();
        }
        catch(Exception)
        {
            scene_name = 1.ToString();
        }

        Scene new_scene = SceneManager.CreateScene(scene_name);

        SceneManager.SetActiveScene(new_scene);

        //Generating

        GenerateTiles();

    }

   private void GenerateTiles()
   {

        int min_bottom = 0;
        int max_bottom = min_height_ * 2;
        int min_top = min_height_ * 3;
        int max_top = min_height_ * 5;


        int bottom = randomizer.Next(min_bottom, max_bottom);
        int top = randomizer.Next(min_top, max_top);

        int next_tile_position = 0;

        int value = min_height_;
        
        List<KeyValuePair<int, int>> top_values = new List<KeyValuePair<int, int>>();

        //Left walls
        DecorateColumn(next_tile_position, bottom, top);
        Instantiate(bottom_left_edge_, new Vector3(next_tile_position, bottom), Quaternion.identity);
        for(int i = bottom + 1; i < top; ++i)
        {
            Instantiate(left_wall_, new Vector3(next_tile_position, i), Quaternion.identity);
        }
        Instantiate(top_left_edge_, new Vector3(next_tile_position, top), Quaternion.identity);
        ++next_tile_position;

        //Top
        while (next_tile_position < width_)
        {
    
            if (top_state_ == GeneratorTopState.Ceil)
            {
                top_values.Add(new KeyValuePair<int, int>(top, value));

                if(next_tile_position + value > width_)
                {
                    value = width_ - next_tile_position;
                }

                for(; value > 0; --value)
                {
                    Instantiate(ceiling_, new Vector3(next_tile_position, top), Quaternion.identity);
                    ++next_tile_position;
                }               
            }
            else if (top_state_ == GeneratorTopState.ShiftDown)
            {
                if (top - value < min_top)
                {
                    value = top - min_top;
                }

                if(value > 2 && next_tile_position + 2 < width_)
                {
                    value -= 2;

                    Instantiate(top_right_edge_, new Vector3(next_tile_position, top), Quaternion.identity);
                    --top;

                    for (; value > 0; --value)
                    {
                        Instantiate(right_wall_, new Vector3(next_tile_position, top), Quaternion.identity);
                        --top;
                    }

                    Instantiate(bottom_left_out_, new Vector3(next_tile_position, top), Quaternion.identity);

                    ++next_tile_position;

                    top_values.Add(new KeyValuePair<int, int>(top, 1));
                }
            }
            else if (top_state_ == GeneratorTopState.ShiftUp)
            {
                if (top + value > max_top)
                {
                    value = max_top - top;
                }

                if (value > 2 && next_tile_position + 2 < width_)
                {
                    value -= 2;

                    Instantiate(bottom_right_out_, new Vector3(next_tile_position, top), Quaternion.identity);
                    ++top;

                    for (; value > 0; --value)
                    {
                        Instantiate(left_wall_, new Vector3(next_tile_position, top), Quaternion.identity);
                        ++top;
                    }

                    Instantiate(top_left_edge_, new Vector3(next_tile_position, top), Quaternion.identity);

                    ++next_tile_position;

                    top_values.Add(new KeyValuePair<int, int>(top, 1));
                }
                
            }

            value = NewTopState();
        }

        //Bottom
        next_tile_position = 1;
        value = min_height_;
        int top_index = 0;
        top = top_values[top_index].Key;
        int top_left = top_values[top_index].Value;

        while (next_tile_position < width_)
        {
            if (bottom_state_ == GeneratorBottomState.Floor)
            {

                if(next_tile_position + value > width_)
                {
                    value = width_ - next_tile_position;
                }

                for(; value > 0; --value)
                {
                    Instantiate(floor_, new Vector3(next_tile_position, bottom), Quaternion.identity);

                    GetNextTop(top_values,ref top,ref top_index,ref top_left);
                    DecorateColumn(next_tile_position, bottom, top);

                    ++next_tile_position;
                }
            }
            else if (bottom_state_ == GeneratorBottomState.LeftStair)
            {
                if(bottom + value > max_bottom)
                {
                    value = max_bottom - bottom;
                }

                if (value > 2 && next_tile_position + value < width_ - 2)
                {
                    --value;

                    Instantiate(bottom_right_edge_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                    ++bottom;

                    for (; value > 0; --value)
                    {
                        Instantiate(left_stairs_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                        GetNextTop(top_values, ref top, ref top_index, ref top_left);
                        DecorateColumn(next_tile_position, bottom, top);
                        ++next_tile_position;
                        ++bottom;
                    }
                    --bottom;
                }
            }
            else if (bottom_state_ == GeneratorBottomState.RightStair)
            {
                if(bottom - value < min_bottom)
                {
                    value = bottom - min_bottom;
                }

                if(value > 2 && next_tile_position + value < width_ - 2)
                {
                    --value;

                    for(; value > 0; --value)
                    {
                        Instantiate(right_stairs_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                        GetNextTop(top_values, ref top, ref top_index, ref top_left);
                        DecorateColumn(next_tile_position, bottom, top);
                        --bottom;
                        ++next_tile_position;
                    }
                    --next_tile_position;

                    Instantiate(bottom_left_edge_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                    ++next_tile_position;
                }
            }
            else if (bottom_state_ == GeneratorBottomState.ShiftDown)
            {
                if (bottom - value < min_bottom)
                {
                    value = bottom - min_bottom;
                }

                if (value > 2 && next_tile_position + 2 < width_)
                {
                    value -= 2;

                    Instantiate(top_right_out_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                    --bottom;

                    for (; value > 0; --value)
                    {
                        Instantiate(left_wall_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                        --bottom;
                    }

                    Instantiate(bottom_left_edge_, new Vector3(next_tile_position, bottom), Quaternion.identity);

                    GetNextTop(top_values, ref top, ref top_index, ref top_left);
                    DecorateColumn(next_tile_position, bottom, top);

                    ++next_tile_position;
                }
            }
            else if (bottom_state_ == GeneratorBottomState.ShiftUp)
            {
                if (bottom + value > max_bottom)
                {
                    value = max_bottom - bottom;
                }

                if (value > 2 && next_tile_position + 2 < width_)
                {
                    value -= 2;

                    Instantiate(bottom_right_edge_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                    ++bottom;

                    for (; value > 0; --value)
                    {
                        Instantiate(right_wall_, new Vector3(next_tile_position, bottom), Quaternion.identity);
                        ++bottom;
                    }

                    Instantiate(top_left_out_, new Vector3(next_tile_position, bottom), Quaternion.identity);

                    GetNextTop(top_values, ref top, ref top_index, ref top_left);
                    DecorateColumn(next_tile_position, bottom, top);

                    ++next_tile_position;
                }
            }

            value = NewBottomState();

        }

        //Right walls
        Instantiate(bottom_right_edge_, new Vector3(next_tile_position, bottom), Quaternion.identity);
        for(int i = bottom + 1; i < top; ++i)
        {
            Instantiate(right_wall_, new Vector3(next_tile_position, i), Quaternion.identity);
        }
        Instantiate(top_right_edge_, new Vector3(next_tile_position, top), Quaternion.identity);

        DecorateColumn(next_tile_position, bottom, top);

    }

    private void GetNextTop(List<KeyValuePair<int, int>> top_values, ref int top, ref int top_index, ref int top_left)
    {
        if (top_left <= 0)
        {
            ++top_index;
            top = top_values[top_index].Key;
            top_left = top_values[top_index].Value;
        }
        --top_left;
    }

    private void DecorateColumn(int column, int bottom, int top)
    {
        int background = 0;

        //Unavaible bottom
        for(int i = 0; i < bottom; ++i)
        {
            background = randomizer.Next(0, 3);
            if (background == 2)
            {
                background = randomizer.Next(1, unavaible_background_.Length);
            }
            else
            {
                background = 0;
            }
            Instantiate(unavaible_background_[background], new Vector3(column, i), Quaternion.identity);
        }

        //Avaible
        for(int i = bottom; i < top; ++i)
        {
            background = randomizer.Next(0, 3);
            if (background == 2)
            {
                background = randomizer.Next(1, avaible_background_.Length);
            }
            else
            {
                background = 0;
            }
            Instantiate(avaible_background_[background], new Vector3(column, i), Quaternion.identity);
        }

        //Unavaible top
        for(int i = top; i <= min_height_ * 5; ++i)
        {
            background = randomizer.Next(0, 3);
            if (background == 2)
            {
                background = randomizer.Next(1, unavaible_background_.Length);
            }
            else
            {
                background = 0;
            }
            Instantiate(unavaible_background_[background], new Vector3(column, i), Quaternion.identity);
        }
    }

    private int NewTopState()
    {
        int state_changer = randomizer.Next(1, 100);

        int value = 0;

        if (state_changer <= 15 && top_state_ == GeneratorTopState.Ceil)
        {
            top_state_ = GeneratorTopState.ShiftUp;
            value = randomizer.Next(min_height_, min_height_ * 2);
        }
        else if(state_changer > 15 && state_changer <= 30 && top_state_ == GeneratorTopState.Ceil)
        {
            top_state_ = GeneratorTopState.ShiftDown;
            value = randomizer.Next(min_height_, min_height_ * 2);
        }
        else 
        {
            top_state_ = GeneratorTopState.Ceil;
            value = randomizer.Next(min_height_, min_height_ * 4);
        }

        return value;
    }

    private int NewBottomState()
    {
        int state_changer = randomizer.Next(1, 100);

        int value = 0;

        if (state_changer <= 10 && bottom_state_ == GeneratorBottomState.Floor)
        {
            bottom_state_ = GeneratorBottomState.ShiftDown;
            value = randomizer.Next(3, min_height_ + 1);
        }
        else if (state_changer > 10 && state_changer <= 20 && bottom_state_ == GeneratorBottomState.Floor)
        {
            bottom_state_ = GeneratorBottomState.ShiftUp;
            value = randomizer.Next(3, min_height_ + 1);
        }
        else if (state_changer > 20 && state_changer <= 30 && bottom_state_ == GeneratorBottomState.Floor)
        {
            bottom_state_ = GeneratorBottomState.LeftStair;
            value = randomizer.Next(3, min_height_ * 2);
        }
        else if (state_changer > 30 && state_changer <= 40 && bottom_state_ == GeneratorBottomState.Floor)
        {
            bottom_state_ = GeneratorBottomState.RightStair;
            value = randomizer.Next(3, min_height_ * 2);
        }
        else
        {
            bottom_state_ = GeneratorBottomState.Floor;
            value = randomizer.Next(min_height_, min_height_ * 4);
        }

        return value;
    }


	// Use this for initialization
	void Start ()
    {
        GenerateNewLevel();
    }
/*
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            GenerateNewLevel();
        }
    }
*/
    // Update is called once per frame
    void Update ()
    {
		
	}

}
