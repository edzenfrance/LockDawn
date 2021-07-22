using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// http://answers.unity.com/answers/1707173/view.html

[RequireComponent(typeof(ToggleGroup))]
public class EventToggleGroup : MonoBehaviour
{
    [System.Serializable]
    public class ToggleEvent : UnityEvent<Toggle> { }

    [SerializeField]
    public ToggleEvent onActiveTogglesChanged;

    [SerializeField]
    private Toggle[] _toggles;

    private ToggleGroup _toggleGroup;

    private void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (Toggle toggle in _toggles)
        {
            if (toggle.group != null && toggle.group != _toggleGroup)
            {
                Debug.LogError($"EventToggleGroup is trying to register a Toggle that is a member of another group.");
            }
            toggle.group = _toggleGroup;
            toggle.onValueChanged.AddListener(HandleToggleValueChanged);
        }
    }

    void HandleToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            onActiveTogglesChanged?.Invoke(_toggleGroup.ActiveToggles().FirstOrDefault());
        }
    }

    void OnDisable()
    {
        foreach (Toggle toggle in _toggleGroup.ActiveToggles())
        {
            toggle.onValueChanged.RemoveListener(HandleToggleValueChanged);
            toggle.group = null;
        }
    }
}