using UnityEngine;
using System.Collections;

public class Test_eat : MonoBehaviour
{
    public Material[] eyeMat;
    public float eyeMaxOffset = 0.3f; // max amount the eyes are clamped to
   //public Renderer eyeRend, eyeRend2; // eyeball renderer
    public Renderer lidRend; // eyelid renderer
    public float blinkingTextureAmount = 4f; // amount of frames of blinking animation
    public float blinkTimer = 1f; // timer for when to blink again
    public float blinkTransition = 3.5f;
    Vector2 eyeOffset;
    Vector2 eyelidOffset;
    float blinkOffset;
    float blinkTimerR;
    bool isEmoting = false;

    void Start()
    {
        //eyeRend.materials[3] = eyeMat[0];
        //eyeRend2.materials[3] = eyeMat[1];
        Material[] matArray = lidRend.materials;
        matArray[2] = eyeMat[0];
        lidRend.materials = matArray;
        blinkTimerR = blinkTimer;
        blinkOffset = 1 / blinkingTextureAmount;
        eyelidOffset = new Vector2(0, 0);
       // lidRend.materials[2].SetTextureScale("_MainTex", new Vector2(blinkOffset, 1));


    }
    void Update()
    {
        if (isEmoting == false)
        {
            blinkTimerR -= Time.deltaTime;
            if (blinkTimerR <= 0.0f)
            {
                 StartCoroutine(Blink());
                isEmoting = true;
            }

        }

    }
    public void EmoteNormal()
    {

    }

    IEnumerator Blink()
    {
        // animating 1 - 2 - 3 - 4 - 3 - 1, if you have more or less blinking animation frames, add or delete them here
        blinkTimerR = 0; // slight randomisation to the blinking
        lidRend.materials[2].SetTextureOffset("_DetailAlbedoMap", eyelidOffset);
        yield return new WaitForSeconds(blinkTransition);
        lidRend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(eyelidOffset.x + (blinkOffset * 2), 0)); //3
        yield return new WaitForSeconds(blinkTransition);
        lidRend.materials[2].SetTextureOffset("_DetailAlbedoMap", new Vector2(eyelidOffset.x + (blinkOffset * 2), 0)); //3
        yield return new WaitForSeconds(blinkTransition);
        lidRend.materials[2].SetTextureOffset("_DetailAlbedoMap", eyelidOffset); //1
        isEmoting = false;
    }

}
