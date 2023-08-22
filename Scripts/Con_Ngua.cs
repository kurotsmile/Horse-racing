using TMPro;
using UnityEngine;

public class Con_Ngua : MonoBehaviour
{
    public SpriteRenderer sp_icon;
    public TextMeshPro txt_name_title;
    public GameObject step_prefab;
    public int index;
    private float speed = 1f;
    private float timer_create_step = 0f;
    private bool is_active = false;

    void Update()
    {
        if (this.is_active)
        {
            transform.Translate(Vector3.right * (speed * Time.deltaTime));
            this.timer_create_step += 1 * Time.deltaTime;
            if (timer_create_step > 0.2f)
            {
                GameObject obj_step = Instantiate(this.step_prefab);
                obj_step.transform.SetParent(this.transform.parent);
                obj_step.transform.localScale=new Vector3(obj_step.transform.localScale.x, obj_step.transform.localScale.y, obj_step.transform.localScale.z);
                obj_step.transform.position = new Vector3(this.transform.position.x, this.transform.position.y-0.5f, this.transform.position.z);
                Destroy(obj_step, 4f);
                this.timer_create_step = 0;
            }
        }
    }

    public void set_name_title(string s_name)
    {
        this.txt_name_title.text = s_name;
    }

    public void set_color_name(Color32 col)
    {
        this.txt_name_title.color = col;
    }

    public void set_speed(float speed_new)
    {
        this.speed = speed_new;
    }

    public void set_active(bool status_active)
    {
        this.is_active = status_active;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "finished_line")
        {
            this.is_active = false;
            GameObject.Find("Game").GetComponent<Game_Handle>().ngua_manager.add_ngua_complete(this);
            GameObject.Find("Game").GetComponent<Game_Handle>().show_complete(); 
        }
    }
}
