using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_bet_ngua : MonoBehaviour
{
    public Text txt_name_player;
    public Image img_icon;
    public int index_bet;

    public void click()
    {
        GameObject.Find("Game").GetComponent<Game_Handle>().set_bet_info(this.img_icon.color, this.txt_name_player.text,this.index_bet);
    }
}
