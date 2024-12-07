using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class punchboss : MonoBehaviour
{
    public BoxCollider punch1;
    public BoxCollider punch2;
    punchboss punch;
    public Animator anim;

    private void Awake()
    {

        punch = GetComponent<punchboss>();
        anim = GetComponentInChildren<Animator>();
    }
    public void BossPunchAttack1()
    {
        punch1.enabled = true;
        StartCoroutine(BossHideCollider());
    }
    public void BossPunchAttack2()
    {
        punch2.enabled = true;
        StartCoroutine(BossHideCollider1());
    }
    IEnumerator BossHideCollider()
    {
        punch1.enabled = true;
        yield return new WaitForSeconds(0.15f);
        punch1.enabled = false;
    }
    IEnumerator BossHideCollider1()
    {
        punch2.enabled = true;
        yield return new WaitForSeconds(0.15f);
        punch2.enabled = false;
    }
}
