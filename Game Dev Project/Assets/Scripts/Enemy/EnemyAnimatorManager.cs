using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    public EnemyLocomotionManager enemyLocomotionManager;
    public EnemyManager enemyManager;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyLocomotionManager = enemyManager.enemyLocomotionManager;
    }
    private void OnAnimatorMove()
    {
        if (enemyManager.enemyLocomotionManager.navMeshAgent.enabled == false)
        {
            float delta = Time.deltaTime;
            enemyLocomotionManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyLocomotionManager.enemyRigidBody.velocity = velocity;
        }
    }
}
