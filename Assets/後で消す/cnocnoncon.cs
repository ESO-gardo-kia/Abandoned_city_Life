using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cnocnoncon : MonoBehaviour
{
    public GameObject Player;
    private Material GetMaterial;
    public Material GiveMaterial;

    public float objectSizeX = 0;
    public float radius = 0;
    public float materialChangeContinuation;
    private float materialChangeContinuationCount;
    private bool isColorChange;
    private bool isXPos;
    private bool isPassingNow;
    private void Update()
    {
        PlayerPositionJudge();
        PassingJudge();
        MaterialChenge();
    }

    private void PassingJudge()
    {
        if (isXPos)
        {
            if (Player.transform.position.x < transform.position.x + objectSizeX && Player.transform.position.x > transform.position.x + -objectSizeX &&
                Vector3.Distance(Player.transform.position, transform.position) < radius)
            {
                isPassingNow = true;
            }
            else
            {
                isPassingNow = false;
            }
        }
    }

    private void MaterialChenge()
    {
        if (isColorChange)
        {
            materialChangeContinuationCount += 1 * Time.deltaTime;
            if (materialChangeContinuationCount >= materialChangeContinuation)
            {
                materialChangeContinuationCount = 0;
                Player.GetComponent<MeshRenderer>().material = GetMaterial;
                isColorChange = false;
            }
        }
    }

    private void PlayerPositionJudge()
    {
        
        if (Player.transform.position.x > transform.position.x + objectSizeX)
        {
            isXPos = false;
            if (isPassingNow)
            {
                isPassingNow = false;
                Debug.Log("‚·‚è”²‚¯‚½");
                isColorChange = true;
                GetMaterial = Player.GetComponent<MeshRenderer>().material;
                Player.GetComponent<MeshRenderer>().material = GiveMaterial;
            }
        }
        else if (Player.transform.position.x < transform.position.x + -objectSizeX)
        {
            isXPos = true;
        }
    }
}
