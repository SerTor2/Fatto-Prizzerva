using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEstaminable
{
    float Stamina { get; set; }
    float MaxStamina { get; set; }
    float CurrentRegenStamina { get; set; }

    void ModifiyStamina(float _stamina, bool _maxStamina = false);
    void RegenStamina();
}
