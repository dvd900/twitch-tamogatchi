using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroneTester : MonoBehaviour
{
    public Rigidbody CapRB;
    public Rigidbody BottleRB;
    public ParticleSystem WaterStream, WaterTop, WaterSplash;

    public float thrust;
    // Start is called before the first frame update
    void Start()
    {
        WaterStream.enableEmission = false;
        WaterTop.enableEmission = false;
        WaterSplash.enableEmission = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UnCap()
    {
        Material liquid = BottleRB.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;

        CapRB.isKinematic = false;
        CapRB.gameObject.transform.parent = null;
        CapRB.AddForce(Random.RandomRange(-10,10), thrust, Random.RandomRange(-10, 10), ForceMode.Impulse);
        CapRB.AddTorque(100, 100, 100, ForceMode.Impulse);
        WaterStream.enableEmission = true;
        WaterTop.enableEmission = true;
        WaterSplash.Clear();
        WaterSplash.Play();
        WaterSplash.enableEmission = true;

        LeanTween.value(gameObject, 2.5f, -3f, 1.5f).setOnUpdate((float val) => {
            liquid.SetFloat("_Height", val);
        });
       
        

    }
    public void DropBottle()
    {
        BottleRB.isKinematic = false;
        BottleRB.gameObject.transform.parent = null;
        BottleRB.AddForce(Random.RandomRange(-10, 10), thrust/2, Random.RandomRange(-10, 10), ForceMode.Impulse);
        //CapRB.AddForce(Random.RandomRange(-10, 10), thrust, Random.RandomRange(-10, 10), ForceMode.Impulse);
        WaterStream.enableEmission = false;
        WaterTop.enableEmission = false;
        WaterSplash.enableEmission = false;
    }
}
