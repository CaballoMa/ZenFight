
public enum CharacterState
{
    Idle,
    Fighting,
    Dead
}

public enum DefendMode
{
    None,
    Doggy,
    Block,
    Attack
}

public enum AttackMode
{
    None,
    Up,
    Mid,
    Down
}

public enum DefendResult
{
    Back,
    Hurted,
    Success
}

public static class GlobalSetting
{
    public const int AniState_Defend_Doggy = 0;
    public const int AniState_Defend_Block = 1;
    public const int AniState_Defend_Attack = 2;

    public const int AniState_Attack_Up = 0;
    public const int AniState_Attack_Mid = 1;
    public const int AniState_Attack_Down = 2;

    public const string AniName_DefendIdx = "DefendIdx";
    public const string AniName_AttackIdx = "AttackIdx";
    public const string AniName_DieTrigger = "DieTrigger";
    public const string AniName_CanDie = "canDie";
    public const string AniName_CanHurt = "canHurt";
    public const string AniName_HasResult = "hasResult";
    public const string AniName_DefendTrigger = "DefendTrigger";
    public const string AniName_HurtedTrigger = "HurtedTrigger";
    public const string AniName_BackTrigger = "Back";
    public const string AniName_ExcuteTrigger = "Excute";
    public const string AniName_CanExcute = "canExcute";
    public const string AniName_CanBack = "canBack";
    public const string AniName_BeginAttackTrigger = "BeginAttackTrigger";
    public const string AniName_AttackTrigger = "AttackTrigger";

    public const string SceneName_Start = "Start";
    public const string SceneName_GameLevel = "GameLevel";
    public const string SceneName_GameLevel2 = "GameLevel2";
    public const string SceneName_Lose = "Lose";
    public const string SceneName_Win = "Win";
}
