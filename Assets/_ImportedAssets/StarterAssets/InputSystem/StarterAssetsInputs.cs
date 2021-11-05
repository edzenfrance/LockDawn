using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		public StaminaController staminaController;
		public GameObject touchZoneSprint;
		public Button touchZoneSprintButton;
		string sceneName;
		
		private void Start()
		{
			sceneName = SceneManager.GetActiveScene().name;
			if (sceneName != "CharacterSelection")
            {
				staminaController = GameObject.Find("UI_Canvas_StarterAssetsInputs_TouchZones/UI_Virtual_Button_Sprint").GetComponent<StaminaController>();
				touchZoneSprint = GameObject.Find("UI_Canvas_StarterAssetsInputs_TouchZones/UI_Virtual_Button_Sprint");
				touchZoneSprintButton = touchZoneSprint.GetComponent<Button>();
			}
		}

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
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			// Sprint character using Joystick (Disable this if using UI_Virtual_TouchZone_Move
			// UI_Canvas_StarterAssetsInputs_Joysticks >  UI_Virtual_Joystick_Move
			// sprint = newMoveDirection.sqrMagnitude > 0.75f;
			move = newMoveDirection;

			// Enable or disable sprint button based on character Vector2
			if (move.Equals(Vector2.zero))
			{
				touchZoneSprintButton.interactable = false;
				SprintInput(false);
			}
			else if (!staminaController.exhausted)
				touchZoneSprintButton.interactable = true;
		}

		public void getMoveInput()
		{
			if (move.Equals(Vector2.zero))
			{
				touchZoneSprintButton.interactable = false;
				SprintInput(false);
			}
			else if (!staminaController.exhausted)
				touchZoneSprintButton.interactable = true;
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

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}