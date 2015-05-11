using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    GameObject player, overlay;
    CharacterController controller;
    Color c;
    public float sensitivityX = 10.0f;
    public float sensitivityY = 10.0f;
    public float yRange = 60.0f;
    float rotationY = 0.0f;
    float verticalVelocity = 0.0f;
    public float jump = 2.0f;
    public float speed = 10.0f;
    private Vector3 moveDirection = Vector3.zero;
    public float distance;
    int levelcounter = 1;

    float transparencyCap = 0.60f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.AddComponent<CharacterController>();
        controller = (CharacterController)player.transform.GetComponent("CharacterController");
        overlay = GameObject.Find("Overlay");
        
    }

    // Update is called once per frame
    void Update()
    {
        //Rotation
        float rotationX = Input.GetAxis("Mouse X") * sensitivityX;
        transform.Rotate(0, rotationX, 0);

        rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, -yRange, yRange);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);

        //Movement
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), verticalVelocity, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;
        controller.Move(moveDirection * Time.deltaTime);

        if (player.transform.position.y < 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        checkWinning();
    }

    void FixedUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distance);
        GameObject winningObject;
        float tempDistance = 0;

        //c = overlay.GetComponent<Renderer>().material.color;
        for(int i = 0; i < hitColliders.Length; i++)
        {   
            
            if(hitColliders[i].name == "Winning Object " + levelcounter.ToString())
            {
                winningObject = hitColliders[i].gameObject;
                c = overlay.GetComponent<Renderer>().material.color;
                Debug.Log("Object is near");
                Vector3 direction = hitColliders[i].transform.position - transform.position;
                float mag = direction.magnitude;
                tempDistance = mag;

                if(mag<distance)
                {

                    if(c.a < transparencyCap)
                    //{
                        c.a += 0.05f;
                    //}                      

                    overlay.GetComponent<Renderer>().material.color = c;
                }
            }

            //else if(hitColliders[i].tag )
            //{
            //    //c = overlay.GetComponent<Renderer>().material.color;
            //    c.a -= 0.01f;
            //    Debug.Log("Running else");
            //    overlay.GetComponent<Renderer>().material.color = c;
            //}
        }

        if(tempDistance > distance)
        {
            c = overlay.GetComponent<Renderer>().material.color;
               c.a -= 0.05f;
                Debug.Log("Running else");
               overlay.GetComponent<Renderer>().material.color = c;

        }
        else
        {
            tempDistance = 0;
        }
    }

    void checkWinning()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, distance))
            {
                if(hit.collider.gameObject.name == "Winning Object " + levelcounter.ToString())
                {
                    Debug.Log("Hit object");
                    Destroy (GameObject.Find("Door " + levelcounter.ToString()));
                }
            }
        }
    }
}
