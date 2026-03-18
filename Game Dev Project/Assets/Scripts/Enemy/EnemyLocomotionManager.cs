using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocomotionManager : MonoBehaviour
{

public EnemyManager enemyManager;
public LayerMask detectionLayer;
public CharacterStats currentTarget;

public Rigidbody enemyRigidBody;

public float distanceFromTarget;
public float stoppingDistance = 1f;
public float rotationSpeed = 25;
public float speed = 3;

public UnityEngine.AI.NavMeshAgent navMeshAgent;
EnemyAnimatorManager enemyAnimatorManager;

public void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        detectionLayer = LayerMask.GetMask("Character");
        navMeshAgent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        enemyRigidBody = GetComponent<Rigidbody>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
    }
public void Start()
    {
      navMeshAgent.enabled = false;
      enemyRigidBody.isKinematic = false;
    }
public void handleDetection()
    {
       Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for(int i = 0 ;i < colliders.Length; i++)
        {
         CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();   

         if (characterStats != null)
         {
            Vector3 targetDirection = characterStats.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
             {
                currentTarget = characterStats;
             }
         
         }
        }
    }

public void handleMoveToTarget()
    {
       Vector3 targetDirection = currentTarget.transform.position - transform.position;
       distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
       float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
       if(enemyManager.isPerformingAction)
       {
        enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
        navMeshAgent.enabled = false;
        }
        else
        {
        if(distanceFromTarget > stoppingDistance)
        {
           enemyManager.enemyAnimatorManager.anim.SetFloat("Horizontal", 1, 0.1f, Time.deltaTime);

           targetDirection.y = 0;
           targetDirection.Normalize();

           
           targetDirection *= speed;
           Vector3 projectedVelocity = Vector3.ProjectOnPlane(targetDirection, transform.up);
           enemyRigidBody.velocity = projectedVelocity;
        }else if(distanceFromTarget <= stoppingDistance)
        {
           enemyManager.enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
        }

        handleRotateTowardsTarget();

        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

}
}

public void handleRotateTowardsTarget()
    {
        if(enemyManager.isPerformingAction)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if(direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyRigidBody.velocity;
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(currentTarget.transform.position);
            enemyRigidBody.velocity = targetVelocity;
            transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
        }
    }
}
