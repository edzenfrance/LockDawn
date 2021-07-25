using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class GraphicSettings : MonoBehaviour
{
    public RenderPipelineAsset[] qualityLevels;
    public TMP_Dropdown qualityDropdown;

    // Start is called before the first frame update
    void Start()
    {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
    }

    public void ChangeQualityLevel(int value)
    {
        QualitySettings.SetQualityLevel(value);
        QualitySettings.renderPipeline = qualityLevels[value];
    }
}
