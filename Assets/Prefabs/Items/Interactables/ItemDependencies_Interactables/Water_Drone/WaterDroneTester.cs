using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroneTester : MonoBehaviour
{
    public Rigidbody CapRB;
    public Rigidbody BottleRB;
    public ParticleSystem WaterStream;

    public float thrust;
    // Start is called before the first frame update
    void Start()
    {
        WaterStream.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UnCap()
    {
        CapRB.isKinematic = false;
        CapRB.gameObject.transform.parent = null;
        CapRB.AddForce(Random.RandomRange(-10,10), thrust, Random.RandomRange(-10, 10), ForceMode.Impulse);
        CapRB.AddTorque(100, 100, 100, ForceMode.Impulse);
        WaterStream.enableEmission = true;

    }
    public void DropBottle()
    {
        BottleRB.isKinematic = false;
        BottleRB.gameObject.transform.parent = null;
        //CapRB.AddForce(Random.RandomRange(-10, 10), thrust, Random.RandomRange(-10, 10), ForceMode.Impulse);
        WaterStream.enableEmission = false;
    }
}
