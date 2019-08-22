using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RogueSpriteRenderer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private SpriteRenderer deathSpriteRenderer;

    [SerializeField]
    private Entry stackNumberEntry;

    [SerializeField]
    private SortingGroup sortingGroup;

    [SerializeField]
    /// <summary>
    /// This sprite is used by RoguelikeSpriteRenderers when no other sprite is available
    /// </summary>
    private Sprite debugSprite;

    public void CopyValues(RogueSpriteRenderer source)
    {
        StackSize = source.StackSize;
        MySprite = source.MySprite;
        Dead = source.Dead;
    }

    public void ClearValues()
    {
        StackSize = 0;
        MySprite = null;
        Dead = false;
    }

    private int stackSize = 0;
    public int StackSize
    {
        get => stackSize;
        set
        {
            stackNumberEntry.maxSize.x = mySpriteRenderer.bounds.size.x *.9f;
            if(value <= 1)
            {
                stackNumberEntry.gameObject.SetActive(false);
            }
            else
            {
                stackNumberEntry.gameObject.SetActive(true);
                stackNumberEntry.SetText(value.ToString());
            }
            stackNumberEntry.gameObject.transform.position = this.transform.position + new Vector3(0, stackNumberEntry.GetDimensions().y + mySpriteRenderer.bounds.size.y * .1f, 0);
            stackSize = value;
        }
    }

    private Sprite mySprite = null;
    public Sprite MySprite
    {
        get => mySprite;
        set
        {
            if(value != null)
            {
                mySpriteRenderer.sprite = value;
            }
            else
            {
                mySpriteRenderer.sprite = debugSprite;
            }
            mySprite = value;
        }
    }

    private bool dead = false;
    public bool Dead
    {
        get => dead;
        set
        {
            deathSpriteRenderer.gameObject.SetActive(value);
            dead = value;
        }
    }

    public int SortingOrder
    {
        get => sortingGroup.sortingOrder;
        set => sortingGroup.sortingOrder = value;
    }


    void Update()
    {
        PlayAnimations();
    }
    // ------------------------------------------------------ANIMATION STUFF--------------------------------------------------------//

    public enum AnimationStateEnum { IdleAnimation, MoveAnimation, AttackAnimation, EatAnimation, HitAnimation, ShakeAnimation, BounceAnimation }

    private float animationTime = 0.0f;
    private float animationSpeed = 5f;
    private bool isAnimating = false;
    private AnimationStateEnum animationStateCurrent = AnimationStateEnum.IdleAnimation;

    private float animationParam1 = 0;
    private float animationParam2 = 0;
    private float animationParam3 = 0;

    //Start an animation with various parameters
    public void StartAnimation(AnimationStateEnum animState, float speed = 5, float param1 = 1, float param2 = 0, float param3 = 1)
    {
        animationSpeed = speed;
        animationParam1 = param1;
        animationParam2 = param2;
        animationParam3 = param3;

        isAnimating = true;
        animationTime = 0.0f;

        animationStateCurrent = animState;

        //This is where we would set the animation speed.
        switch (animState)
        {
            case AnimationStateEnum.MoveAnimation:
                break;
            case AnimationStateEnum.AttackAnimation:
                break;
            case AnimationStateEnum.EatAnimation:
                break;
            case AnimationStateEnum.HitAnimation:
                break;
        }
    }


    //perform different sprite movements depending on the current animation
    void PlayAnimations()
    {
        if (isAnimating)
        {
            animationTime += Time.deltaTime * animationSpeed;


            if (animationStateCurrent == AnimationStateEnum.BounceAnimation)
            {
                if (animationTime < 0.5f)
                {
                    mySpriteRenderer.transform.localPosition = new Vector3(animationTime * animationParam1, animationTime * animationParam2, 0) * animationParam3;
                }
                else
                {
                    mySpriteRenderer.transform.localPosition = new Vector3((1 - animationTime) * animationParam1, (1 - animationTime) * animationParam2, 0) * animationParam3;
                }
            }
            else if (animationStateCurrent == AnimationStateEnum.MoveAnimation)
            {
                mySpriteRenderer.transform.localPosition = new Vector3((1 - animationTime) * -animationParam1, (1 - animationTime) * -animationParam2, 0);
            }
            else if (animationStateCurrent == AnimationStateEnum.ShakeAnimation)
            {
                mySpriteRenderer.transform.localPosition = new Vector3((float)(Mathf.Sin(animationTime * animationParam3) * animationParam1), (float)(Mathf.Sin(animationTime * animationParam3) * animationParam2), 0);
            }

            if (animationTime >= 1.0f)
            {
                mySpriteRenderer.transform.localPosition = new Vector3(0, 0, 0);
            }
            if (animationTime >= 1.0f)
            {
                isAnimating = false;
            }

        }
    }

}
