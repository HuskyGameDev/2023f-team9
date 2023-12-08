using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

public class BlockQueue : MonoBehaviour
{
    public Image firstBlock;
    public Image secondBlock;
    public Image thirdBlock;
    public NewBoard queue;
    public Sprite JBlock;
    public Sprite IBlock;
    public Sprite OBlock;
    public Sprite LBlock;
    public Sprite SBlock;
    public Sprite TBlock;
    public Sprite ZBlock;

    public void updateQueue()
    {
        GameObject[] blockQueue = new GameObject[3];
        blockQueue = queue.pieceQueue.ToArray();
        firstBlock.sprite = setImage(blockQueue[0]);
        firstBlock.SetNativeSize();
        secondBlock.sprite = setImage(blockQueue[1]);
        secondBlock.SetNativeSize();
        thirdBlock.sprite = setImage(blockQueue[2]);
        thirdBlock.SetNativeSize();
    }

    public Sprite setImage(GameObject piece)
    {
        if (piece == queue.J_Block)
        {
            return JBlock;
        } else if (piece == queue.I_Block)
        {
            return IBlock;
        } else if (piece == queue.O_Block)
        {
            return OBlock;
        } else if (piece == queue.L_Block)
        {
            return LBlock;
        } else if (piece == queue.S_Block)
        {
            return SBlock;
        } else if (piece == queue.T_Block)
        {
            return TBlock;
        } else
        {
            return ZBlock;
        }
    }
}
