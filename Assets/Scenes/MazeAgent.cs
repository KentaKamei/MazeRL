using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MazeAgent : Agent
{
    public Transform goalTransform;
    private Rigidbody rb;

    public float moveSpeed = 3f;
    float prevDistance;
    float currentDistance;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // エージェントとゴールの位置をリセット（必要に応じて調整）
        transform.localPosition = new Vector3(-15, 1.0f, -15);
        rb.velocity = Vector3.zero;

        prevDistance = Vector3.Distance(transform.localPosition, goalTransform.localPosition);

        // ゴールも位置を固定 or ランダムで変更可
        //goalTransform.localPosition = new Vector3(14, 0.5f, 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 relativePosition = goalTransform.localPosition - transform.localPosition;
        sensor.AddObservation(relativePosition.x);
        sensor.AddObservation(relativePosition.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int action = actions.DiscreteActions[0];
        Vector3 moveDir = Vector3.zero;

        switch (action)
        {
            case 0: moveDir = Vector3.forward; break;   // ↑
            case 1: moveDir = Vector3.back;    break;   // ↓
            case 2: moveDir = Vector3.left;    break;   // ←
            case 3: moveDir = Vector3.right;   break;   // →
        }

        rb.AddForce(moveDir * moveSpeed, ForceMode.VelocityChange);
        AddReward(-0.001f); // 時間かけすぎ防止

        currentDistance = Vector3.Distance(transform.localPosition, goalTransform.localPosition);
        float reward = prevDistance - currentDistance;
        AddReward(reward * 0.1f);
        prevDistance = currentDistance;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            AddReward(1.0f);
            EndEpisode();
        }
        else if (other.CompareTag("Wall"))
        {
            AddReward(-0.2f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W)) discreteActions[0] = 0;
        else if (Input.GetKey(KeyCode.S)) discreteActions[0] = 1;
        else if (Input.GetKey(KeyCode.A)) discreteActions[0] = 2;
        else if (Input.GetKey(KeyCode.D)) discreteActions[0] = 3;
    }
}
