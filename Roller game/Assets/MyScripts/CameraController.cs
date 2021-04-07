

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//シーン切り替えに使用するライブラリ
public class CameraController : MonoBehaviour
{
    public Transform m_target = null;
    public Transform look_target = null;
    public float     m_speed  = 5;
    public float     m_attenuation = 0.5f;
    private Vector3 m_velocity;
    public bool is_Playing = true;
    void Start() {
        //transform.position = new Vector3(-23.22773f, -7.038884f, 0.0f);
    }

    private void Update() {
        if(is_Playing){ //プレイ中
            //m_velocity += ( m_target.position - transform.position ) * m_speed;
            //m_velocity *= m_attenuation;
            //transform.position += m_velocity *= Time.deltaTime;
            transform.LookAt(look_target);
        } 
    }

}
