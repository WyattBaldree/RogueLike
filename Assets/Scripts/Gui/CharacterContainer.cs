using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class CharacterContainer : MonoBehaviour
{
    public enum AnimationStateEnum { none, wave, move, bounce, rainbow };

    public TextMeshPro textMesh;

    bool animate = false;

    public AnimationStateEnum anim = AnimationStateEnum.none;

    public float animationTime = 0.0f;

    public float animationSpeed = 0.05f;

    public float animationLoopDelay = 1;

    public float width = 0.0f;

    AnimationStateEnum animationStateCurrent = AnimationStateEnum.none;

    float animationParam1 = 1;
    float animationParam2 = 1;
    float animationParam3 = 1;

    float previousAnimationTime = 0;

    float animationOffset;

    public void Animate(float animationTime)
    {
        float animationLength = 1 / animationSpeed;

        float animationTimeMod = (animationTime + animationOffset) % (animationLength + animationLoopDelay);

        if (animate && animationTimeMod <= animationLength)
        {
            float animationPercent = animationTimeMod / animationLength;
            if (animationStateCurrent == AnimationStateEnum.wave)
            {
                textMesh.transform.localPosition = new Vector3((float)Math.Sin(animationPercent * 2 * Math.PI) * animationParam1, (float)Math.Sin(animationPercent * 2 * Math.PI) * animationParam2, 0) * animationParam3;
            }
            else if (animationStateCurrent == AnimationStateEnum.bounce)
            {
                textMesh.transform.localPosition = new Vector3((float)Math.Sin(animationPercent * Math.PI) * animationParam1, (float)Math.Sin(animationPercent * Math.PI) * animationParam2, 0) * animationParam3;
            }
            else if (animationStateCurrent == AnimationStateEnum.rainbow)
            {
                textMesh.transform.GetComponentInChildren<TextMeshPro>().color = Color.HSVToRGB(animationPercent * animationParam1, animationParam2, animationParam3);
            }
        }
        else
        {
            textMesh.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    //Start an animation with various parameters
    public void StartAnimation(AnimationStateEnum animState,float animationOffsetParam = 0.0f, float loopDelay = 0.0f, float speed = .05f, float param1 = 1.0f, float param2 = 0, float param3 = 1)
    {
        animationSpeed = speed;
        animationParam1 = param1;
        animationParam2 = param2;
        animationParam3 = param3;


        animationStateCurrent = animState;

        animationOffset = animationOffsetParam;
        animate = true;

        animationLoopDelay = loopDelay;
    }

    public void EndAnimation()
    {
        animate = false;
    }

    ///Returns the width of a text mesh.
    public float GetWidth()
    {
        float width = 0;

        width += textMesh.textInfo.characterInfo[0].xAdvance;

        return width;
    }
}
