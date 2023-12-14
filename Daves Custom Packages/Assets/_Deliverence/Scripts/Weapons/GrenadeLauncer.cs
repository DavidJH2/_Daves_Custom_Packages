using com.davidhopetech.core.Run_Time.DTH.Scripts.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Deliverence
{
    public class GrenadeLauncer : DHTInteractable
    {
        [SerializeField] private GameObject grenadePrefab;
        [SerializeField] private Transform  launchPoint;

        [SerializeField] private float grenadeMinSpeed = 10;
        [SerializeField] private float grenadeMaxSpeed = 35;

        //[SerializeField] private VisualEffect _visualEffect;
        public ParticleSystem _particleSystem;

        [SerializeField] private float ParitcleStartSpeed             = 0.4f;
        [SerializeField] private float MaxGreandeChargeTime           = 0.8f;
        [SerializeField] private float ParitcleStartSpeedIncreaseRate = 2f;

        public AudioSource ChargeSound;
    
        [SerializeField] private TextMeshProUGUI debugTMP;


        private float           _chargeTime;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void ResetParticleSystem()
        {
            _chargeTime = 0;
        }

        // Update is called once per frame
        public virtual void Update()
        {
        }

        public void ChargeGrenade()
        {
            ParticleSystem.MainModule  psMain  = _particleSystem.main;
            ParticleSystem.ShapeModule psShape = _particleSystem.shape;

            var elapsedTime = Time.fixedDeltaTime;
            _chargeTime = Mathf.Min(_chargeTime + elapsedTime, MaxGreandeChargeTime);

            psMain.startSpeed = ParitcleStartSpeed + ParitcleStartSpeedIncreaseRate * _chargeTime;
        }

        public override void Activate()
        {
            LaunchGrenade();
        }

        public void LaunchGrenade()
        {
            // tmp.text += "Launch Grenade!\n";
            var percent = _chargeTime / MaxGreandeChargeTime;


            var newGrenadeGO = Instantiate(grenadePrefab, launchPoint.position, Quaternion.identity);
            var newGrenade   = newGrenadeGO.GetComponent<Grenade>();
            newGrenade.percent = percent;
        
            var rb = newGrenadeGO.GetComponent<Rigidbody>();

            var speed = Mathf.Lerp(grenadeMinSpeed, grenadeMaxSpeed, percent);
            
            // GameEngine.SetDebugText($"Speed: {speed}\n");
            var newVel = transform.forward * speed;
        
            rb.velocity = newVel;
        }
    }
}