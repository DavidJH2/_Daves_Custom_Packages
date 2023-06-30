using System.Linq;
using _Deliverence.Scripts.Player;
using com.davidhopetech.core.Run_Time.Extensions;
using TMPro;
using UnityEngine;

namespace _Deliverence
{
    public class Explosion : MonoBehaviour
    {
        private GameObject   _playerGO;
        private ZombieTarget _player;
        void OnEnable()
        {
            var playerController = FindObjectOfType<DeliverancePlayerController>();
            _playerGO = playerController.gameObject;
            _player   = _playerGO.GetComponentInChildren<ZombieTarget>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void AddExplosiveForce(float force, float radius)
        {
            var ePos = transform.position;
            var pPos = _playerGO.transform.position;

            ePos.y = 0;
            pPos.y = 0;
            var dist = Vector3.Distance(ePos, pPos);
            if (dist < radius)
            {
                var per = (radius - dist) / radius;
                var damage = (int) (Mathf.Lerp(0.0f, force, per) * 10);
                // GameEngine.SetDebugText($"Damage: {damage}\n");
                _player.TakeDamage(damage);
            }
            
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
