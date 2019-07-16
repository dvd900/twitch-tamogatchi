using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyCharacter : MonoBehaviour
{


    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float GroundDistance = 0.2f;
    public float DashDistance = 5f;
    public LayerMask Ground;
    Animator animController;

    private Skin skin;
    private Rigidbody _body;
    private Vector3 _inputs = Vector3.zero;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _groundChecker = transform.GetChild(0);
        animController = gameObject.GetComponent<Animator>();
        skin = GetComponent<Skin>();
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, 
            Ground, QueryTriggerInteraction.Ignore);


        _inputs = Vector3.zero;
        _inputs.x = Input.GetAxis("Horizontal");
        _inputs.z = Input.GetAxis("Vertical");
        if (_inputs != Vector3.zero)
        {
            transform.forward = _inputs;
            animController.SetBool("isIdle", false);
        }
        else
        {
            animController.SetBool("isIdle", true);
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _body.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        if(Input.GetButtonDown("Interact")) 
        {
            if(skin.pickupController.heldItem != null) 
            {
                skin.pickupController.heldItem.Eat();
            } 
            else 
            {
                Item item = skin.pickupController.FindPickupCandidate();
                if(item != null) 
                {
                    skin.pickupController.Pickup(item); 
                }
            }
        }
    }


    void FixedUpdate()
    {
        _body.MovePosition(_body.position + _inputs * Speed * Time.fixedDeltaTime);
    }
}
