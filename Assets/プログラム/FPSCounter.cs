using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
public class FPSCounter : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1.0f)]
    float EveryCalcurationTime = 0.5f;
    public float Fps{get; private set;}

    int frameCount;
    float prevTime;
    private Text FPStext;

    void Start()
    {
        FPStext = transform.Find("System_Canvas/FPStext").gameObject.GetComponent<Text>();
        frameCount = 0;
        prevTime = 0.0f;
        Fps = 0.0f;
    }
    void Update()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        // n•b‚²‚Æ‚ÉŒv‘ª
        if (time >= EveryCalcurationTime)
        {
            Fps = frameCount / time;

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
            FPStext.text = Fps.ToString();
        }
    }
}