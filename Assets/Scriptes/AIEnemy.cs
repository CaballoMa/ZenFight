using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class AIEnemy : AIBase
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected CharacterState characterState;
    [SerializeField] protected AttackMode attackMode;
    
    [SerializeField] private UIQTEBar qteBar;
    [SerializeField] private GameObject qteExcuteBar;
    [SerializeField] private Button ExcuteButton;

    [SerializeField] protected bool isAttacking;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool isGettingResult;
    [SerializeField] private bool hasExcuteResult;

    [SerializeField] private float canAttackDistance;
    [SerializeField] private int randomMode;

    [SerializeField] private AudioSource beginAttack;
    [SerializeField] private AudioSource attack;
    [SerializeField] private AudioSource excute;
    [SerializeField] private AudioSource beExcuted;

    private Vector3 targetPos;
    private PlayerController playerController;
    private Animator playerAnimator;
    private float tempTimer;
    private bool excuteButtonOnClick = false;

    [SerializeField] private float attackBeginTime;

    [SerializeField] private float attackUpGapTime;
    [SerializeField] private float attackMidGapTime;
    [SerializeField] private float attackDownGapTime;

    [SerializeField] private float excuteSoundUpWaitTime;
    [SerializeField] private float excuteSoundMidWaitTime;
    [SerializeField] private float excuteSoundDownWaitTime;

    [SerializeField] private float beExcutedSoundUpWaitTime;
    [SerializeField] private float beExcutedSoundMidWaitTime;
    [SerializeField] private float beExcutedSoundDownWaitTime;

    [SerializeField] private float bulletTimeSpeed;
    
    void Start()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if(target == null)
        {
            target = FindObjectOfType<PlayerController>().GetComponent<CharacterBase>();
            targetPos = target.transform.position;
            playerController = target.GetComponent<PlayerController>();
            playerAnimator = target.GetComponent<Animator>();
        }

        if (qteBar == null)
        {
            qteBar = FindObjectOfType<UIQTEBar>();
        }

        ExcuteButton.onClick.AddListener(ExcuteButtonClick);
        characterState = CharacterState.Idle;
        attackMode = AttackMode.None;
        isAttacking = false;
        canAttack = false;
        isGettingResult = false;
        hasExcuteResult = false;
        tempTimer = 0;
    }

    private void ExcuteButtonClick()
    {
        if (!qteBar.gameObject.active || !qteBar.isExcuteMoving) return;
        excuteButtonOnClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        tempTimer += Time.deltaTime;
        if(!isAttacking && tempTimer > attackBeginTime)
        {
            BeginAttack();
        }

        if (isAttacking && characterState == CharacterState.Fighting)
        {
            if(Vector3.Distance(transform.position, targetPos) > canAttackDistance)
            {
                Vector3 movement = (targetPos - transform.position).normalized * speed * Time.deltaTime;
                transform.Translate(movement, Space.World);
            }
            else
            {
                Attack();
            }
        }

        if(!canAttack && !qteBar.isMoving && !isGettingResult && attackMode != AttackMode.None)
        {
            DetectDefend();
        }

        if(qteBar.isExcuteMoving)
        {
            if (!qteExcuteBar.active)
            {
                qteExcuteBar.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space) || excuteButtonOnClick)
            {
                qteBar.isExcuteMoving = false;
                excuteButtonOnClick = false;
            }

        }

        if(hasExcuteResult && !qteBar.isExcuteMoving)
        {
            hasExcuteResult = false;
            DetectExcute();
        }
    }

    private void Attack()
    {
        if(!canAttack)
        {
            return;
        }

        randomMode = Random.Range(0, 3);
        if (randomMode == 0)
        {
            animator.SetInteger(GlobalSetting.AniName_AttackIdx, GlobalSetting.AniState_Attack_Up);
            playerAnimator.SetInteger(GlobalSetting.AniName_AttackIdx, GlobalSetting.AniState_Attack_Up);
            animator.SetTrigger(GlobalSetting.AniName_AttackTrigger);
            attackMode = AttackMode.Up;
        }
        else if(randomMode == 1)
        {
            animator.SetInteger(GlobalSetting.AniName_AttackIdx, GlobalSetting.AniState_Attack_Mid);
            playerAnimator.SetInteger(GlobalSetting.AniName_AttackIdx, GlobalSetting.AniState_Attack_Mid);
            animator.SetTrigger(GlobalSetting.AniName_AttackTrigger);
            attackMode = AttackMode.Mid;
        }
        else if(randomMode == 2)
        {
            animator.SetInteger(GlobalSetting.AniName_AttackIdx, GlobalSetting.AniState_Attack_Down);
            playerAnimator.SetInteger(GlobalSetting.AniName_AttackIdx, GlobalSetting.AniState_Attack_Down);
            animator.SetTrigger(GlobalSetting.AniName_AttackTrigger);
            attackMode = AttackMode.Down;
        }

        if(!qteBar.gameObject.active)
        {
            qteBar.gameObject.SetActive(true);
        }

        if (!attack.isPlaying)
        {
            attack.Play();
        }
        animator.speed = bulletTimeSpeed;
        qteBar.isMoving = true;
        canAttack = false;
    }

    private void DetectDefend()
    {
        DefendMode defendMode = playerController.GetDefendMode();

        if (qteBar.CheckHit())
        {
            if(attackMode == AttackMode.Up)
            {
                if(defendMode == DefendMode.Doggy)
                {
                    //playerController.GetResult(DefendResult.Success);
                    GetResult(DefendResult.Hurted);
                }
                else if(defendMode == DefendMode.Attack)
                {
                    playerController.GetResult(DefendResult.Back);
                    GetResult(DefendResult.Back);
                }
                else if(defendMode == DefendMode.Block || defendMode == DefendMode.None)
                {
                    playerController.GetResult(DefendResult.Hurted);
                    GetResult(DefendResult.Success);
                }
            }
            else if(attackMode == AttackMode.Mid)
            {
                if (defendMode == DefendMode.Block)
                {
                    //playerController.GetResult(DefendResult.Success);
                    GetResult(DefendResult.Hurted);
                }
                else if (defendMode == DefendMode.Doggy)
                {
                    playerController.GetResult(DefendResult.Back);
                    GetResult(DefendResult.Back);
                }
                else if (defendMode == DefendMode.Attack || defendMode == DefendMode.None)
                {
                    playerController.GetResult(DefendResult.Hurted);
                    GetResult(DefendResult.Success);
                }
            }
            else if (attackMode == AttackMode.Down)
            {
                if (defendMode == DefendMode.Attack)
                {
                    //playerController.GetResult(DefendResult.Success);
                    GetResult(DefendResult.Hurted);
                }
                else if (defendMode == DefendMode.Block)
                {
                    playerController.GetResult(DefendResult.Back);
                    GetResult(DefendResult.Back);
                }
                else if (defendMode == DefendMode.Doggy || defendMode == DefendMode.None)
                {
                    playerController.GetResult(DefendResult.Hurted);
                    GetResult(DefendResult.Success);
                }
            }
        }
        else
        {
            target.GetDamage(damage);
            playerController.GetResult(DefendResult.Hurted);
            GetResult(DefendResult.Success);
        }
        animator.speed = 1;
        isGettingResult = true;
    }

    public void BeginAttack()
    {
        canAttack = true;
        isAttacking = true;
        characterState = CharacterState.Fighting;
        animator.SetTrigger(GlobalSetting.AniName_BeginAttackTrigger);
        //if(!beginAttack.isPlaying)
        //{
        //    beginAttack.Play();
        //}
    }

    public void ReturnToIdle()
    {
        canAttack = true;
        isGettingResult = false;
        qteBar.isMoving = true;
        animator.SetBool(GlobalSetting.AniName_CanExcute, false);
        animator.SetBool(GlobalSetting.AniName_CanBack, false);
        animator.SetBool(GlobalSetting.AniName_HasResult, false);
        animator.SetBool(GlobalSetting.AniName_CanHurt, false);
    }

    public void GetResult(DefendResult result)
    {

        if (result == DefendResult.Success)
        {
            //if (playerController.isDead())
            //{
            //    animator.SetBool(GlobalSetting.AniName_CanExcute, true);
            //    animator.SetBool(GlobalSetting.AniName_CanBack, false);
            //    animator.SetTrigger(GlobalSetting.AniName_ExcuteTrigger);
            //}
            //else
            //{
            //    animator.SetBool(GlobalSetting.AniName_CanExcute, false);
            //    animator.SetBool(GlobalSetting.AniName_CanBack, true);
            //}
            animator.SetBool(GlobalSetting.AniName_CanExcute, true);
            animator.SetBool(GlobalSetting.AniName_CanBack, false);
            animator.SetTrigger(GlobalSetting.AniName_ExcuteTrigger);

            if (attackMode == AttackMode.Up)
            {
                StartCoroutine(PlayBeExcutedSound(beExcutedSoundUpWaitTime));
            }
            else if (attackMode == AttackMode.Mid)
            {
                StartCoroutine(PlayBeExcutedSound(beExcutedSoundMidWaitTime));
            }
            else if (attackMode == AttackMode.Down)
            {
                StartCoroutine(PlayBeExcutedSound(beExcutedSoundDownWaitTime));
            }
            animator.SetBool(GlobalSetting.AniName_HasResult, true);
        }
        else if (result == DefendResult.Back)
        {
            //animator.SetBool(GlobalSetting.AniName_CanExcute, false);
            animator.SetBool(GlobalSetting.AniName_CanBack, true);
            animator.SetTrigger(GlobalSetting.AniName_BackTrigger);
            attack.Play();
            if(attackMode == AttackMode.Up)
            {
                StartCoroutine(AttackAgain(attackUpGapTime));
            }
            else if(attackMode == AttackMode.Mid)
            {
                StartCoroutine(AttackAgain(attackMidGapTime));
            }
            else if( attackMode == AttackMode.Down)
            {
                StartCoroutine(AttackAgain(attackDownGapTime));
            }
            animator.SetBool(GlobalSetting.AniName_HasResult, true);
        }
        else if (result == DefendResult.Hurted)
        {
            hasExcuteResult = true;
            qteBar.isExcuteMoving = true;
            if (attackMode == AttackMode.Down)
            {
                playerAnimator.SetTrigger(GlobalSetting.AniName_ExcuteTrigger);
                playerAnimator.speed = Mathf.Max(0.1f, bulletTimeSpeed - 0.1f);
            }
            else if(attackMode == AttackMode.Up)
            {
                animator.SetTrigger(GlobalSetting.AniName_HurtedTrigger);
                animator.SetBool(GlobalSetting.AniName_CanHurt, true);
            }
            else if(attackMode == AttackMode.Mid)
            {
                playerAnimator.speed = Mathf.Max(0.1f, bulletTimeSpeed - 0.1f);
            }
        }
        
        
    }

    void DetectExcute()
    {
        playerAnimator.speed = 1;
        if (qteBar.CheckExcuteHit())
        {
            qteBar.isExcuteMoving = false;
            if (attackMode == AttackMode.Up)
            {
                StartCoroutine(PlayExcuteSound(excuteSoundUpWaitTime));
            }
            else if (attackMode == AttackMode.Mid)
            {
                StartCoroutine(PlayExcuteSound(excuteSoundMidWaitTime));
            }
            else if (attackMode == AttackMode.Down)
            {
                StartCoroutine(PlayExcuteSound(excuteSoundDownWaitTime));
            }
            playerController.GetResult(DefendResult.Success);
            Die();
        }
        else
        {
            qteBar.isExcuteMoving = false;
            if (attackMode != AttackMode.Up)
            {
                animator.SetTrigger(GlobalSetting.AniName_HurtedTrigger);
                animator.SetBool(GlobalSetting.AniName_CanHurt, true);
            }
            playerController.GetResult(DefendResult.Back);
            if(attackMode == AttackMode.Down)
            {
                StartCoroutine(AttackAgain(2f));
            }
            else
            {
                StartCoroutine(AttackAgain(1f));
            }
        }
        animator.SetBool(GlobalSetting.AniName_HasResult, true);
    }

    protected override void Die()
    {
        base.Die();
        characterState = CharacterState.Dead;
        animator.SetTrigger(GlobalSetting.AniName_DieTrigger);
        animator.SetBool(GlobalSetting.AniName_CanDie, true);
        GameManager.Instance.WinGame(4f);
    }

    private IEnumerator AttackAgain(float attackGapTime)
    {
        yield return new WaitForSeconds(attackGapTime);

        ReturnToIdle();
        canAttack = true;
        Attack();
    }

    private IEnumerator PlayExcuteSound(float time)
    {
        yield return new WaitForSeconds(time);

        excute.Play();
    }

    private IEnumerator PlayBeExcutedSound(float time)
    {
        yield return new WaitForSeconds(time);

        beExcuted.Play();
    }
}
