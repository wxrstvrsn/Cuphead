using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    /* TODO:
        Хэндлить стрельбу (вообще всю) не через триггеры, а через флаги (bool переменные в parametrs внутри Animator)
        Видимо, триггеры чисто для единоразовых акций я хуй знает уже башка не варит
        + покурить документацию про триггеры VS bool переменные в Animator
     */
    [SerializeField] private float attackCooldown;
    private Animator anim;
    private PlayerMovement playerMovement;
    private float coolDownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Z) /*&& coolDownTimer > attackCooldown*/)
        {
            if (anim.GetBool("runNormal")) // Если игрок бежит, выполняем анимацию атаки в беге
            {
                Debug.Log("Attack while running!");
                AttackAndRunStraight();
            }
            else // Если не бежит, выполняем обычную атаку
            {
                Debug.Log("Normal attack!");
                Attack();
            }
        }

        coolDownTimer += Time.deltaTime;
    }

    private void AttackAndRunStraight()
    {
        anim.SetTrigger("attack_and_run");
        // coolDownTimer = 0;
    }

    private void Attack()
    {
        anim.SetTrigger("attack");
        // coolDownTimer = 0;
    }
}