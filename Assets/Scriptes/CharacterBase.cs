using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] protected float hp;
    [SerializeField] protected float maxhp;

    virtual public void GetDamage(float dmg)
    {
        hp -= dmg;
        if(hp < 0) {  hp = 0; }
    }

    public bool isDead()
    {
        return hp <= 0;
    }

    virtual protected void Die()
    {

    }
}
