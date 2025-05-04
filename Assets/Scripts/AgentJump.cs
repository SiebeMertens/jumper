using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;

public class AgentJump : Agent
{
    Rigidbody rb;
    public float jumpForce = 10f;
    public LayerMask groundMask;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = true; // Assume starting grounded, adjust if needed
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            isGrounded = true;
            Debug.Log("Collision Enter - Grounded: True");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            isGrounded = false;
            Debug.Log("Collision Exit - Grounded: False");
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int jumpAction = actions.DiscreteActions[0];
    
        if (jumpAction == 1 && isGrounded )
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump Action Received and Executed!");
        }
    }
}
