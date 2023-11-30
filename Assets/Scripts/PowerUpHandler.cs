using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GivePowerUp : MonoBehaviour
{
    public TextMeshProUGUI runnerPowText;
    public Image runnerPowImage;
    public TextMeshProUGUI dropperPowText;
    public Image dropperPowImage;
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
            givePowerUp(true);
            givePowerUp(false);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            usePowerUp(RunnerPowerUp, true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            usePowerUp(DropperPowerUp, false);
        }
    }

    public void givePowerUp(Boolean runnerPowerUp)
    {
        int powerUpNumber = UnityEngine.Random.Range(1, 4);
        if (runnerPowerUp) {
            if (powerUpNumber == 1)
            {
                changePowerUp("speedUp", true);
                RunnerPowerUp = "speedUp";
            }
            else if (powerUpNumber == 2)
            {
                changePowerUp("speedDown", true);
                RunnerPowerUp = "speedDown";
            }
            else if (powerUpNumber == 3)
            {
                changePowerUp("jumpHeightUp", true);
                RunnerPowerUp = "jumpHeightUp";
            }
        } else
        {
            if (powerUpNumber == 1)
            {
                changePowerUp("speedUp", false);
                DropperPowerUp = "speedUp";
            }
            else if (powerUpNumber == 2)
            {
                changePowerUp("speedDown", false);
                DropperPowerUp = "speedDown";
            }
            else if (powerUpNumber == 3)
            {
                changePowerUp("jumpHeightUp", false);
                DropperPowerUp = "jumpHeightUp";
            }
        }
        
    }
    public void changePowerUp(string powerUpName, Boolean runner)
    {
        if (runner)
        {
            if (powerUpName == "speedUp")
            {
                RunnerPowerUp = "Speed Up 1";
                runnerPowImage.sprite = Blue;
            }
            else if (powerUpName == "speedDown")
            {
                RunnerPowerUp = "Speed Down 2";
                runnerPowImage.sprite = Cyan;
            }
            else if (powerUpName == "jumpHeightUp")
            {
                RunnerPowerUp = "Jump Height Up 3";
                runnerPowImage.sprite = Orange;
            }
            else
            {
                RunnerPowerUp = "";
                runnerPowImage.sprite = Empty;
            }
            runnerPowText.text = "Runner Power Up: " + RunnerPowerUp;
        } else
        {
            if (powerUpName == "speedUp")
            {
                DropperPowerUp = "Speed Up 1";
                dropperPowImage.sprite = Blue;
            }
            else if (powerUpName == "speedDown")
            {
                DropperPowerUp = "Speed Down 2";
                dropperPowImage.sprite = Cyan;
            }
            else if (powerUpName == "jumpHeightUp")
            {
                DropperPowerUp = "Jump Height Up 3";
                dropperPowImage.sprite = Orange;
            }
            else
            {
                DropperPowerUp = "";
                dropperPowImage.sprite = Empty;
            }
            dropperPowText.text = "Dropper Power Up: " + DropperPowerUp;
        }
        
    }

    public void usePowerUp(string powerUpName, Boolean runner)
    {
        if (runner)
        {
            if (powerUpName == "speedUp")
            {
                RunnerMovement.movementSpeed += 1;
            }
            else if (powerUpName == "speedDown")
            {
                RunnerMovement.movementSpeed -= 1;
            }
            changePowerUp("", true);
        }
        else
        {
            if (powerUpName == "speedUp")
            {
                RunnerMovement.movementSpeed += 1;
            }
            else if (powerUpName == "speedDown")
            {
                RunnerMovement.movementSpeed -= 1;
            }
            changePowerUp("", false);
        }
        
    }
    
}
