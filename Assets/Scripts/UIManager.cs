using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreTextP;
    [SerializeField]
    private Text _scoreTextSP;
    [SerializeField]
    private Text _deadText;
    [SerializeField]
    private Text _deadContinue;
    [SerializeField]
    private Text _deadQuit;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private SecondPlayer _secondPlayer;
    private AudioSource _audioSource;    

    void Awake() {
        Time.timeScale = 1;
    }
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _secondPlayer = GameObject.Find("SecondPlayer").GetComponent<SecondPlayer>();
        _deadContinue.gameObject.SetActive(false);
        _deadQuit.gameObject.SetActive(false);
    }

    void Update()
    {
        
        _scoreTextP.text = "Score: " + _player.getScoreP().ToString();
        _scoreTextSP.text = "Score: " + _secondPlayer.getScoreSP().ToString();
        if (_player.getScoreP() < 3 && _secondPlayer.getScoreSP() < 3) {
            if (_secondPlayer.healhttwo <= 0) 
            {
                DeadScene();

                if (_secondPlayer.contGameSPlayer) {
                    _secondPlayer.transform.position = new Vector3(9.09f,-3.0f,0);
                    _player.transform.position = new Vector3(-9.57f,-3.0f,0);
                    _player.healthone = _player.maxHealthone;
                    _secondPlayer.healhttwo = _secondPlayer.maxHealthtwo;
                    AssetsController._alreadyScored = false;
                    _deadContinue.gameObject.SetActive(false);
                    _deadQuit.gameObject.SetActive(false);
                    _secondPlayer.contGameSPlayer = false;
                }

                if (_secondPlayer.restartGameSPlayer) {
                    SceneManager.LoadScene(1);
                }

                if (Input.GetKeyDown(KeyCode.Q)) {
                    SceneManager.LoadScene(0);
                }
            }
            else if (_player.healthone <= 0)
            {
                DeadScene();

                if (_player.contGamePlayer)
                {
                    _secondPlayer.transform.position = new Vector3(9.09f, -3.0f, 0);
                    _player.transform.position = new Vector3(-9.57f, -3.0f, 0);
                    _player.healthone = _player.maxHealthone;
                    _secondPlayer.healhttwo = _secondPlayer.maxHealthtwo;
                    AssetsController._alreadyScored = false;
                    _deadContinue.gameObject.SetActive(false);
                    _deadQuit.gameObject.SetActive(false);
                    _player.contGamePlayer = false;
                }

                if (_player.restartGamePlayer) 
                {
                    SceneManager.LoadScene(1);
                }

                if (Input.GetKeyDown(KeyCode.Q)) {
                    SceneManager.LoadScene(0);
                }
            }
            else { _deadText.gameObject.SetActive(false);}
        } 
            if (_player.getScoreP() >= 3) {
                EndOfGameScene(winner: "Player 1");
                if (_secondPlayer.restartGameSPlayer)
                {
                    SceneManager.LoadScene(1);
                }
            }
            if (_secondPlayer.getScoreSP() >= 3) {
                EndOfGameScene(winner: "Player 2");
                if (_player.restartGamePlayer)
                {
                    SceneManager.LoadScene(1);
                }
            }
    }

    private void DeadScene()
    {
        _deadContinue.gameObject.SetActive(true);
        _deadText.gameObject.SetActive(true);
        _deadQuit.gameObject.SetActive(true);
        _deadText.text = "Player is dead";
        _deadContinue.text = "Please jump to continue";
        _deadQuit.text = "Please crouch to quit";
    }

    private void EndOfGameScene(string winner)
    {
        _deadText.gameObject.SetActive(true);
        _deadQuit.gameObject.SetActive(true);
        _deadText.text = winner + " has won";
        _deadQuit.text = "Please crouch to restart";
    }
}
