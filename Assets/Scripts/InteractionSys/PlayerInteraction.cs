using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction: MonoBehaviour {

    public GameObject player;
    public float interactionDistance;
    public GameObject crosshair;
    public Image image;
    private bool transparent = true;
    public GameObject EscapeMenu;
    private bool isEscOpen = false;
    public TMPro.TextMeshProUGUI interactionText;
    public GameObject interactionHoldGO; // the ui parent to disable when not interacting
    public UnityEngine.UI.Image interactionHoldProgress; // the progress bar for hold interaction type

    Camera cam;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        //crosshair = GameObject.Find("Crosshair");
        //image =  crosshair.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isEscOpen = !isEscOpen;
            if (isEscOpen) Cursor.lockState = CursorLockMode.None;
            else Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = isEscOpen;
            //EscapeMenu.SetActive(isEscOpen);
            player.GetComponent<FirstPersonAIO>().ChangeSense();

        }
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        //Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            Debug.Log(interactable);
            if (interactable != null) {
                HandleInteraction(interactable);
                ////interactionText.text = interactable.GetDescription();
                //successfulHit = true;
                //if (transparent)
                //{ 
                //    //image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                //    transparent = !transparent;
                //    //Debug.Log("Change");
                //}
                //interactionHoldGO.SetActive(true);
            }
        }

        // if we miss, hide the UI
        if (!successfulHit) {
            //interactionText.text = "";
            //if (!transparent)
            //{
            //    image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            //    transparent = !transparent;
            //    //Debug.Log("change");
            //}
            //interactionHoldGO.SetActive(false);
        }
    }

    void HandleInteraction(Interactable interactable) {
        #if UNITY_STANDALONE
        KeyCode key = KeyCode.E;

        switch (interactable.interactionType) {
            case Interactable.InteractionType.Click:
                // interaction type is click and we clicked the button -> interact
                if (Input.GetKeyDown(key)) {
                    Debug.Log("use");
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(key)) {
                    // we are holding the key, increase the timer until we reach 1f
                    interactable.IncreaseHoldTime();
                    if (interactable.GetHoldTime() > 1f) {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }
                } else {
                    interactable.ResetHoldTime();
                }
                interactionHoldProgress.fillAmount = interactable.GetHoldTime();
                break;
                // here is started code for your custom interaction :)
            case Interactable.InteractionType.Minigame:
                // here you make ur minigame appear
                break;

            case Interactable.InteractionType.Read:
                interactable.Interact();
                break;
                // helpful error for us in the future
            default:
                throw new System.Exception("Unsupported type of interactable.");



            #elif UNITY_ANDROID
            
            switch (interactable.interactionType) {
                case Interactable.InteractionType.Click:
                    // interaction type is click and we clicked the button -> interact
                    if (CrossPlatformInputManager.GetButtonDown("Interact")) {
                        interactable.Interact();
                    }
                    break;
                case Interactable.InteractionType.Hold:
                    if (CrossPlatformInputManager.GetButton("Interact")) {
                        // we are holding the key, increase the timer until we reach 1f
                        interactable.IncreaseHoldTime();
                        if (interactable.GetHoldTime() > 1f) {
                            interactable.Interact();
                            interactable.ResetHoldTime();
                        }
                    } else {
                        interactable.ResetHoldTime();
                    }
                    interactionHoldProgress.fillAmount = interactable.GetHoldTime();
                    break;
                    // here is started code for your custom interaction :)
                case Interactable.InteractionType.Minigame:
                    // here you make ur minigame appear
                    break;

                case Interactable.InteractionType.Read:
                    interactable.Interact();
                    break;
                    // helpful error for us in the future
                default:
                    throw new System.Exception("Unsupported type of interactable.");
            #endif
        }
    }
}