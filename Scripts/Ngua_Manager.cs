using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ngua_Manager : MonoBehaviour
{
    public GameObject obj_btn_play;
    public GameObject obj_btn_bet;
    public GameObject obj_btn_delete_all;
    public GameObject obj_btn_delete_one;
    public GameObject panel_input_name;
    public InputField inp_field_name;
    public Transform area_ngua;
    public Transform area_ngua_complete;
    public Transform area_ngua_pet;
    public Transform pos_top;
    public Transform pos_bottom;
    public SmoothCamera2D cam_effect;
    public GameObject con_ngua_prefab;
    public GameObject con_ngua_complete_prefab;
    public GameObject obj_btn_bet_ngua;
    private int length_ngua;
    private List<GameObject> list_con_ngua;
    private List<Con_Ngua> list_con_ngua_complete;
    private float timer_random_speed = 0f;
    private bool is_active;
    private bool is_done;
    private Con_Ngua con_ngua_top = null;

    public void load()
    {
        this.obj_btn_play.SetActive(false);
        this.obj_btn_delete_all.SetActive(false);
        this.panel_input_name.SetActive(false);
        this.is_active = false;
        this.is_done = false;
        this.length_ngua = PlayerPrefs.GetInt("length_ngua",0);
        this.list_con_ngua = new List<GameObject>();
        this.restore_all_ngua();
    }

    public void add_con_ngua()
    {
        this.panel_input_name.SetActive(true);
        this.inp_field_name.text = this.get_name_player();
    }

    private void add(string s_name="",int index_add=-1)
    {
        float rand_y = Random.Range(this.pos_top.position.y, this.pos_bottom.position.y);
        GameObject ngua_obj = Instantiate(this.con_ngua_prefab);
        ngua_obj.name = "ngua_obj";
        ngua_obj.transform.SetParent(this.area_ngua);
        ngua_obj.transform.position = new Vector3(this.pos_top.position.x, rand_y, rand_y);

        ngua_obj.GetComponent<SpriteRenderer>().color = this.get_color_rand();
        ngua_obj.GetComponent<Con_Ngua>().set_color_name(this.get_color_rand());

        if (s_name == "")
        {
            PlayerPrefs.SetString("ngua_name_" + this.length_ngua, this.inp_field_name.text);
            ngua_obj.GetComponent<Con_Ngua>().set_name_title(this.inp_field_name.text);
        }
        else
        {
            ngua_obj.GetComponent<Con_Ngua>().set_name_title(s_name);
        }

        if(index_add==-1)
            ngua_obj.GetComponent<Con_Ngua>().index = this.length_ngua;
        else
            ngua_obj.GetComponent<Con_Ngua>().index = index_add;

        float rand_speed = Random.Range(0.5f,3f);
        ngua_obj.GetComponent<Con_Ngua>().set_speed(rand_speed);
        this.list_con_ngua.Add(ngua_obj);
    }

    public void restore_all_ngua()
    {
        for(int i = 0; i < this.length_ngua; i++)
        {
            string s_name_player = PlayerPrefs.GetString("ngua_name_" + i,"");
            this.add(s_name_player,i);
        }

        this.check_show_btn_func();
    }

    private Color32 get_color_rand()
    {
        byte c_r = (byte)Random.Range(0, 100);
        byte c_g = (byte)Random.Range(0, 100);
        byte c_b = (byte)Random.Range(0, 100);
        return new Color32(c_r, c_g, c_b, 225);
    }

    public void activer_all_con_ngua()
    {
        this.list_con_ngua_complete = new List<Con_Ngua>();
        this.is_done = false;
        this.is_active = true;
        for (int i=0;i < this.list_con_ngua.Count; i++) this.list_con_ngua[i].GetComponent<Con_Ngua>().set_active(true);
    }

    private void ramdom_speed_all_con_ngua()
    {
        for (int i = 0; i < this.list_con_ngua.Count; i++)
        {
            float rand_speed = Random.Range(0.5f,5f);
            this.list_con_ngua[i].GetComponent<Con_Ngua>().set_speed(rand_speed);
        }
    }

    void Update()
    {
        if (this.is_active)
        {
            this.timer_random_speed += 1 * Time.deltaTime;
            if (this.timer_random_speed > 2f)
            {
                this.ramdom_speed_all_con_ngua();
                if(this.con_ngua_top!= this.get_ngua_top())
                {
                    this.con_ngua_top = this.get_ngua_top();
                    if(this.is_done==false)this.cam_effect.set_target(this.con_ngua_top.transform);
                }
                this.timer_random_speed = 0f;
            } 
        }
    }

    public void stop()
    {
        this.is_active = false;
        this.is_done = false;
        this.delete_all_obj_ngua();
        this.cam_effect.transform.position = new Vector3(0f, 0f, -10f);
    }

    public void delete_all_obj_ngua()
    {
        for (int i = 0; i < this.list_con_ngua.Count; i++) Destroy(this.list_con_ngua[i].gameObject);
        this.list_con_ngua = new List<GameObject>();
        this.list_con_ngua_complete = new List<Con_Ngua>();
    }

    public void clear_all_ngua()
    {
        this.obj_btn_delete_all.SetActive(false);
        this.obj_btn_play.SetActive(false);
        this.length_ngua = 0;
        this.delete_all_obj_ngua();
        PlayerPrefs.DeleteKey("length_ngua");
        this.check_show_btn_func();
    }

    private Con_Ngua get_ngua_top()
    {
        int max_index = 0;
        float max_x = this.list_con_ngua[max_index].transform.position.x;
        
        for(int i = 0; i < this.list_con_ngua.Count; i++)
        {
            if (this.list_con_ngua[i].transform.position.x > max_x) {
                max_x = this.list_con_ngua[i].transform.position.x;
                max_index = i;
            }
        }
        return this.list_con_ngua[max_index].GetComponent<Con_Ngua>();
    }

    public void btn_done_input_name()
    {
        PlayerPrefs.SetString("inp_name_"+this.length_ngua,this.inp_field_name.text);
        this.add();
        this.panel_input_name.SetActive(false);
        this.length_ngua++;
        PlayerPrefs.SetInt("length_ngua", this.length_ngua);
        this.check_show_btn_func();
    }

    public void btn_close_ipnut_name()
    {
        this.panel_input_name.SetActive(false);
    }

    public string get_name_player()
    {
        return "Player " + this.length_ngua;
    }

    public void delete_one()
    {
        int last_index = this.list_con_ngua.Count - 1;
        Destroy(this.list_con_ngua[last_index].gameObject);
        this.list_con_ngua.RemoveAt(last_index);
        this.length_ngua--;
        PlayerPrefs.SetInt("length_ngua", this.length_ngua);
    }

    private void check_show_btn_func()
    {
        if (this.length_ngua == 0)
        {
            this.obj_btn_delete_all.SetActive(false);
            this.obj_btn_delete_one.SetActive(false);
            this.obj_btn_play.SetActive(false);
            this.obj_btn_bet.SetActive(false);
        }
        else
        {
            this.obj_btn_delete_all.SetActive(true);
            this.obj_btn_delete_one.SetActive(true);
            this.obj_btn_play.SetActive(true);
            this.obj_btn_bet.SetActive(true);
        }
    }

    public void add_ngua_complete(Con_Ngua con_ngua)
    {
        this.list_con_ngua_complete.Add(con_ngua);
        GameObject obj_con_ngua_complete = Instantiate(this.con_ngua_complete_prefab);
        obj_con_ngua_complete.transform.SetParent(this.area_ngua_complete);
        obj_con_ngua_complete.transform.localScale = new Vector3(1f, 1f, 1f);
        obj_con_ngua_complete.GetComponent<con_ngua_complete_item>().txt_name.text = con_ngua.txt_name_title.text;
        obj_con_ngua_complete.GetComponent<con_ngua_complete_item>().img_icon.color = con_ngua.GetComponent<SpriteRenderer>().color;
    }

    public int count_list_ngua_complete()
    {
        return this.list_con_ngua_complete.Count;
    }

    public Con_Ngua get_list_ngua_complete_first()
    {
        return this.list_con_ngua_complete[0];
    }

    public int count_list_ngua()
    {
        return this.list_con_ngua.Count;
    }

    public void show_bet()
    {
        this.GetComponent<Game_Handle>().carrot.clear_contain(this.area_ngua_pet);
        this.obj_btn_play.SetActive(false);
        for (int i = 0; i < this.list_con_ngua.Count; i++)
        {
            GameObject obj_btn_pet = Instantiate(this.obj_btn_bet_ngua);
            obj_btn_pet.transform.SetParent(this.area_ngua_pet);
            obj_btn_pet.transform.localScale = new Vector3(1f, 1f, 1f);
            obj_btn_pet.GetComponent<Item_bet_ngua>().txt_name_player.text = this.list_con_ngua[i].GetComponent<Con_Ngua>().txt_name_title.text;
            obj_btn_pet.GetComponent<Item_bet_ngua>().txt_name_player.color = this.list_con_ngua[i].GetComponent<Con_Ngua>().sp_icon.color;
            obj_btn_pet.GetComponent<Item_bet_ngua>().img_icon.color = this.list_con_ngua[i].GetComponent<Con_Ngua>().sp_icon.color;
            obj_btn_pet.GetComponent<Item_bet_ngua>().index_bet = this.list_con_ngua[i].GetComponent<Con_Ngua>().index;

        }
    }

    public int get_index_con_ngua_top()
    {
        return this.list_con_ngua_complete[0].index;
    }
}
