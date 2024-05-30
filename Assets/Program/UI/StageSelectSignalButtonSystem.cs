using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectSignalButtonSystem : MonoBehaviour
{
    [SerializeField] private Stage_Information stage_Information;
    public StageStartButtonSystem stageStartButton;
    public Text FixationRewardText;
    public Text StageDescriptionText;
    public int stageNumber;
    public void OnButtonClick()
    {
        stageStartButton.stageNumber = stageNumber;
        FixationRewardText.text = stage_Information.data[stageNumber].fixationReward.ToString();
        StageDescriptionText.text = stage_Information.data[stageNumber].stageDescription;
    }
}
