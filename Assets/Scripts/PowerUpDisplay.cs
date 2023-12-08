using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class PowerUpDisplay : MonoBehaviour
{
    private string pow1;
    public TextMeshProUGUI pow1Text;
    public Image img1;
    public Sprite Blue;
    public Sprite Cyan;

    // Update is called once per frame
    void Update()
    {
        pow1Text.text = "Player 1 Powerup: " + pow1;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int num = UnityEngine.Random.Range(1, 10);
            pow1 = num.ToString();
            if (num > 5)
            {
                img1.sprite = Blue;
            }
            else
            {
                img1.sprite = Cyan;
            }
        }
    }
}
