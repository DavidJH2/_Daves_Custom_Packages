using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Deliverence
{
    public class ZombieBrain : DamageTaker
    {
        private ZombieTarget    target;
        private Rigidbody       rb;
        private Animator        animator;
        private CapsuleCollider _collider;

        private float angY = 0;

    
        void Start()
        {
            target    = FindObjectOfType<ZombieTarget>();
            rb        = GetComponent<Rigidbody>();
            animator  = GetComponent<Animator>();
            _collider = GetComponent<CapsuleCollider>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ;UpdateRotation();
        }

        private void LateUpdate()
        {
            UpdateRotation();
        }

        private Vector3 zombiePos;
        private Vector3 targatPos;
        private Vector3 delta;
        private float   ang;
    
    
        private void UpdateRotation()
        {
            zombiePos = transform.position;
            targatPos = target.transform.position;
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
            if (target)
            {
                // Gizmos.DrawLine(zombiePos, targatPos);

                var message = $"Angle: {(int) ang}\n";
                message += $"Rot: {rb.rotation.eulerAngles}";
                
                // Handles.Label(transform.position, message);
            }
        }

        public void KillZombie()
        {
            animator.applyRootMotion = false;
            animator.SetBool("Flying", true);
            _collider.height = .3f;
            _collider.radius = .3f;
            _collider.center = new Vector3(0, 0.36f, -0.75f);
        }
    }
}
