using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPlayerScript : MonoBehaviour
{
    [SerializeField] private PlayerScript player;
    [SerializeField] private Image progresBarStamina;
    [SerializeField] private Image progresBarStaminaToRecover;


    public void ChangeStamina()
    {
        progresBarStamina.fillAmount = player.GetCurrentStamina() / player.GetMaxStamina();
        progresBarStaminaToRecover.fillAmount = (player.GetCurrentStamina() + player.GetCurrentRecoverStamina()) / player.GetMaxStamina();
    }

}
