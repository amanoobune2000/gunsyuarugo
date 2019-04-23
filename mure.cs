using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mure_src : MonoBehaviour
{
    //Y 軸方向へ動かないようにするために、Inspector ビューから Rigidbody > Constrains の項目を開き、Freez Position の Y にチェックを入れます。


    public int MaxChild = 30;        //個体の最大数
    public float hani = 20;          //発生させる範囲

    public GameObject BoidsChild;
    public GameObject[] BoidsChildren;

    //群れの中心を求める
    public GameObject BoidsBoss;
    public GameObject BoidsCenter;

    //条件-1 : 各個体は群れの中央へ移動しようとする
    public float Turbulence = 0.2f;//★ここが１だと群れのようにみえず、0.9だと群れっぽくなった
    //乱れを示す係数。その個体が元々進行しようとしていた力と、群れの中央へ移動しようとする力の割合を調節。
    //もし1が与えられれば、その個体は中央へ向かわずに、元々自分が進もうとしていた方向へ進む


    //条件-2 : 各個体は一定の距離をとる
    private float Distance;//2以上？と思われる
    public float Distance_hanit = 3f;//ランダム 2以上？と思われる

    public float start_speed = 0.0001f;

    void Start()
    {

        

        this.BoidsChildren = new GameObject[MaxChild];

        for (int i = 0; i < this.MaxChild; i++)
        {
            this.BoidsChildren[i] =
                GameObject.Instantiate(BoidsChild) as GameObject;

            this.BoidsChildren[i].transform.position
                = new Vector3(Random.Range(-hani, hani),
                              this.BoidsChild.transform.position.y,
                              Random.Range(-hani, hani)); //初期位置はランダム

            //これがないと動かない
            this.BoidsChildren[i].GetComponent<Rigidbody>().AddForce(this.BoidsChildren[i].transform.forward* start_speed, ForceMode.Impulse);

        }
    }

    
    void Update()
    {

        Distance = Random.Range(2, Distance_hanit);

        //★群れの中心を求める

        Vector3 center = Vector3.zero;

        foreach (GameObject child in this.BoidsChildren)
        {
            center += child.transform.position;
        }

        center /= (BoidsChildren.Length - 1);
        center += this.BoidsBoss.transform.position;
        center /= 2;
        this.BoidsCenter.transform.position = center;


        //★条件-1 : 各個体は群れの中央へ移動しようとする

        foreach (GameObject child in this.BoidsChildren)
        {
            Vector3 dirToCenter = (center - child.transform.position).normalized;
            Vector3 direction = (child.GetComponent<Rigidbody>().velocity.normalized * this.Turbulence
                                + dirToCenter * (1 - this.Turbulence)).normalized;

            //direction *= Random.Range(20f, 30f);//値でかすぎ？突き抜ける
            //direction *= Random.Range(5f, 15f);//値小さくする これでも少し早い
            direction *= Random.Range(1f, 10f);//値小さくする

            child.GetComponent<Rigidbody>().velocity = direction;

        }

        //殆どの場合に Turbulence の値は大きい方が生物らしさが現れます。
        //個体が進行する速度も調節しましょう。個体ごとに速度のパラメータを持たせて各個体の個性を出すのも良いですが、
        //ここではランダムです。表現する対象によって速度は適当に変更してください。


        //★条件-2 : 各個体は一定の距離をとる
        //個体間の距離を算出して、指定した距離より短ければ片方の進行方向を変更するか、速度を減少する必要があります。ここでは進行方向を反転するように設定します。

        foreach (GameObject child_a in this.BoidsChildren)
        {
            foreach (GameObject child_b in this.BoidsChildren)
            {
                if (child_a == child_b)
                {
                    continue;
                }

                Vector3 diff = child_a.transform.position - child_b.transform.position;

                if (diff.magnitude < Random.Range(2, this.Distance))//★ここが要確認
                {
                    child_a.GetComponent<Rigidbody>().velocity =
                        diff.normalized * child_a.GetComponent<Rigidbody>().velocity.magnitude;
                }
            }
        }
        //ここでも1つ工夫をしています。距離はフィールド変数 Distance によって指定しているのですが、
        //最小値を 2、最大値を Distance としてランダムに変更しています。この設定によって
        //各個体が侵入を許す距離が変動するのですが、生物の群れを表現する場合には、この方が自然に見えることが多いです。
        // 固定値にすると個体の密度が高い場合には常に均等な距離を取ってしまい、
        //どこか機械的な表現になります。現実的には生物もかなり高度なレベルで距離を
        //取ったりするのですが、極端に表現した方が "らしさ" が増します。
        //ランダムで取りうる値の範囲が小さいほど均整がとれた群れになるので、表現する対象によって適度に調整してください。


        //★条件-3 : 各個体は平均移動ベクトルに合わせようとする

        Vector3 averageVelocity = Vector3.zero;

        foreach (GameObject child in this.BoidsChildren)
        {
            averageVelocity += child.GetComponent<Rigidbody>().velocity;
        }

        averageVelocity /= this.BoidsChildren.Length;

        foreach (GameObject child in this.BoidsChildren)
        {
            child.GetComponent<Rigidbody>().velocity = child.GetComponent<Rigidbody>().velocity * this.Turbulence
                                       + averageVelocity * (1f - this.Turbulence);
        }

    }
}
