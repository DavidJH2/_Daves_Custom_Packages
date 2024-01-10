using System.Collections.Generic;
using System.Linq;
using com.davidhopetech.core.Run_Time.Extensions;
using UnityEditor;
using UnityEngine;

namespace _Deliverence
{
    public class ZombieBrain : DamageTaker
    {
        [SerializeField] private int   startingHealth = 10;

        private List<ZombieTarget> targets;
        private ZombieTarget       currentTarget;
        private Rigidbody          rb;
        private Animator           animator;
        private CapsuleCollider    _collider;

        private float angY = 0;
        public  int   health;


        void Start()
        {
            targets    = FindObjectsOfType<ZombieTarget>().ToList();
            rb        = GetComponent<Rigidbody>();
            animator  = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider>();

            health = startingHealth;
        }


        public void HurtPlayer()
        {
            currentTarget.TakeDamage(10);
        }


        void Update()
        {
            var dist = transform.Dist(currentTarget.transform);
            // GameEngine.SetDebugText($"Dist: {dist}");
            
            if (currentTarget.Health>0 && dist < currentTarget.Radius)
            {
                animator.SetInteger("Attack", 1);
            }
            else
            {
                animator.SetInteger("Attack", 0);
            }
        }
        
        
        void FixedUpdate()
        {
            UpdateRotation();
        }

        private void LateUpdate()
        {
            SelectTarget();
            UpdateRotation();
        }


        private float selectTargetTimer = 0;
        private void SelectTarget()
        {
            selectTargetTimer -= Time.deltaTime;
            if (selectTargetTimer<0)
            {
                var orderedTargets = targets.OrderBy(o => o.Dist(transform.position));
                foreach (var target in orderedTargets)
                {
                    if (target.Health > 0)
                    {
                        currentTarget     = target;
                        break;
                    }
                }
                selectTargetTimer = 1;
            }
        }

        private Vector3 zombiePos;
        private Vector3 targatPos;
        private Vector3 delta;
        private float   ang;
    
    
        private void UpdateRotation()
        {
            
            
            zombiePos = transform.position;
            targatPos = currentTarget.transform.position;
            delta     = targatPos - zombiePos;
            delta.y   = 0;

            ang = Vector3.SignedAngle(rb.transform.forward, delta, Vector3.up);

        
        
            // var deltaRot = Quaternion.Euler(0, ang * .008f, 0);
            // var rot      = rb.rotation;

            angY += ang * Time.fixedDeltaTime * 2f;

            //var newRot = rot * deltaRot;


            var newRot = Quaternion.Euler(0, angY, 0);
            transform.rotation = newRot;
            rb.angularVelocity = Vector3.zero;

#if UNITY_EDITOR
            if (Selection.Contains(gameObject))
            {
                
            }
#endif
        }

        private void OnDrawGizmos()
        {
            if (currentTarget)
            {
#if UNITY_EDITOR
                // Gizmos.DrawLine(zombiePos, targatPos);

                var message = "";
                
                // message =  $"Angle: {(int)ang}\n";
                // message += $"Rot: {rb.rotation.eulerAngles}";

                //message = $"Target: {currentTarget.name}";
                
                Handles.Label(transform.position, message);
#endif
            }
        }

        public void KillZombie()
        {
            animator.applyRootMotion = false;
            animator.SetBool("Flying", true);
            _collider.height = .3f;
            _collider.radius = .3f;
            _collider.center = new Vector3(0, 0.36f, -0.75f);
            health           = 0;
        }
    }
}
