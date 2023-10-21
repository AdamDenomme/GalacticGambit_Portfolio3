using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class topDownPlayerController : MonoBehaviour
{
    [Header("--- Components ---")]
    public Camera camera;
    

    [SerializeField] float cameraSensitivity;
    [SerializeField] int heightMaxLimitation;
    [SerializeField] int heightMinLimitation;
    [SerializeField] int widthMaxLimitation;
    [SerializeField] int widthMinLimitation;
    [SerializeField] int rotationSpeed;

    [SerializeField] float zoomSpeed;
    [SerializeField] float minFieldOfView;
    [SerializeField] float maxFieldOfView;

    public crewMember selectedCrewMember;
    int screenHeight;
    int screenWidth;
    // Start is called before the first frame update
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        zoom();
        crewMemberControl();
        rotate();
    }
    void movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {

            if (transform.position.x + verticalInput >= heightMaxLimitation)
            {
                verticalInput = -1f;
            }
            if (transform.position.x + verticalInput <= heightMinLimitation)
            {
                verticalInput = 1f;
            }
            if (transform.position.z + horizontalInput >= widthMaxLimitation)
            {
                horizontalInput = 1f;
            }
            if (transform.position.z + horizontalInput <= widthMinLimitation)
            {
                horizontalInput = -1f;
            }

            Vector3 cameraForward = camera.transform.forward;
            Vector3 cameraRight = camera.transform.right;

            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 moveDirection = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized;

            Vector3 moveTo = transform.position + moveDirection * cameraSensitivity * Time.deltaTime;

            transform.position = moveTo;
        }
    }

    void rotate()
    {
        RaycastHit center;
        Physics.Raycast(camera.ScreenPointToRay(new Vector3(screenWidth / 2, screenHeight / 2, 0)), out center, 1000);
        if (Input.GetButton("Roll+"))
        {
            float rotationAngle = rotationSpeed * Time.deltaTime;

            transform.RotateAround(center.point, Vector3.down, rotationAngle);
        }
        else if (Input.GetButton("Roll-"))
        {
            float rotationAngle = -rotationSpeed * Time.deltaTime;
            transform.RotateAround(center.point, Vector3.down, rotationAngle);
        }
    }

    void zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        camera.transform.position += new Vector3(0, zoomSpeed * -scroll, 0);

        camera.transform.position = new Vector3(transform.position.x, Mathf.Clamp(camera.transform.position.y, minFieldOfView, maxFieldOfView), transform.position.z);
    }

    void crewMemberControl()
    {
        if (selectedCrewMember == null && Input.GetButton("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                crewMember crew;
                if (hit.collider.TryGetComponent(out crew))
                {
                    selectedCrewMember = crew;
                    selectedCrewMember.toggleSelected(true);
                    crew.updateGameUI();
                }
            }
        }
        else if (selectedCrewMember != null && Input.GetButton("Fire1"))
        {
            selectedCrewMember.toggleSelected(false);
            selectedCrewMember.resetGameUI();
            selectedCrewMember = null;
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                crewMember crew;
                if (hit.collider.TryGetComponent(out crew))
                {
                    selectedCrewMember = crew;
                    selectedCrewMember.toggleSelected(true);
                    crew.updateGameUI();
                }
            }
        }
        else if(selectedCrewMember != null && Input.GetButtonDown("Cancel"))
        {
            if(selectedCrewMember.selectedInteraction != null)
            {
                selectedCrewMember.selectedInteraction.onInteractable(false);
            }

            selectedCrewMember.toggleSelected(false);
            selectedCrewMember.resetGameUI();
            selectedCrewMember = null;
        }
        else
        {
            if(selectedCrewMember != null && Input.GetButton("Fire2"))
            {
                RaycastHit hit;
                if(Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    NavMeshHit navMeshHit;
                    if (NavMesh.SamplePosition(hit.point, out navMeshHit, 100, NavMesh.AllAreas))
                    {
                        selectedCrewMember.moveTo(hit.point);
                    }
                    
                }
            }
        }
        if(selectedCrewMember != null && selectedCrewMember.health <= 0)
        {
            selectedCrewMember = null;
        }
    }
    

}
