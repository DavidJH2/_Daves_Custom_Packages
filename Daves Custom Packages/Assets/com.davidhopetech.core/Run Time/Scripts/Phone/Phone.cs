using UnityEngine;


namespace com.davidhopetech.phone.run_time.scripts
{
	public class Phone : MonoBehaviour
	{
		Vector3 acceleration_m;
		Quaternion rawGyroAtt_m;
		Quaternion calibratedGyroAtt_m;
		Vector3 rotRate_m;
		Quaternion invGyroAtt_m;
		Quaternion calibration;

		int frameCount = 0;

		// Start is called before the first frame update
		void Start()
		{
			Input.gyro.enabled = true;
			calibration = Quaternion.identity;
		}

		// Update is called once per frame
		void Update()
		{
			frameCount++;

			rotRate_m = Input.gyro.rotationRate;

			// ----------  Calculate Phone Attitude  ----------

			rawGyroAtt_m = Input.gyro.attitude;

			// Correct Orientation
			rawGyroAtt_m = Quaternion.Euler(new Vector3(-90, 0, 0)) * rawGyroAtt_m;

			// Correct Handedness
			rawGyroAtt_m.x = -rawGyroAtt_m.x;
			rawGyroAtt_m.y = -rawGyroAtt_m.y;

			if (frameCount < 20)
			{
				calibration = Quaternion.Inverse(rawGyroAtt_m);
				//Debug.Log("Calibration: " + calibration);
				calibratedGyroAtt_m = Quaternion.identity;
			}
			else
			{
				calibratedGyroAtt_m = calibration * rawGyroAtt_m;
			}

			invGyroAtt_m = Quaternion.Inverse(calibratedGyroAtt_m);



			// ----------  Calculate Phone Acceleration  ----------

			acceleration_m = Input.acceleration;
			acceleration_m.z = -acceleration_m.z;

			// Correct Orientation
			Quaternion phoneSpace2WorldSpace;

			phoneSpace2WorldSpace = CalibratedGyroAttitude;
			acceleration_m = phoneSpace2WorldSpace * acceleration_m;

			//Debug.Log("\t\tInput Attitude: " + Input.gyro.attitude + "\t\tInput Accel: " + Input.acceleration + "Correction: " + phoneSpace2WorldSpace);
		}


		public Vector3 accerlation
		{
			get { return acceleration_m; }
		}


		public Quaternion RawGyroAtt
		{
			get { return rawGyroAtt_m; }
		}


		public Quaternion CalibratedGyroAttitude
		{
			get { return calibratedGyroAtt_m; }
		}

		public Quaternion InvGyroAtt
		{
			get { return invGyroAtt_m; }
		}

		public Vector3 RotRate
		{
			get { return rotRate_m; }
		}
	}
}