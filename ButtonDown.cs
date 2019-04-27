using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDown : MonoBehaviour
{
    public float speed = 0.01f;//ボタンの下へ移動するスピード
    public float min_Y = 0.07f;//ボタンの下へ移動する距離

    private bool push_f = false;//マウスでクリックされたら、trueとなる
    private float init_y;
    private AudioSource sound01;
    private bool sound_f = false;//音を鳴らしたら、true

    private bool button_on_f = false;//ボタン初期状態：false ボタンが押された状態：true

    private void Start()
    {
        init_y = this.gameObject.transform.position.y;//Yの初期位置を保存
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound01 = audioSources[0];
    }

    private void Update()
    {
        if (push_f)//マウスでクリック
        {
            //ボタンが初期状態の場合
            if (button_on_f == false)
            {
                if (this.gameObject.transform.position.y > init_y - min_Y)
                {
                    Vector3 pos = this.gameObject.transform.position;
                    this.gameObject.transform.position = new Vector3(pos.x, pos.y - speed, pos.z);

                    if (sound_f == false)
                    {
                        sound01.PlayOneShot(sound01.clip);//ボタンを押す音
                        sound_f = true;
                    }
                }
                else
                {
                    button_on_f = true;
                    push_f = false;
                }
                
            }
            else
            {
                //ボタンがすでに押された状態の場合
                if (this.gameObject.transform.position.y < init_y)
                {
                    Vector3 pos = this.gameObject.transform.position;
                    this.gameObject.transform.position = new Vector3(pos.x, pos.y + speed, pos.z);
                }
                else
                {
                    sound_f = false;
                    button_on_f = false;
                    push_f = false;
                }
                
            }
        }
        
    }

    public void button_push()
    {
        push_f = true;
    }
}
