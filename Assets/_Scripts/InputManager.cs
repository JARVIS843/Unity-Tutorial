using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace UnityTutorial.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;

        public Vector2 move {get; private set;}
        public Vector2 look {get; private set;}
        public bool run {get; private set;}



        private InputActionMap currentMap;

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction runAction;



       private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            AssignReferences();
            SubscribeKeys();
        }

        private void SubscribeKeys()
        {
            moveAction.performed += onMove;
            lookAction.performed += onLook;
            runAction.performed += onRun;


            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            runAction.canceled += onRun;
        }

        private void AssignReferences()
       {
           currentMap = PlayerInput.currentActionMap;
           moveAction = currentMap.FindAction("Move");
           lookAction = currentMap.FindAction("Look");
           runAction = currentMap.FindAction("Run");
       }

       private void onMove(InputAction.CallbackContext context)
       {
           move = context.ReadValue<Vector2>();
       }

        private void onLook(InputAction.CallbackContext context)
        {
           look = context.ReadValue<Vector2>();
        }

        private void onRun(InputAction.CallbackContext context)
        {
            run = context.ReadValueAsButton();
        }




        private void OnEnable() {
            currentMap.Enable();
        }

        private void onDisable()
        {
            currentMap.Disable();
        }
        
    }
}
