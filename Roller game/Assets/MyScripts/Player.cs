

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//シーン切り替えに使用するライブラリ
public class Player : MonoBehaviour
{
    public Transform m_target = null;
    public float     m_speed  = 5;
    public float     m_attenuation = 0.5f;
    public Material red, blue, yellow;
    private Vector3 m_velocity;

    private string current_color; // 0 = red, 1 = blue, 2 = yellow 

    public Text scoreText;
    public Text gameover;
    public Text  clear;
    private int score;
    //スクリーンサイズによって変わりそう
    public float pow = 0.001f;
    private bool is_Playing = true;
    void Start() {
        //最初の色を設定
        current_color = "Yellow";
        //最初のスコアを設定
        score = 0;
        SetCountText();

        //テキストのアクティブを設定
        gameover.gameObject.SetActive(false);
        clear.gameObject.SetActive(false);

        //gameObject.SetActive(false);
        //gameObject.SetActive(true);

    }

    private void Update() {
        if(is_Playing){ //プレイ中
            m_velocity += ( m_target.position - transform.position ) * m_speed;
            m_velocity *= m_attenuation;
            transform.position += m_velocity *= Time.deltaTime;
            Vector3 look = (m_target.position - transform.position).normalized;
            //Debug.Log("look = " + look);

            if(Input.GetKey(KeyCode.LeftArrow)){
                Vector3 vec = RotateVector(look, new Vector3(0, 1, 0), -90); 
                transform.position += (vec * pow);
            } else if(Input.GetKey(KeyCode.RightArrow)){
                Vector3 vec = RotateVector(look, new Vector3(0, 1, 0), 90);
                transform.position += (vec * pow);
            }

            RayTest();
        } else { //ゲームオーバー or クリア時にリトライ
            /*
            if(Input.GetKeyDown("r")){
                //SceneManager.LoadScene (SceneManager.GetActiveScene().name);
                Application.LoadLevel("Level01");
            }
            */
        }
    }

    Vector3 ProjectVector(Vector3 v, Vector3 axisNormalized){
        return Vector3.Dot(v, axisNormalized) * axisNormalized;
    }
    
    Vector3 RotateVector (Vector3 v, Vector3 axisNormalized, float theta){
        var c = ProjectVector(v, axisNormalized);
        var p = v - c;

        var q = Vector3.Cross(axisNormalized, p);

        var s = Mathf.Cos(theta);
        var t = Mathf.Sin(theta);
        return c + (p * s) + (q * t);
    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log(other.gameObject.tag);
        //自身とボールの色が一致しているときボールを消す
        if(other.gameObject.tag == "clear") {
            gameover.gameObject.SetActive(false);
            clear.gameObject.SetActive(true);
            is_Playing = false;
            GameObject cam = GameObject.Find("Camera");
            cam.GetComponent<CameraController>().is_Playing = false;
        } else if(current_color == other.gameObject.tag) {
                other.gameObject.SetActive(false);
                score++;
                SetCountText();
                //Debug.Log("Get Red Ball!");
        } else if(other.gameObject.tag != "road"){ //違う色にぶつかったとき
            //ゲームオーバー画面を表示
            gameover.gameObject.SetActive(true);
            clear.gameObject.SetActive(false);
            //ボールを停止
            is_Playing = false;
            //カメラを停止
             GameObject cam = GameObject.Find("Camera");
            cam.GetComponent<CameraController>().is_Playing = false;
        }

    }

    void RayTest(){
        //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
        Ray ray = new Ray (transform.position, new Vector3 (0, -1, 0));

        //Rayが当たったオブジェクトの情報を入れる箱
        RaycastHit hit;

        //Rayの飛ばせる距離
        int distance = 10;

        //Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
        Debug.DrawLine (ray.origin, ray.direction * distance, Color.red);

        //もしRayにオブジェクトが衝突したら
        //                 ↓Ray  ↓Rayが当たったオブジェクト ↓距離
        if (Physics.Raycast(ray,out hit,distance)) {
            //Rayが当たったオブジェクトのtagがRed_Floorだったら
            if (hit.collider.tag == "Red_Floor") {
                //プレイヤーにアタッチ               
                GetComponent<Renderer>().material = red;
                current_color = "Red";
            } else if (hit.collider.tag == "Blue_Floor") {
                //プレイヤーにアタッチ               
                GetComponent<Renderer>().material = blue;
                current_color = "Blue";
            } else if (hit.collider.tag == "Yellow_Floor") {
                //プレイヤーにアタッチ               
                GetComponent<Renderer>().material = yellow;
                current_color = "Yellow";
            } 
        }
    }

    void SetCountText(){
        scoreText.text = score.ToString();
    }
}
