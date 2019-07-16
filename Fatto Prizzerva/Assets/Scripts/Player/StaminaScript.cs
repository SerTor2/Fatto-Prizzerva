using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaScript : MonoBehaviour, IEstaminable
{
    [SerializeField] private float currentStamina;
    [SerializeField] private float maxStamina = 100;

    [SerializeField] private float multiplyStaminaPerSpeed = 2f;
    private float currentTimeStamina = 0;
    private float recoverStamina = 0;

    [SerializeField] private CanvasPlayerScript canvasPlayer;

    public float Stamina { get => currentStamina; set => currentStamina = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public float CurrentRegenStamina { get => recoverStamina; set => recoverStamina = value; }

    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ModifiyStamina(float _stamina, bool _maxStamina = false)
    {
        if(_maxStamina)
        {

        }
        else
        {
            currentStamina += _stamina;
            if (_stamina < 0)
                recoverStamina += _stamina * -1;

            if (currentStamina < 0)
                currentStamina = 0;
            else if (currentStamina > maxStamina)
                currentStamina = maxStamina;

            if (_stamina < 0)
                currentTimeStamina = 0;

        }
        canvasPlayer.ChangeStamina();
    }

    public void RegenStamina()
    {
        currentTimeStamina += Time.deltaTime;
        float value = Time.deltaTime * multiplyStaminaPerSpeed * Mathf.Max(currentTimeStamina * currentTimeStamina, 2);
        ModifiyStamina(value);
        ModifyRecoverStamina(-value);
    }

    private void ModifyRecoverStamina(float _value)
    {
        recoverStamina += _value;
        if (recoverStamina < 0)
        {
            currentStamina += recoverStamina;
            recoverStamina = 0;
        }
        if (recoverStamina > maxStamina)
            recoverStamina = maxStamina;

        canvasPlayer.ChangeStamina();

    }

}
