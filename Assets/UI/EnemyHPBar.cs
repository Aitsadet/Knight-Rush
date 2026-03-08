using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    public Image hpFill;

    int maxHP;
    int currentHP;

    public void Setup(int hp)
    {
        maxHP = hp;
        currentHP = hp;
        UpdateBar();
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP < 0)
            currentHP = 0;

        UpdateBar();
    }

    void UpdateBar()
    {
        hpFill.fillAmount = (float)currentHP / maxHP;
    }
}