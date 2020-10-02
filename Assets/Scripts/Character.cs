using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningToEnemy,
        RunningFromEnemy,
        BeginAttack,
        Attack,
        BeginShoot,
        Shoot,
        BeginDeath,
        Death,
    }

    public enum Weapon
    {
        Pistol,
        Bat,
        Fist,
    }

    Animator animator;
    State state;

    public Weapon weapon;
    public Character target;
    public float runSpeed;
    public float distanceFromEnemy;
    Vector3 originalPosition;
    Quaternion originalRotation;
    int health;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        state = State.Idle;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        health = 100;
    }

    public void SetState(State newState)
    {
        state = newState;
    }

    [ContextMenu("Attack")]
    void AttackEnemy()
    {
        switch (weapon) {
            case Weapon.Fist:
            case Weapon.Bat:
                state = State.RunningToEnemy;
                break;
            case Weapon.Pistol:
                state = State.BeginShoot;
                break;
        }
    }

    public void HitEnemy()
    {
        target.Hit();
    }

    void Hit()
    {
        if (health <= 0)
        {
            return;
        }
        health -= 50;
        SetState(State.BeginDeath);
    }

    public void Hurt()
    {
        if (health > 0)
        {
            animator.SetTrigger("Hurt");
            SetState(State.Idle);
        }
    }

    bool RunTowards(Vector3 targetPosition, float distanceFromTarget)
    {
        Vector3 distance = targetPosition - transform.position;
        if (distance.magnitude < 0.00001f) {
            transform.position = targetPosition;
            return true;
        }

        Vector3 direction = distance.normalized;
        transform.rotation = Quaternion.LookRotation(direction);

        targetPosition -= direction * distanceFromTarget;
        distance = (targetPosition - transform.position);

        Vector3 step = direction * runSpeed;
        if (step.magnitude < distance.magnitude) {
            transform.position += step;
            return false;
        }

        transform.position = targetPosition;
        return true;
    }

    void FixedUpdate()
    {
        switch (state) {
            case State.Idle:
                transform.rotation = originalRotation;
                animator.SetFloat("Speed", 0.0f);
                break;

            case State.RunningToEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(target.transform.position, distanceFromEnemy))
                    state = State.BeginAttack;
                break;

            case State.RunningFromEnemy:
                animator.SetFloat("Speed", runSpeed);
                if (RunTowards(originalPosition, 0.0f))
                    state = State.Idle;
                break;

            case State.BeginAttack:
                animator.SetFloat("Speed", 0.0f);
                if (weapon == Weapon.Fist)
                {
                    animator.SetTrigger("FistAttack");
                } else
                {
                    animator.SetTrigger("MeleeAttack");
                }
                
                state = State.Attack;
                break;

            case State.Attack:
                break;

            case State.BeginShoot:
                animator.SetTrigger("Shoot");
                state = State.Shoot;
                break;

            case State.Shoot:
                break;

            case State.BeginDeath:
                animator.SetTrigger("Death");
                state = State.Death;
                break;

            case State.Death:
                break;
        }
    }
}
