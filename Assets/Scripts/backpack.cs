using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class backpack : MonoBehaviour
{
    public UnityEvent enter;
    public UnityEvent exit;
    GameController gameController;
    public GameObject ui;

    public int torusCounter=0;
    public int sphereCounter=0;
    public int boxCounter=0;

    public GameObject spherePrefab;
    public GameObject boxPrefab;
    public GameObject torusPrefab;

    private GameObject currentSphere;
    private GameObject currentBox;
    private GameObject currentTorus;

    public GameObject lastObject;

    // Start is called before the first frame update
    void Start()
    {
        //setting up events
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        enter.AddListener(gameController.addToInv);
        exit.AddListener(gameController.removeFromInv);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get the Rigidbody component of the object that entered the trigger and disable it
        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        other.gameObject.tag = "Untagged";

        //snap the object to a specific position inside the backpack
        movableObject movob = other.GetComponent<movableObject>();
        if (movob.type == "torus")
        {
            other.transform.position = new Vector3(-2.14f, 2.05f, -6.47f);
            other.transform.rotation = Quaternion.Euler(-16, -60, 103);
            torusCounter++;
            currentTorus = other.gameObject;
            lastObject = other.gameObject;
        }
        if (movob.type == "sphere")
        {
            other.transform.position = new Vector3(-2.1f, 1.95f, -6.79f);
            other.transform.rotation = Quaternion.Euler(-90, 0, 0);
            sphereCounter++;
            currentSphere = other.gameObject;
            lastObject = other.gameObject;
        }
        if (movob.type == "box")
        {
            other.transform.position = new Vector3(-2.03f, 2.04f, -6.27f);
            other.transform.rotation = Quaternion.Euler(-90, 0, 0);
            boxCounter++;
            currentBox = other.gameObject;
            lastObject = other.gameObject;
        }
        enter.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        movableObject movob = other.GetComponent<movableObject>();
        if (movob.type == "torus")
        {
            torusCounter=torusCounter-1;
        }
        if (movob.type == "sphere")
        {
            sphereCounter=sphereCounter-1;
        }
        if (movob.type == "box")
        {
            boxCounter=boxCounter-1;
        }

        exit.Invoke();

        // Get the Rigidbody component of the object that entered the trigger and enable it
        //Rigidbody rb = other.GetComponent<Rigidbody>();
        //rb.isKinematic = false;
    }

    void OnMouseDown()
    {
        // This method is called when the object is clicked
        ui.SetActive(true);
    }
    void OnMouseUp()
    {
        // This method is called when the object is unclicked

        // Raycast to find the UI elements under the cursor
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // Set the position to the current mouse position
        };

        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            // Get the first result (the topmost UI element)
            GameObject clickedObject = results[0].gameObject;

            // Check if the clicked object is an Image
            if (clickedObject.GetComponent<UnityEngine.UI.Image>() != null)
            {
                //instantinating selected prefab
                //Debug.Log("Mouse released over: " + clickedObject.name);
                //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
                Vector3 mouseWorldPosition = new Vector3(0,1,-7);
                if (clickedObject.name == "torus" && torusCounter>0) {
                    Destroy(currentTorus);
                    currentTorus = Instantiate(torusPrefab, mouseWorldPosition, Quaternion.identity);
                    torusCounter--;
                    lastObject = currentTorus;
                }
                if (clickedObject.name == "box" && boxCounter>0)
                {
                    Destroy(currentBox);
                    currentBox = Instantiate(boxPrefab, mouseWorldPosition, Quaternion.identity); 
                    boxCounter--;
                    lastObject = currentBox;
                }
                if (clickedObject.name == "sphere" && sphereCounter > 0)
                {
                    Destroy(currentSphere);
                    currentSphere = Instantiate(spherePrefab, mouseWorldPosition, Quaternion.identity); 
                    sphereCounter--;
                    lastObject = currentSphere;
                }
                exit.Invoke();
            }
        }


        ui.SetActive(false);
    }
}
