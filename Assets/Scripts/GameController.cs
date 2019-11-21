using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // ゲームステートの定義
    enum State
    {
        Ready,
        Play,
        GameOver
    }
    
    State state;
    int score;
    
    public AzarashiController azarashi;
    public GameObject blocks;
    public Text scoreLabel;
    public Text stateLabel;
    
    // Start is called before the first frame update
    void Start()
    {
        // 開始と同時にReadyステートに移行
        Ready();
    }

    // ゲームステートの切り替え
    void LateUpdate()
    {
        // ゲームのステートごとにイベントを監視
        switch(state){
            case State.Ready:
                // タッチしたらゲームスタート
                if(Input.GetButtonDown("Fire1")){
                    GameStart();
                }
                break;
                
            case State.Play:
                // キャラクターが死亡したらゲームオーバー
                if(azarashi.IsDead()){
                    GameOver();
                }
                break;
                
            case State.GameOver:
                // タッチしたらシーンをリロード
                if(Input.GetButtonDown("Fire1")){
                    Reload();
                }
                break;
        }
    }
    
    // Readyステートへの切り替え
    void Ready()
    {
        state = State.Ready;
        
        // 各オブジェクトを無効状態にする
        azarashi.SetSteerActive(false);
        blocks.SetActive(false);
        
        // ラベルを更新
        scoreLabel.text = "Score : " + 0;
        
        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "READY";
    }
    
    // ゲーム開始の処理
    void GameStart()
    {
        state = State.Play;
        
        // 各オブジェクトを有効にする
        azarashi.SetSteerActive(true);
        blocks.SetActive(true);
        
        // 最初の入力だけゲームコントローラから渡す
        azarashi.Flap();
        
        // ラベルを更新
        stateLabel.gameObject.SetActive(false);
        stateLabel.text = "";
    }
    
    // ゲームオーバーの処理
    void GameOver()
    {
        state = State.GameOver;
        
        // シーン中の全てのScrollObjectコンポーネントを探し出す
        ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();
        
        // 全ScrollObjectのスクロール処理を無効にする
        foreach(ScrollObject so in scrollObjects) so.enabled = false;
        
        // ラベルを更新
        stateLabel.gameObject.SetActive(true);
        stateLabel.text = "GAME OVER";
    }
    
    // シーンのリロード
    void Reload()
    {
        // 現在読み込んでいるシーンを再読み込み
        Application.LoadLevel(Application.loadedLevel);
    }
    
    public void IncreaseScore()
    {
        ++score;
        scoreLabel.text = "Score : " + score;
    }
}
