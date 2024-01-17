using System.Globalization;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FishPreyAgent : Agent
{
    private Rigidbody _rigidbody;
    private FishRaycast _fishRaycast;
    
    [SerializeField] private float swimSpeed = 10;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float boxSize = 3f;

    private float _currentHorizontalAxis;
    private float _currentVerticalAxis;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _fishRaycast = GetComponent<FishRaycast>();
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        
        transform.localPosition = Random.insideUnitSphere * 2.75f;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(_fishRaycast.GetDistances());
    }
    
    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        if (!gameObject.activeSelf) return;
        
        _currentHorizontalAxis = actions.ContinuousActions[0];
        _currentVerticalAxis = actions.ContinuousActions[1];
        _rigidbody.angularVelocity = new Vector3(_currentVerticalAxis, _currentHorizontalAxis, 0) * rotationSpeed;
        _rigidbody.AddForce(transform.forward * swimSpeed);
        AddReward(0.1f);
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);

        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("FishPredator"))
        {
            AddReward(-100.0f);
            gameObject.SetActive(false);
        }
    }
    
    private void FixedUpdate()
    {
        // Keep fish in tank, if it approaches left it gets pushed right and vice versa
        if (transform.localPosition.x > boxSize)
        {
            transform.localPosition = new Vector3(-boxSize, transform.localPosition.y, transform.localPosition.z);
        }
        else if (transform.localPosition.x < -boxSize)
        {
            transform.localPosition = new Vector3(boxSize, transform.localPosition.y, transform.localPosition.z);
        }
        
        // Keep fish in tank, if it approaches top it gets pushed down and vice versa
        if (transform.localPosition.y > boxSize)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -boxSize, transform.localPosition.z);
        }
        else if (transform.localPosition.y < -boxSize)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, boxSize, transform.localPosition.z);
        }
        
        // Keep fish in tank, if it approaches front it gets pushed back and vice versa
        if (transform.localPosition.z > boxSize)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -boxSize);
        }
        else if (transform.localPosition.z < -boxSize)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, boxSize);
        }
    }
}
