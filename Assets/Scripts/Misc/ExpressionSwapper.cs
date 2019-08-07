using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionSwapper : MonoBehaviour
{
    public Renderer rend;
    public Material[] eyeMat, faceMat;

    Vector2 defaultOffset; //offset of spritesheet
    float frameOffset; //next frame in spritesheet
    public float frameAmount; //amount of frames in the spritesheet

    float frameTime;
    bool animate = false;

    void Start()
    {
        defaultOffset = new Vector2(0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EyesDefault() //Default eye state
    {
        Material[] matArray = rend.materials;
        matArray[0] = eyeMat[0]; //Left eye
        matArray[1] = eyeMat[1]; //Right eye
        rend.materials = matArray;
    }
    public void EyesClosed() //Eating
    {
        Material[] matArray = rend.materials;
        matArray[0] = eyeMat[2]; //Left eye
        matArray[1] = eyeMat[3]; //Right eye
        rend.materials = matArray;
    }
    public void MouthDefault()
    {
        Material[] matArray = rend.materials;
        matArray[2] = faceMat[0];
        rend.materials = matArray;
        StopCoroutine(AnimateFace(frameAmount, frameTime));
        rend.materials[2].SetTextureOffset("_DetailAlbedoMap", defaultOffset);
        animate = false;
    }
    public void MouthEating() //2 frame spritesheet
    {
        animate = true;
        Material[] matArray = rend.materials;
        matArray[2] = faceMat[1];
        rend.materials = matArray;
        frameAmount = 2;
        frameTime = 0.1f;
        StartCoroutine(AnimateFace(frameAmount, frameTime));
    }
    IEnumerator AnimateFace(float frames, float fTime) //animates by changing texture offset in face material
    {
        frameOffset = 1 / frames;
        // animating 1 - 2 - 3 - 4 - 3 - 1, if you have more or less blinking animation frames, add or delete them here
        rend.materials[2].SetTextureOffset("_DetailAlbedoMap", defaultOffset);
        yield return new WaitForSeconds(fTime);
        rend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(defaultOffset.x + frameOffset, 0)); //2
        yield return new WaitForSeconds(fTime);
        if (frameAmount > 2)
        {
            rend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(defaultOffset.x + (frameOffset * 2), 0)); //3
            yield return new WaitForSeconds(fTime);
            if(frameAmount >3)
            {
                rend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(defaultOffset.x + (frameOffset * 3), 0)); //3
                yield return new WaitForSeconds(fTime);
                rend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(defaultOffset.x + (frameOffset * 3), 0)); //3
                yield return new WaitForSeconds(fTime);
            }
            rend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(defaultOffset.x + (frameOffset * 2), 0)); //3
            yield return new WaitForSeconds(fTime);
        }
        rend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(defaultOffset.x + frameOffset, 0)); //2
        yield return new WaitForSeconds(fTime);
        rend.materials[2].SetTextureOffset("_DetailAlbedoMap", defaultOffset); //1

        if(animate == true)
            StartCoroutine(AnimateFace(frameAmount, frameTime));
    }
}
