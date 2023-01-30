using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlayerInput : MonoBehaviour
{
    [SerializeField] private ActivePlayerManager manager;

    public float speed;
    public float time;
    
    public Camera activeCam;
    public GameObject[] playerCameras = new GameObject[2];
    public int[] playerCamerasID = new int[]    {0, 1};

    public Rigidbody playerRB;

    public LayerMask whatIsGround;
    public bool isGrounded;
    public Transform groundPoint;

    private Vector2 moveInput;
    public float moveSpeed, jumpForce;

    public BulletScript bullet;
    public float timeBetweenShots;
    public float shotCounter;
    public Transform firePoint;

    [SerializeField] public SpriteRenderer playerSprite;

    void Update()
    {
        for (int i = 0; i < playerCamerasID.Length; i++)
        {
            playerCameras[i].SetActive(true);
        }

        if (manager.currentPlayer == manager.player1)
        {
            activeCam = GameObject.Find("PlayerCam1").GetComponent<Camera>();
            firePoint = GameObject.Find("FirePoint1").GetComponent<Transform>();
            groundPoint = GameObject.Find("GroundCheckPoint1").GetComponent<Transform>();
            playerRB = GameObject.Find("PlayerOne").GetComponent<Rigidbody>();
            playerSprite = GameObject.Find("PlayerSprite1").GetComponent<SpriteRenderer>();
        }

        if (manager.currentPlayer == manager.player2)
        {
            activeCam = GameObject.Find("PlayerCam2").GetComponent<Camera>();
            firePoint = GameObject.Find("FirePoint2").GetComponent<Transform>();
            groundPoint = GameObject.Find("GroundCheckPoint2").GetComponent<Transform>();
            playerRB = GameObject.Find("PlayerTwo").GetComponent<Rigidbody>();
            playerSprite = GameObject.Find("PlayerSprite2").GetComponent<SpriteRenderer>();
        }

        if (manager.PlayerCanPlay() && !manager.gameEnded)
        {
            HandleCamera();
            HandleMovement();
            HandleShoot();

            RaycastHit hit;
            
            if(Physics.Raycast(groundPoint.position, Vector3.down, out hit, 1f, whatIsGround))
            {
                isGrounded = true;
            } else
            {
                isGrounded = false;
            } 
        } 
    }

    private void HandleCamera()
    {
        if (manager.currentPlayer == manager.player1)
        {
            for(int i = 0; i < playerCamerasID.Length; i++)
            {
                if (playerCamerasID[i] != 0)
                {
                    playerCameras[i].SetActive(false);
                }

                if (playerCamerasID[i] == 0)
                {
                    playerCameras[i].SetActive(true);
                }
            }
        }

        else if (manager.currentPlayer == manager.player2)
        {
            for(int i = 0; i < playerCamerasID.Length; i++)
            {
                if (playerCamerasID[i] != 1)
                {
                    playerCameras[i].SetActive(false);
                }

                if (playerCamerasID[i] == 1)
                {
                    playerCameras[i].SetActive(true);
                }
            }
        }
    }

    private void HandleMovement()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        playerRB.velocity = new Vector3(moveInput.x * moveSpeed, playerRB.velocity.y, moveInput.y * moveSpeed);

        if (playerSprite.flipX && moveInput.x > 0)
        {
            playerSprite.flipX = false;
        }

        else if (!playerSprite.flipX && moveInput.x < 0)
        {
            playerSprite.flipX = true;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRB.velocity += new Vector3(0f, jumpForce, 0f);
        }
    }

    private void HandleShoot()
    {
        Vector3 targetPoint = new Vector3(0, 0, 0);
        Vector3 mousePos = Input.mousePosition;

        Ray ray = activeCam.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = new Vector3(hit.point.x, firePoint.transform.position.y, hit.point.z);
            
        }


        if (Input.GetMouseButtonDown(0))
        {
            shotCounter -= Time.deltaTime;

            firePoint.transform.LookAt(targetPoint);
            
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                BulletScript newBullet = Instantiate(bullet, firePoint.transform.position, Quaternion.identity) as BulletScript;
                newBullet.transform.LookAt(targetPoint);
            }
            else
            {
                shotCounter = 0;
            }
        }
    }
}
