using UnityEngine;

namespace _Deliverence
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private GameObject ExplosionPrefab;
    
        [SerializeField] private float minPower;
        [SerializeField] private float maxPower;
    
        [SerializeField] private float powerParticleSpeedRatio = 1;
    
        public float percent;
        public float power;
    
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnCollisionEnter(Collision other)
        {
            power = Mathf.Lerp(minPower, maxPower, percent);
            
            var explosionGO = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            var explosionPS = explosionGO.GetComponentInChildren<ParticleSystem>();
            var explosion   = explosionGO.GetComponent<Explosion>();
        
            explosion.AddExplosiveForce(power, power);

            ParticleSystem.MainModule main = explosionPS.main;
            main.startSpeed = Mathf.Lerp(minPower * powerParticleSpeedRatio, maxPower * powerParticleSpeedRatio, percent); 
        
            Destroy(gameObject);
        }
    }
}
