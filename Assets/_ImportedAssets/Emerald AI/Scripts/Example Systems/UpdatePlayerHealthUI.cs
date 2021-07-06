using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This example is no longer used and will be removed with a future update.
namespace EmeraldAI.Example
{
    public class UpdatePlayerHealthUI : MonoBehaviour
    {
        EmeraldAIPlayerHealth PlayerHealthComponent;
        Slider PlayerHealthSlider;

        void Start()
        {
            if (GetComponent<EmeraldAIPlayerHealth>())
            {
                PlayerHealthComponent = GetComponent<EmeraldAIPlayerHealth>();
            }

            if (GameObject.Find("HealthBar") != null)
            {
                PlayerHealthSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
            }           
        }

        public void UpdateHealthUI ()
        {
            if (PlayerHealthSlider != null && PlayerHealthComponent != null)
            {
                PlayerHealthSlider.value = (float)PlayerHealthComponent.CurrentHealth / 100;
            }
        }
    }
}