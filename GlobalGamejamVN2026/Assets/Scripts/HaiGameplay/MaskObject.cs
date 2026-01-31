using DG.Tweening;
using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : InteractableObject
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem dropVfx;
    bool isInFace = false;
    bool isOverlapMaskObject = false;

    public MaskItemType itemType;

    public enum State
    {
        IDLE,
        DRAGGED,
    }

    [SerializeField] private State m_state;

    private void UpdateState()
    {
        switch (m_state)
        {
            case State.IDLE:
                break;
        }
    }

    public void SetState(State state)
    {
        if (state != m_state)
        {
            OnEndOldState(m_state);
            OnStartNewState(state);
        }

        m_state = state;
    }

    public State GetState()
    {
        return m_state;
    }

    private void OnEndOldState(State state)
    {
    }

    private void OnStartNewState(State state)
    {
        switch (state)
        {
            case State.IDLE:
                break;
        }
    }

    /// <summary>
    /// /////////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>
    /// 

    private void Start()
    {
        SetState(State.IDLE);
        base.Setup();
    }

    private void Update()
    {
        UpdateState();
    }

    public override void OnReceiveActionFromPlayer(Vector2 touchPos)
    {
        base.OnReceiveActionFromPlayer(touchPos);

        if (MaskItemConveyorManager.Instance != null)
        {
            MaskItemConveyorManager.Instance.NotifyItemPickedUp(this.gameObject);
        }
    }

    public override bool IsAllowInteract()
    {
        return true;
    }

    public override void OnHover()
    {
        Debug.Log("On Hover");
        base.OnHover();
    }

    override public void OnDrag(Vector2 dragPos)
    {
        base.OnDrag(dragPos);
        Debug.Log(animator);
        animator.Play("Drag");

        SetState(State.DRAGGED);
        transform.SetParent(null);
        transform.position = dragPos;
    }

    override public void OnDrop(Vector2 dragPos)
    {
        SetState(State.IDLE);
        if (!isInFace || isOverlapMaskObject)
        {
            Disappear();
        }
        else
        {
            animator.Play("Idle");
            base.OnDrop(dragPos);
            dropVfx.Play();
            if (itemType == MaskItemType.ChiliSlice && isInFace)
            {
                FaceController.Instance.Angry();
                return;
            }

            if (MaskItemConveyorManager.Instance != null)
            {
                MaskItemConveyorManager.Instance.RegisterFilledItem(this);
            }

            if (isInFace)
            {
                transform.SetParent(FaceController.Instance.maskObjectContainer);
            }
        }
    }

    public void Disappear()
    {
        animator.Play("Disappear");
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Face"))
        {
            isInFace = true;
        }

        if (collision.transform.CompareTag("Cucumber"))
        {
            isOverlapMaskObject = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Face"))
        {
            isInFace = false;
        }

        if (collision.transform.CompareTag("Cucumber"))
        {
            isOverlapMaskObject = false;
        }
    }
}
