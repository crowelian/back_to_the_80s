using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GraphicsQualityManager : MonoBehaviour
{

    
    public GameObject postProcessingStack;
    public PostProcessLayer ppl;
    public bool qualityHigh = true;

    private string hqText = "Quality Now HQ";
    private string medText = "Quality Now MEDIUM";
    private string lqText = "Quality Now LQ";
    public Text qualityBtnText;
    public Text DEBUG_QUALITY_WARNING_TEXT;
   

    // Start is called before the first frame update
    void Start()
    {
        if (qualityHigh) {
            ppl.enabled = true;
            postProcessingStack.SetActive(true);
            if (DEBUG_QUALITY_WARNING_TEXT != null)
            DEBUG_QUALITY_WARNING_TEXT.gameObject.SetActive(false);
        } else {
            ppl.enabled = false;
            postProcessingStack.SetActive(false);
            if (DEBUG_QUALITY_WARNING_TEXT != null)
            DEBUG_QUALITY_WARNING_TEXT.gameObject.SetActive(true);
        }

        
    }

    
    void ToggleGraphicsQuality() {
        qualityHigh = !qualityHigh;
        if (qualityHigh) {
            ppl.enabled = true;
            postProcessingStack.SetActive(true);
        } else {
            ppl.enabled = false;
            postProcessingStack.SetActive(false);
        }
    }


    public void SetQualityLevel(int i) {
        //5 high
        //6 custom low
        //7 custom medium
        QualitySettings.SetQualityLevel(i, true);

    }

    public void ToggleQuality() {
        qualityHigh = !qualityHigh;

        if (qualityHigh) {
            SetQualityLevel(5);
            postProcessingStack.SetActive(true);
            qualityBtnText.text = hqText;
            DEBUG_QUALITY_WARNING_TEXT.gameObject.SetActive(false);
        } else {
            SetQualityLevel(6);
            postProcessingStack.SetActive(false);
            qualityBtnText.text = lqText;
            DEBUG_QUALITY_WARNING_TEXT.gameObject.SetActive(true);
        }
    }

    public void QualityLow() {
        SetQualityLevel(6);
        postProcessingStack.SetActive(false);
        qualityBtnText.text = lqText;
        DEBUG_QUALITY_WARNING_TEXT.gameObject.SetActive(true);
    }
    public void QualityMed() {
        SetQualityLevel(7);
        postProcessingStack.SetActive(true);
        qualityBtnText.text = medText;
        DEBUG_QUALITY_WARNING_TEXT.gameObject.SetActive(false);
    }
    public void QualityHigh() {
        SetQualityLevel(5);
        postProcessingStack.SetActive(true);
        qualityBtnText.text = hqText;
        DEBUG_QUALITY_WARNING_TEXT.gameObject.SetActive(false);
    }
    


}
