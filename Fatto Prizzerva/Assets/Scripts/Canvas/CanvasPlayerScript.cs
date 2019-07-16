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
        progresBarStamina.fillAmount = player.stamina.Stamina / player.stamina.MaxStamina;
        progresBarStaminaToRecover.fillAmount = (player.stamina.Stamina + player.stamina.CurrentRegenStamina) / player.stamina.MaxStamina;
    }

}
