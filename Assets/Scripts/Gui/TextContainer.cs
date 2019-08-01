using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextContainer : MonoBehaviour
{
    public enum AnimationStateEnum { none, wave, move, bounce, rainbow };

    public TextMesh textMesh;

    bool animate = false;

    public AnimationStateEnum anim = AnimationStateEnum.none;

    public float animationTime = 0.0f;

    public float animationSpeed = 0.05f;

    public float animationLoopDelay = 1;

    public float animationLoopTime = 0.0f;

    public float width = 0.0f;

    AnimationStateEnum animationStateCurrent = AnimationStateEnum.none;

    float animationParam1 = 1;
    float animationParam2 = 1;
    float animationParam3 = 1;

    private void Update()
    {
        animationLoopTime += Time.deltaTime;

        if (!animate)
        {
            if (animationLoopTime >= animationLoopDelay)
            {
                animationLoopTime -= animationLoopDelay;
                animate = true;
            }
        }

        if (animate)
        {
            if (animationTime > 1.0f)
            {
                EndAnimation();
                return;
            }

            animationTime += Time.deltaTime * animationSpeed;

            if (animationStateCurrent == AnimationStateEnum.wave)
            {
                textMesh.transform.localPosition = new Vector3((float) Math.Sin(animationTime * 2 * Math.PI) * animationParam1, (float)Math.Sin(animationTime * 2 * Math.PI) * animationParam2, 0) * animationParam3;
            }
            else if (animationStateCurrent == AnimationStateEnum.bounce)
            {
                textMesh.transform.localPosition = new Vector3((float)Math.Sin(animationTime * Math.PI) * animationParam1, (float)Math.Sin(animationTime * Math.PI) * animationParam2, 0) * animationParam3;
            }
            else if (animationStateCurrent == AnimationStateEnum.rainbow)
            {
                textMesh.transform.GetComponentInChildren<TextMesh>().color = Color.HSVToRGB(animationTime*animationParam1, animationParam2, animationParam3); ;
            }


        }
    }

    //Start an animation with various parameters
    public void StartAnimation(AnimationStateEnum animState,float animationStartTime = 0.0f, float loopDelay = 0.0f, float speed = .05f, float param1 = 1.0f, float param2 = 0, float param3 = 1)
    {
        animationSpeed = speed;
        animationParam1 = param1;
        animationParam2 = param2;
        animationParam3 = param3;


        animationStateCurrent = animState;
        //animate = true;
        animationTime = animationStartTime;

        if(animationTime > 0)
        {
            animate = true;
        }

        animationLoopDelay = loopDelay;

        animationLoopTime = 0;

        //This is where we would set the animation speed.
        switch (animState)
        {
            case AnimationStateEnum.wave:
                break;
        }
    }

    public void EndAnimation()
    {
        animationTime = 0.0f;
        animate = false;

        textMesh.transform.localPosition = new Vector3(0, 0, 0);
    }

    ///Returns the width of a text mesh.
    public float GetWidth()
    {
        float width = 0;
        foreach (char symbol in textMesh.text)
        {
            CharacterInfo info;
            if (textMesh.font.GetCharacterInfo(symbol, out info, textMesh.fontSize, textMesh.fontStyle))
            {
                width += info.advance;
            }
        }
        return width * textMesh.characterSize * 0.1f;// * transform.lossyScale.x;
    }
}
