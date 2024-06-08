using UnityEngine;
using UnityEngine.UI;

public class StageSelectSignalButtonSystem : MonoBehaviour
{
    [SerializeField] private Stage_Information stage_Information;
    public StageStartButtonSystem stageStartButton;
    public Image stageSpritePanel;
    public Sprite stageSprite;
    public Text fixationRewardText;
    public Text stageDescriptionText;
    public int stageNumber;
    public void OnButtonClick()
    {
        stageStartButton.stageNumber = stageNumber;
        stageSpritePanel.sprite = stage_Information.data[stageNumber].stageSprite;
        fixationRewardText.text = "ïÒèVÅF"+stage_Information.data[stageNumber].fixationReward.ToString()+"MONEY";
        stageDescriptionText.text = stage_Information.data[stageNumber].stageDescription;
    }
}
