using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : CharacterBase
{
    [SerializeField] protected float speed;
    [SerializeField] protected CharacterBase target;
    [SerializeField] protected float damage;
}
