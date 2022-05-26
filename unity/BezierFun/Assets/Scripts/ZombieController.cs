using Assets.Scripts.RuntimeChecks;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls a zombie.
/// </summary>
public class ZombieController : MonoBehaviour
{
    [InspectorName("Walk Speed")]
    public float walkSpeed = 0.5f;

    [InspectorName("Run Speed")]
    public float runSpeed = 3.0f;

    [InspectorName("Smell Distance")]
    public float smellDistance = 15f;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform player;

    private void Awake()
    {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.animator = this.GetComponent<Animator>();
        this.player = SceneManager.GetActiveScene().GetRootGameObjects().Single(o => o.tag == "Player").transform;

        Field.AssertNotNull(this.navMeshAgent, nameof(this.navMeshAgent));
        Field.AssertNotNull(this.animator, nameof(this.animator));
        Field.AssertNotNull(this.player, nameof(this.player));
    }

    private void Update()
    {
        // Navigation.
        if (Distance(this.transform.position, player.position) < this.smellDistance)
        {
            this.navMeshAgent.speed = this.runSpeed;
            this.navMeshAgent.SetDestination(this.player.position);
        }
        else
        {
            this.navMeshAgent.speed = this.walkSpeed;
        }

        this.navMeshAgent.isStopped = Distance(this.transform.position, this.navMeshAgent.destination) < 0.5f;

        // Play animation.
        if (this.navMeshAgent.velocity.magnitude > 1f)
        {
            this.animator.Play("Z_Run");
        }
        else if (this.navMeshAgent.velocity.magnitude > 0.1f)
        {
            this.animator.Play("Z_Walk");
        }
        else
        {
            this.animator.Play("Z_Idle");
        }
    }

    private static float Distance(Vector3 pointA, Vector3 pointB)
    {
        return (pointA - pointB).magnitude;
    }
}
