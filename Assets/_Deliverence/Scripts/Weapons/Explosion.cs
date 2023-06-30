using System.Linq;
using _Deliverence.Scripts.Player;
using com.davidhopetech.core.Run_Time.Extensions;
using TMPro;
using UnityEngine;

namespace _Deliverence
{
    public class Explosion : MonoBehaviour
    {
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void AddExplosiveForce(float force, float radius)
        {
            var damageTakers = FindObjectsOfType<DamageTaker>();

            if (damageTakers.Length == 0)
            {
                return;
            }

            var inRangeDamageTakers  = damageTakers.Where(o => o.transform.Dist(transform)<radius).ToList();
        
            // GameEngine.SetDebugText($"Power: {force}\nRadius: {radius}\nNum InRangeDamageTakers: {inRangeDamageTakers.Count()}");

            foreach (var damageTaker in inRangeDamageTakers)
            {
                var zombie   = damageTaker.GetComponent<ZombieBrain>();
                if (zombie)
                {
                    zombie.KillZombie();
                }

                var rb   = damageTaker.GetComponent<Rigidbody>();
                rb.AddExplosionForce(force * 100, transform.position, radius);
            }
        }
    }
}
