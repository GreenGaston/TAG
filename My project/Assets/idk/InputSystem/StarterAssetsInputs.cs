using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif


public class StarterAssetsInputs : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public Vector2 scroll;
	public bool jump;
	public bool sprint;
	public bool crouch;
	public bool interact;
	public bool pause;
	public bool slide;
	public bool leftMouse;
	public bool rightMouse;
	public bool paused;
	
	
	

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

	public GameObject menuObject;
	private GameObject menuInstance;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	public void OnMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}

	public void OnLook(InputValue value)
	{
		if(cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
		else{
			//0 vector
			LookInput(Vector2.zero);
		}
	}

	public void OnJump(InputValue value)
	{
		
		JumpInput(value.isPressed);
	}

	public void OnSprint(InputValue value)
	{
		SprintInput(value.isPressed);
	}

	public void OnScroll(InputValue value)
	{
		ScrollInput(value.Get<Vector2>());

	}
	public void OnMouseLeft(InputValue value)
	{
		// Debug.Log("Left Mouse:" + value.isPressed );
		LeftMouse(value.isPressed);
	}
	public void OnMouseRight(InputValue value)
	{
		RightMouse(value.isPressed);
	}

	public void OnSliding(InputValue value)
	{
		SlideInput(value.isPressed);
	}

	public void OnPause(InputValue value)
	{
		PauseInput(value.isPressed);
	}
#endif


	void Start(){
		
		PlayerInput _playerInput = GetComponent<PlayerInput>();
		//enable this object
		_playerInput.enabled = true;
	}
	void Update(){
		if(leftMouse){
			if(Mouse.current.leftButton.wasReleasedThisFrame){
				//Debug.Log("Left Mouse Released");
				LeftMouse(false);
			}
		}
		if(rightMouse){
			if(Mouse.current.rightButton.wasReleasedThisFrame){
				//Debug.Log("Right Mouse Released");
				RightMouse(false);
			}
		}
		if(slide){
			//c key
			if(Keyboard.current.cKey.wasReleasedThisFrame){
				//Debug.Log("C Key Released");
				SlideInput(false);
			}
		}

	}

	public void LeftMouse(bool newLeftMouseState)
	{
		leftMouse = newLeftMouseState;
	}
	public void RightMouse(bool newRightMouseState)
	{
		rightMouse = newRightMouseState;
	}
	public void MoveInput(Vector2 newMoveDirection)
	{
		move = newMoveDirection;
	} 

	public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}

	public void JumpInput(bool newJumpState)
	{
		jump = newJumpState;
	}

	public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}
	
	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}

	private void CrouchInput(bool newCrouchState)
	{
		crouch = newCrouchState;
	}
	private void ScrollInput(Vector2 newScrollState)
	{
		// if(newScrollState.y>0){
		// 	Debug.Log("Scroll Up");
		// }
		// if(newScrollState.y<0){
		// 	Debug.Log("Scroll Down");
		// }
		scroll = newScrollState;
		
	}

	public void SlideInput(bool newSlideState)
	{
		slide = newSlideState;
	}

	public void PauseInput(bool newPauseState)
	{
		if(newPauseState){
			if(menuInstance == null){
				paused = true;
				menuInstance = Instantiate(menuObject);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				cursorInputForLook = false;
			}
			else{
				paused = false;
				Destroy(menuInstance);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				cursorInputForLook = true;
			}
		}
	}


	
}

