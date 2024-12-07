using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AnimationEvent : MonoBehaviour
{
    public Ultimate ultimate;
    public Ultimate2 ultimate2;
    public PlayerMovement CharController;
    public PlayerSword sword;
    public EnemySword enemySword;
    public EnemyController EnemyCharController;
    public BossController bossController;
    public HealthSystem health;
    public bossattack1 bossattack1;
    public punchboss punch;
    public bookscript bookscript;

    public void Freeze() => CharController.CanFreeze();
    public void LightAttackEvent() => CharController.Attack2Event();
    public void LightAttackEvent2() => CharController.Attack3Event();

    public void IscolliderEnable() => EnemyCharController.ColliderEnabled();
    public void IscolliderDisable() => EnemyCharController.ColliderDisabled();
    public void EnemyAttack3() => EnemyCharController.StartLight();


    public void PlayerAttack1() => sword.DoAttack1();
    public void PlayerAttackU1() => sword.DoAttackU1();
   

    public void EnemyAttack() => enemySword.EnemyDoAttack();
    public void EnemyAttack2() => enemySword.EnemyDoAttack1();
    public void EnemyAttack4() => EnemyCharController.EnemyDoAttack2();
    public void Hit() => EnemyCharController.Hit1();
    public void BossHit() => bossController.Hit1();
    public void BossAttack1W() => bossController.Attack1EventW();
    public void BossAttack1() => bossController.Attack1Event();
    public void BossAttack2() => bossController.Attack2Event();
    public void BossAttack3() => bossController.Attack3Event();
    public void BossAttackP1() => punch.BossPunchAttack1();
    public void BossAttackP2() => punch.BossPunchAttack2();
    public void Ultimate() => ultimate.DoUltiamte();
    public void Ultimate2() => ultimate2.DoUltiamte2();
    public void Ultimate2Damage() => ultimate2.DoUltiamte2Damage();

    public void BossIceUltimate() => bossController.IceUltimateAttack();

    public void bookfunc() => bookscript.playertriggerenter2();
    public void bookfunc1() => bookscript.playertriggerexit2();
    //public void UltimateDefend() => 
}
