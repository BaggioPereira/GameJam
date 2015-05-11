using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public Text timer;
    float seconds = 30;
    AudioSource heart;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.AddComponent<CharacterController>();
        controller = (CharacterController)player.transform.GetComponent("CharacterController");
        overlay = GameObject.Find("Overlay");
        heart = GetComponent<AudioSource>();
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

        seconds -= Time.deltaTime;
        int temp = (int)seconds;
        timer.text = temp.ToString() + " Seconds";

        if(seconds <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
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
                    {
                        c.a += 0.05f;
                        if(heart.pitch < 1.5)
                            heart.pitch += 0.05f;

                        Debug.Log(heart.pitch.ToString());
                    }
                        

                    overlay.GetComponent<Renderer>().material.color = c;
                }
            }
        }

        if(tempDistance > distance)
        {
            c = overlay.GetComponent<Renderer>().material.color;
               c.a -= 0.05f;
            if(heart.pitch > 1)
            {
                heart.pitch -= 0.05f;
            }
               
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
