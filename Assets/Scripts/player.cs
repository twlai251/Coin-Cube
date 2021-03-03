using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody playerBody;
    private Game game;

    private GameObject sword;

    private bool jump;
    [SerializeField]
    private TMPro.TextMeshProUGUI coinText;
    [SerializeField]
    private int coins;


    private Vector3 inputVector;
    
    // Start is called before the first frame update
    void Start()
    {
        sword = transform.GetChild(0).gameObject;
        game = FindObjectOfType<Game>();
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //print(inputVector.GetAxis("Horizontal"));
        //playerBody.velocity = new Vector3(2f, playerBody.velocity.y, 2f);

        inputVector = new Vector3(Input.GetAxis("Horizontal") * 10f, playerBody.velocity.y, Input.GetAxis("Vertical") * 10f);
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Attack")){
            PerformAttack();
        }


    }

    private void FixedUpdate()
    {
        playerBody.velocity = inputVector;

        if (jump && IsGrounded())
        {
            playerBody.AddForce(Vector3.up * 20f, ForceMode.Impulse);
            jump = false;
        }
    }


    private void PerformAttack(){
        if (!sword.activeSelf){
            sword.SetActive(true);
        }
    }


    bool IsGrounded()
    {
        float distance = GetComponent<Collider>().bounds.extents.y + 0.01f;
        Ray ray = new Ray(transform.position, Vector3.down);

        return Physics.Raycast(ray, distance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            game.ReloadCurrentLevel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "coin":
                //increment coin value
                coins++;
                Destroy(other.gameObject);
                //update UI
                coinText.text = string.Format("Coins : {0}", coins);
                break;
            case "goal":
                //check for completion
                other.GetComponent<Goal>().CheckForCompletion(coins);
                break;

            default:
                break;
        }
    }



}
