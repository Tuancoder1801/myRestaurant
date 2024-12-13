using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Unity.VisualScripting;

public class Player : Character
{
    public Joystick joystick;

    //public Transform itemHolderTransform;

    //public int NumOfItemsHolding;

    public int maxItems = 3;
    public List<Transform> itemPositions;
    public List<Transform> cariedItems = new List<Transform>();

    public bool isHolding = false;

    private bool isMoving = false;
    private Vector3 moveVector;
    private float valueVertical;
    private float valueHorizontal;

    public override void Awake()
    {
        base.Awake();
        joystick = FindObjectOfType<Joystick>();
    }

    public override void Update()
    {
        CheckInput();
        base.Update();
    }

    private void CheckInput()
    {
        valueVertical = joystick.Vertical;
        valueHorizontal = joystick.Horizontal;

        isMoving = valueVertical != 0 || valueHorizontal != 0;
    }

    #region Idle

    public override void BeginIdle()
    {
        base.BeginIdle();
    }

    public override void UpdateIdle()
    {
        if (state == CharacterState.Idle)
        {
            if (isMoving)
            {
                ChangeState(CharacterState.Run);
                return;
            }
        }
    }

    public override void BeginIdleHold()
    {
        base.BeginIdleHold();
    }

    public override void UpdateIdleHold()
    {
        if (state == CharacterState.IdleHold)
        {
            if (isMoving)
            {
                ChangeState(CharacterState.RunHold);
                return;
            }

            if(!isHolding)
            {
                ChangeState(CharacterState.Idle);
                return;
            }
        }
    }

    #endregion  

    #region Run

    public override void BeginRun()
    {
        base.BeginRun();
    }

    public override void UpdateRun()
    {
        if (state == CharacterState.Run)
        {
            if (!isMoving)
            {
                ChangeState(CharacterState.Idle);
                return;
            }

            if (isHolding)
            {
                ChangeState(CharacterState.RunHold);
                return;
            }
           
            HandleMovement();
        }
    }

    public override void BeginRunHold()
    {
        base.BeginRunHold();
    }

    public override void UpdateRunHold()
    {
        if (state == CharacterState.RunHold)
        {
            if (!isMoving)
            {
                ChangeState(CharacterState.IdleHold);
                return;
            }

            if(!isHolding && isMoving)
            {
                ChangeState(CharacterState.Run);
                return;
            }

            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        moveVector = Vector3.zero;
        moveVector.x = valueHorizontal * moveSpeed * Time.deltaTime;
        moveVector.z = valueVertical * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);

        Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    #endregion

    public void AddNewItem(List<Transform> itemsToAdd)
    {
        isHolding = true;
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < itemsToAdd.Count; i++)
        {
            if (cariedItems.Count >= maxItems)
            {
                Debug.Log("Reached maximum item limit!"); // Thông báo khi đạt giới hạn
                break;
            }

            int index = cariedItems.Count;
            Transform itemToAdd = itemsToAdd[index];
            cariedItems.Add(itemToAdd);

            sequence.Append(
            itemToAdd.DOJump(itemPositions[index].position, 2f, 1, 0.5f).OnComplete(() =>
            {
                // Khi hoàn thành DOJump, gán item vào itemHolderTransform
                itemToAdd.SetParent(itemPositions[index]);
                itemToAdd.localPosition = Vector3.zero;
                itemToAdd.localRotation = Quaternion.identity;
                itemToAdd.localScale = Vector3.one;
            })
            );
        }
    }
}
