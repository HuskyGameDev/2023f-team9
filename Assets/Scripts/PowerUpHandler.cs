using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GivePowerUp : MonoBehaviour
{
    private string pow1;
    public TextMeshProUGUI pow1Text;
    public Image img1;
    public Sprite Blue;
    public Sprite Cyan;
    public Sprite Orange;
    public Sprite Empty;
    public string RunnerPowerUp;
    public string DropperPowerUp;
    public PlayerMovement RunnerMovement;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            givePowerUp();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            usePowerUp(RunnerPowerUp);
        }
    }

    public void givePowerUp()
    {
        int powerUpNumber = Random.Range(1, 4);
        if (powerUpNumber == 1)
        {
            changePowerUp("speedUp");
            RunnerPowerUp = "speedUp";
        } else if (powerUpNumber == 2)
        {
            changePowerUp("speedDown");
            RunnerPowerUp = "speedDown";
        } else if (powerUpNumber == 3)
        {
            changePowerUp("jumpHeightUp");
            RunnerPowerUp = "jumpHeightUp";
        }
    }
    public void changePowerUp(string powerUpName)
    {
        if (powerUpName == "speedUp")
        {
            pow1 = "Speed Up 1";
            img1.sprite = Blue;
        } else if (powerUpName == "speedDown")
        {
            pow1 = "Speed Down 2";
            img1.sprite = Cyan;
        } else if (powerUpName == "jumpHeightUp")
        {
            pow1 = "Jump Height Up 3";
            img1.sprite = Orange;
        } else
        {
            pow1 = "";
            img1.sprite = Empty;
        }
        pow1Text.text = pow1;
    }

    public void usePowerUp(string powerUpName)
    {
        if (powerUpName == "speedUp")
        {
            RunnerMovement.movementSpeed += 1;
        } else if (powerUpName == "speedDown")
        {
            RunnerMovement.movementSpeed -= 1;
        }
        changePowerUp("");
    }
    
}
