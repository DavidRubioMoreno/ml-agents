using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Material _winMat;
    [SerializeField] private Material _loseMat;
    [SerializeField] private MeshRenderer _floorMeshRenderer;
    public float _speed;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-1f, +2f), 1f, Random.Range(-2f, +2f));
        _targetTransform.transform.localPosition = new Vector3(Random.Range(3f, 5f), 1f, Random.Range(-2f, +2f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Solo una observación de prueba
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(_targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * _speed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> constinousActions = actionsOut.ContinuousActions;
        constinousActions[0] = Input.GetAxisRaw("Horizontal");
        constinousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            _floorMeshRenderer.material = _winMat;
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            _floorMeshRenderer.material = _loseMat;
            EndEpisode();
        }

    }
}
