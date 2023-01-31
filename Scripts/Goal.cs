using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = other.transform;
        if (otherTransform.CompareTag("Ball"))
        {
            points++;
            textMesh.text = "<mark>Points: " + points + "</mark>";
            otherTransform.position = new Vector3(0, 5, 0);
            Rigidbody otherRB = otherTransform.GetComponent<Rigidbody>();
            otherRB.velocity = otherRB.angularVelocity = Vector3.zero;
            Debug.Log("Added a point");
        }
    }
}
