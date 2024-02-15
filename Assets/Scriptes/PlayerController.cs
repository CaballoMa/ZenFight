using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterBase
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected CharacterState characterState;
    [SerializeField] protected DefendMode defendMode;

    [SerializeField] protected float damage;
    [SerializeField] private bool canDefend;

    [SerializeField] private UIQTEBar qteBar;

    [SerializeField] private Button DodgyButton;
    [SerializeField] private Button BlockButton;
    [SerializeField] private Button AttackButton;

    private bool dodgyButtonOnClick = false;
    private bool blockButtonOnClick = false;
    private bool attackButtonOnClick = false;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if(qteBar == null)
        {
            qteBar = FindObjectOfType<UIQTEBar>();
        }

        defendMode = DefendMode.None;
        characterState = CharacterState.Idle;
        canDefend = true;

        DodgyButton.onClick.AddListener(DodgyButtonClick);
        BlockButton.onClick.AddListener(BlockButtonClick);
        AttackButton.onClick.AddListener(AttackButtonClick);
    }

    private void DodgyButtonClick()
    {
        if (!canDefend) return;
        dodgyButtonOnClick = true;
    }

    private void BlockButtonClick()
    {
        if (!canDefend) return;
        blockButtonOnClick = true;
    }

    private void AttackButtonClick()
    {
        if (!canDefend) return;
        attackButtonOnClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(qteBar.isMoving)
        {
            Defend();
        }
    }

    private void Defend()
    {
        if (!canDefend) return;
        if (Input.GetKeyDown(KeyCode.A) || dodgyButtonOnClick)
        {
            animator.SetInteger(GlobalSetting.AniName_DefendIdx,GlobalSetting.AniState_Defend_Doggy);
            animator.SetTrigger(GlobalSetting.AniName_DefendTrigger);
            defendMode = DefendMode.Doggy;
            characterState = CharacterState.Fighting;
            qteBar.isMoving = false;
            canDefend = false;
            dodgyButtonOnClick = false;
        }
        else if(Input.GetKeyDown(KeyCode.S) || blockButtonOnClick)
        {
            animator.SetInteger(GlobalSetting.AniName_DefendIdx, GlobalSetting.AniState_Defend_Block);
            animator.SetTrigger(GlobalSetting.AniName_DefendTrigger);
            defendMode = DefendMode.Block;
            characterState = CharacterState.Fighting;
            qteBar.isMoving = false;
            canDefend = false;
            blockButtonOnClick = false;
        }
        else if (Input.GetKeyDown(KeyCode.D) || attackButtonOnClick)
        {
            animator.SetInteger(GlobalSetting.AniName_DefendIdx, GlobalSetting.AniState_Defend_Attack);
            animator.SetTrigger(GlobalSetting.AniName_DefendTrigger);
            defendMode = DefendMode.Attack;
            characterState = CharacterState.Fighting;
            qteBar.isMoving = false;
            canDefend = false;
            attackButtonOnClick = false;
        }
       
    }

    public void GetResult(DefendResult result)
    {
        Debug.Log("Player Result: " + result);
        if(result == DefendResult.Success)
        {
            animator.SetBool(GlobalSetting.AniName_CanExcute, true);
            if (defendMode != DefendMode.Attack)
            {
                animator.SetTrigger(GlobalSetting.AniName_ExcuteTrigger);
            }
        }
        else if(result == DefendResult.Back)
        {
            //animator.SetBool(GlobalSetting.AniName_CanExcute, false);
            ReturnToIdle();
        }
        else if(result == DefendResult.Hurted)
        {
            //if(isDead())
            //{
            //    Die();
            //}
            //else
            //{
            //    animator.SetTrigger(GlobalSetting.AniName_HurtedTrigger);
            //}
            Die();
        }
    }

    public void ReturnToIdle()
    {
        canDefend = true;
        defendMode = DefendMode.None;
        //characterState = CharacterState.Idle;
        animator.SetBool(GlobalSetting.AniName_CanExcute, false);
    }

    protected override void Die()
    {
        base.Die();
        characterState = CharacterState.Dead;
        animator.SetBool(GlobalSetting.AniName_CanDie, true);
        animator.SetTrigger(GlobalSetting.AniName_DieTrigger);
        GameManager.Instance.LoseGame(4f);
    }

    public DefendMode GetDefendMode() { return defendMode; }
}
