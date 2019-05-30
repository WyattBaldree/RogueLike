using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Healthbar : MonoBehaviour
{
    public Sprites backgroundObj;
    public Sprites healthObj;
    public Sprites redHealthObj;

    public float redHealthDelay = 2.0f;
    private float redHealthDelayCurrent;

    public float redHealthSpeed = .01f;

    [Range(0f, 1f)] public float health = 0.5f;

    private float oldHealth;

    private float redHealth;

    void Start()
    {
        oldHealth = health;
        redHealth = health;
        redHealthDelayCurrent = 0;


        SetPercentage(backgroundObj, 1f);

        SetPercentage(redHealthObj, 1f);

        SetPercentage(healthObj, health);
    }

    public void SetParent(Transform t)
    {

    }

    private void Update()
    {
        
        // count the redhealth delay down to 0 if it is above 0
        if (redHealthDelayCurrent > 0)
        {
            redHealthDelayCurrent -= Time.deltaTime;
        }
        
        //If our health has changed
        if (health != oldHealth)
        {
            //Adjust our sprite
            SetHealthPercent();
        }

        //if our redhealth is different from our health and the red health delay has finished, move the red health towards the green health.
        if (redHealth != health && redHealthDelayCurrent <= 0)
        {
            if(Math.Abs(redHealth - health) < redHealthSpeed)
            {
                redHealth = health;
            }
            else
            {
                redHealth -= Math.Sign(redHealth - health)* redHealthSpeed;
            }
        }

        //update redhealth sprite.
        SetPercentage(redHealthObj, redHealth);
    }

    //This functino just sets the specified sprite to the specified length [0 - 1]
    void SetPercentage(Sprites sprites, float percentage)
    {
        Sprite s = sprites.GetSprite();
        Sprite newSprite = Sprite.Create(s.texture, new Rect(0, 0, s.texture.width*percentage, s.texture.height), Vector2.zero, 16);
        sprites.SetSprite(newSprite);
    }

    private void SetHealthPercent()
    {
        SetPercentage(healthObj, health);
        oldHealth = health;
        redHealthDelayCurrent = redHealthDelay;
    }

    public void SetHealth(float h)
    {
        health = h;
        SetHealthPercent();
    }
}
