using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GivePowerUp : MonoBehaviour
{
    public TextMeshProUGUI runnerPowText;
    public TextMeshProUGUI dropperPowText;
    public string RunnerPowerUp;
    public string DropperPowerUp;
    public PlayerMovement RunnerMovement;
    public NewBoard DropperMovement;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            usePowerUp(RunnerPowerUp, true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            usePowerUp(DropperPowerUp, false);
        }
    }

    public void dropperPowerUpRandomizer()
    {
        int random = UnityEngine.Random.Range(1, 7);
        if (random == 1) {
            givePowerUp(false);
        }
    }

    public void givePowerUp(Boolean runner)
    {
        int powerUpNumber = UnityEngine.Random.Range(1, 4);
        if (runner) {
            if (powerUpNumber == 1)
            {
                RunnerPowerUp = "speedUp";
                runnerPowText.text = "Runner Power Up: Speed Up";
            }
            else if (powerUpNumber == 2)
            {
                RunnerPowerUp = "speedDown";
                runnerPowText.text = "Runner Power Up: Speed Down";
            }
            else if (powerUpNumber == 3)
            {
                RunnerPowerUp = "jumpHeightUp";
                runnerPowText.text = "Runner Power Up: Jump Height Up";
            }
        } else
        {
            if (powerUpNumber == 1)
            {
                DropperPowerUp = "speedUp";
                dropperPowText.text = "Dropper Power Up: Speed Up";
            }
            else if (powerUpNumber == 2)
            {
                DropperPowerUp = "speedDown";
                dropperPowText.text = "Dropper Power Up: Speed Down";
            }
            else if (powerUpNumber == 3)
            {
                DropperPowerUp = "jumpHeightDown";
                dropperPowText.text = "Dropper Power Up: Jump Height Down";
            }
        }
        
    }
    
    public void usePowerUp(string powerUpName, Boolean runner)
    {
        if (runner)
        {
            if (RunnerPowerUp == "speedUp")
            {
                RunnerMovement.movementSpeed += 1;
            }
            else if (powerUpName == "speedDown")
            {
                DropperMovement.velocityY += 1;
            } else if (RunnerPowerUp == "jumpHeightUp")
            {
                RunnerMovement.jumpHeight += 1;
            }
            RunnerPowerUp = "None";
            runnerPowText.text = "Runner Power Up: None";
        }
        else
        {
            if (DropperPowerUp == "speedUp")
            {
                DropperMovement.velocityY -= 1;
            }
            else if (DropperPowerUp == "speedDown")
            {
                RunnerMovement.movementSpeed -= 1;
            }
            else if (DropperPowerUp == "jumpHeightDown")
            {
                RunnerMovement.jumpHeight -= 1;
            }
            DropperPowerUp = "None";
            dropperPowText.text = "Dropper Power Up: None";
        }
        
    }
    
}
