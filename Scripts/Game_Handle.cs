using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Handle : MonoBehaviour
{
    public Carrot.Carrot carrot;
    public Ngua_Manager ngua_manager;
    public GameObject panel_main;
    public GameObject panel_play;
    public GameObject panel_complete;
    public GameObject panel_main_bet;
    public AudioSource []sounds;
    private bool is_mode_bet = false;
    private int scores_bet = 0;

    [Header("Info Bet")]
    public GameObject panel_bet_info_play;
    public Text txt_bet_name_player;
    public Text txt_scores_player;
    public Text txt_scores_player_main;
    public Image img_icon_player;
    private int index_bet = -1;

    void Start()
    {
        this.panel_main.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_complete.SetActive(false);
        this.panel_main_bet.SetActive(false);
        this.panel_bet_info_play.SetActive(false);
        this.carrot.Load_Carrot(check_exit_app);
        ngua_manager.load();

        this.scores_bet = PlayerPrefs.GetInt("scores_bet",0);
        this.update_score_bet();
        if (this.carrot.get_status_sound()) this.carrot.game.load_bk_music(this.sounds[3]);
    }

    private void check_exit_app()
    {
        if (this.panel_play.activeInHierarchy)
        {
            this.btn_back_main();
            this.carrot.set_no_check_exit_app();
        }else if (this.panel_main_bet.activeInHierarchy)
        {
            this.btn_close_mode_bet();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.panel_complete.activeInHierarchy)
        {
            this.btn_back_main();
            this.carrot.set_no_check_exit_app();
        }
        else if (this.ngua_manager.panel_input_name.activeInHierarchy)
        {
            this.ngua_manager.btn_close_ipnut_name();
            this.carrot.set_no_check_exit_app();
        }  
    }

    public void btn_add_ngua()
    {
        this.carrot.ads.show_ads_Interstitial();
        this.carrot.play_sound_click();
        ngua_manager.add_con_ngua();
    }

    private void start_all_con_ngua()
    {
        this.play_sound(0);
        this.play_sound(1);
        this.panel_main.SetActive(false);
        this.panel_play.SetActive(true);
        this.carrot.play_sound_click();
        this.ngua_manager.activer_all_con_ngua();
    }

    public void btn_play_all_ngua()
    {
        if (this.ngua_manager.count_list_ngua() < 2)
        {
            this.carrot.show_msg("Horse racing", "You need two or more horses to get started");
            return;
        }

        this.is_mode_bet = false;
        this.start_all_con_ngua();
        this.panel_bet_info_play.SetActive(false);
    }

    public void btn_delete_all_ngua()
    {
        this.carrot.play_sound_click();
        this.ngua_manager.clear_all_ngua();
    }

    public void btn_delete_one()
    {
        this.carrot.play_sound_click();
        this.ngua_manager.delete_one();
    }

    public void btn_back_main()
    {
        this.carrot.ads.show_ads_Interstitial();
        if (this.is_mode_bet)
        {
            this.ngua_manager.obj_btn_bet.SetActive(true);
            this.ngua_manager.obj_btn_play.SetActive(false);
        }
        else
        {
            this.ngua_manager.obj_btn_bet.SetActive(true);
            this.ngua_manager.obj_btn_play.SetActive(true);
        }

        this.sounds[0].Stop();
        this.carrot.play_sound_click();
        this.panel_main.SetActive(true);
        this.panel_play.SetActive(false);
        this.panel_complete.SetActive(false);
        this.panel_bet_info_play.SetActive(false);

        this.carrot.clear_contain(this.ngua_manager.area_ngua);
        this.ngua_manager.stop();
        this.ngua_manager.restore_all_ngua();
    }

    public void show_complete()
    {
        if (this.ngua_manager.count_list_ngua_complete() == 1)
        {
            this.carrot.play_vibrate();
            this.carrot.clear_contain(this.ngua_manager.area_ngua_complete);
            this.play_sound(2);
            this.ngua_manager.add_ngua_complete(this.ngua_manager.get_list_ngua_complete_first());

            if (this.is_mode_bet)
            {
                if (this.index_bet == this.ngua_manager.get_index_con_ngua_top())
                {
                    this.scores_bet += this.ngua_manager.count_list_ngua();
                    PlayerPrefs.SetInt("scores_bet", this.scores_bet);
                    this.carrot.game.update_scores_player(this.scores_bet);
                    this.update_score_bet(); 
                }
            }
        }

        this.panel_complete.SetActive(true);
        this.panel_play.SetActive(false);
    }

    public void btn_close_complete()
    {
        this.carrot.play_sound_click();
        this.panel_play.SetActive(false);
        this.panel_complete.SetActive(false);
    }

    public void btn_show_ranks()
    {
        this.carrot.game.Show_List_Top_player();
    }

    public void btn_show_rate()
    {
        this.carrot.show_rate();
    }

    public void btn_show_share()
    {
        this.carrot.show_share();
    }

    public void btn_show_user()
    {
        this.carrot.user.show_login();
    }

    public void btn_show_setting()
    {
        Carrot.Carrot_Box box_setting=this.carrot.Create_Setting();
        box_setting.set_act_before_closing(after_close_setting);
    }

    private void after_close_setting(List<string> list_item_change)
    {
        foreach(string s in list_item_change)
        {
            if (s == "list_bk_music") this.carrot.game.load_bk_music(this.sounds[3]);
        }

        if (this.carrot.get_status_sound())
            this.sounds[3].Play();
        else
            this.sounds[3].Stop();
    }

    public void btn_show_mode_bet()
    {
        this.carrot.play_sound_click();

        if (this.ngua_manager.count_list_ngua() < 2)
        {
            this.carrot.show_msg("Horse racing", "You need two or more horses to get started");
            return;
        }
        this.ngua_manager.obj_btn_bet.SetActive(false);
        this.panel_main_bet.SetActive(true);
        this.ngua_manager.show_bet();
    }

    public void btn_close_mode_bet()
    {
        this.ngua_manager.obj_btn_bet.SetActive(true);
        this.ngua_manager.obj_btn_play.SetActive(true);
        this.carrot.play_sound_click();
        this.panel_main_bet.SetActive(false);
    }

    public void play_sound(int index_sound)
    {
        if (this.carrot.get_status_sound()) this.sounds[index_sound].Play();
    }

    public void set_bet_info(Color32 color_bet,string s_name_player,int index_bet_play)
    {
        this.index_bet = index_bet_play;
        this.is_mode_bet = true;
        this.carrot.play_sound_click();
        this.img_icon_player.color = color_bet;
        this.txt_bet_name_player.text = s_name_player;
        this.start_all_con_ngua();
        this.panel_bet_info_play.SetActive(true);
    }

    private void update_score_bet()
    {
        this.txt_scores_player.text = this.scores_bet.ToString();
        this.txt_scores_player_main.text = this.scores_bet.ToString();
    }
}
