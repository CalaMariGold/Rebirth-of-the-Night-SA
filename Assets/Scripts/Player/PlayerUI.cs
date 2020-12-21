using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rebirth.Player {
    public class PlayerUI : MonoBehaviour
    {
        private GameObject _ui;
        private GameObject _inventory;
        private MouseLook _mouseLook;
        // Start is called before the first frame update
        private bool _inventoryOpened;

        void Start()
        {
            _mouseLook = transform.Find("Main Camera").GetComponent<MouseLook>();

            _ui = transform.Find("UI").gameObject;
            _inventory = _ui.transform.Find("Inventory").gameObject;
            _inventoryOpened = false;
        }

        public void InventoryToggle(ref PlayerControls playerControls) {
            _inventoryOpened = !_inventoryOpened;
            if(_inventoryOpened) {
                _inventory.GetComponent<RectTransform>().localScale = new Vector3(2,2);
                playerControls.Movement.Disable();
                Cursor.lockState = CursorLockMode.None;
                _mouseLook.Disable();
            } else {
                _inventory.GetComponent<RectTransform>().localScale = new Vector3(0,0);
                playerControls.Movement.Enable();
                Cursor.lockState = CursorLockMode.Locked;
                _mouseLook.Enable();
            }
        }
    }
}