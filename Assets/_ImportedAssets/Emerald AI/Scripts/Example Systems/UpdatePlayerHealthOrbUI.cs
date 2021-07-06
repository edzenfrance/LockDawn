using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmeraldAI.Example
{
    public class UpdatePlayerHealthOrbUI : MonoBehaviour
    {
        public static UpdatePlayerHealthOrbUI Instance;
        EmeraldAIPlayerHealth PlayerHealthComponent;
        Image PlayerHealthOrb;

        void Start()
        {
            Instance = this;

            if (GetComponent<EmeraldAIPlayerHealth>())
            {
                PlayerHealthComponent = GetComponent<EmeraldAIPlayerHealth>();
            }

            if (GameObject.Find("Health Orb") != null)
            {
                PlayerHealthOrb = GameObject.Find("Health Orb").GetComponent<Image>();
            }           
        }

        public void UpdateHealthUI ()
        {
            if (PlayerHealthOrb != null && PlayerHealthComponent != null)
            {
                PlayerHealthOrb.fillAmount = (float)PlayerHealthComponent.CurrentHealth / 100;
            }
        }
    }
}