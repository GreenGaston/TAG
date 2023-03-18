using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class CameraScript : MonoBehaviour
{
	

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 90.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -90.0f;
	[Tooltip("How fast the camera moves up and down")]
	public float RotationSpeed =1f;


	public float xRotation = 0f;
	public float yRotation = 0f;
	private Transform _transform;




	// cinemachine
	private float _cinemachineTargetPitch;


	



#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	private PlayerInput _playerInput;
#endif
	
	private StarterAssetsInputs _input;

	private const float _threshold = 0.01f;

	private bool IsCurrentDeviceMouse
	{
		get
		{
			#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			return _playerInput.currentControlScheme == "KeyboardMouse";
			#else
			return false;
			#endif
		}
	}


	private void Start()
	{
				
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();

#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

		//CinemachineCameraTarget = GameObject.FindGameObjectWithTag("CinemachineTarget");
		_transform = transform.parent.transform;
	}




	private void Update()
	{
		CameraRotation();

	}




	

	private void CameraRotation()
	{
		// if there is an input
		if (_input.look.sqrMagnitude >= _threshold)
		{
			//Don't multiply mouse input by Time.deltaTime
			float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
			
			_cinemachineTargetPitch += _input.look.y * RotationSpeed * deltaTimeMultiplier;
			float _rotationVelocity = _input.look.x * RotationSpeed * deltaTimeMultiplier;

			// clamp our pitch rotation
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Update Cinemachine camera target pitch
			CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, xRotation , yRotation);

			// rotate the player left and right
			_transform.Rotate(Vector3.up * _rotationVelocity);
		}
	}



	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

}

