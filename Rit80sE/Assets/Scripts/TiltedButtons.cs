using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TiltedButtons : MonoBehaviour
{
    private void Start()
    {
        //Проверяет спрайт кнопки на параметр альфа, если ниже порогового, то кнопку нельзя нажать (использовано для наклонной кнопки)
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
