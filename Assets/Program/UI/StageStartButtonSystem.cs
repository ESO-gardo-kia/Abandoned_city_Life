using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStartButtonSystem : MonoBehaviour
{
    public int stageNumber;
    public void OnButtonClick()
    {
        FindObjectsOfType<GameManager>()[0].SceneTransitionProcess(stageNumber);
    }
}
