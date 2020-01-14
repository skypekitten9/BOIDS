using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float sight;
    float privateRange;
    Rigidbody rb;
    Vector3 direction;
    GameObject flock;
    List<Transform> neighbours;
    List<Transform> breachers;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        privateRange = sight / 3;
    }

    private void FixedUpdate()
    {
        rb.AddForce(direction * speed);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Update()
    {
        flock = GameObject.Find("Flock");
        neighbours = GetNeighbours();
        if (neighbours.Count > 0)
        {
            GetNeighboursDirection();
            AddCohesion();
            if (HasPrivacyBreached(neighbours)) AddSeperation();
        }
        else direction = GameObject.Find("Rendezvous").transform.position - transform.position;
    }

    void AddCohesion()
    {
        Vector3 position = new Vector3(0, 0, 0);
        foreach (Transform neighbour in neighbours)
        {
            position = position + neighbour.position;
        }
        position = position / neighbours.Count;

        Vector3 cohesionVector = position - transform.position;
        direction = direction + cohesionVector;
        direction = direction.normalized;
    }

    void AddSeperation()
    {
        Vector3 position = new Vector3(0, 0, 0);
        foreach (Transform breacher in breachers)
        {
            position = position + breacher.position;
        }
        position = position / breachers.Count;
        Vector3 seperationVector = position - transform.position;
        seperationVector = seperationVector * (-1);
        direction = direction + seperationVector;
        direction = direction.normalized;
    }

    void GetNeighboursDirection()
    {
        direction = neighbours[0].GetComponent<AgentScript>().GetDirection();
    }

    Vector3 GetDirection()
    {
        return direction;
    }

    List<Transform> GetNeighbours()
    {
        List<Transform> neighboursToReturn = new List<Transform>();
        foreach (Transform child in flock.transform)
        {
            float distance = Vector3.Distance(child.transform.position, transform.position);
            if (distance < sight && child.transform.position != transform.position)
            {
                neighboursToReturn.Add(child.transform);
            }
        }
        return neighboursToReturn;
    }

    bool HasPrivacyBreached(List<Transform> neighboursToCheck)
    {
        bool result = false;
        breachers = new List<Transform>();
        foreach (Transform potentialBreacher in neighboursToCheck)
        {
            float distance = Vector3.Distance(potentialBreacher.position, transform.position);
            if (distance < privateRange)
            {
                result = true;
                breachers.Add(potentialBreacher);
            }
        }
        return result;
    }
}
