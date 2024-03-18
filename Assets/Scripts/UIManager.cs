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

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _secondPlayer = GameObject.Find("SecondPlayer").GetComponent<SecondPlayer>();
        _deadContinue.gameObject.SetActive(false);
    }

    void Update()
    {
        _scoreTextP.text = "Score: " + _player.getScoreP().ToString();
        _scoreTextSP.text = "Score: " + _secondPlayer.getScoreSP().ToString();
        if (_secondPlayer.healhttwo <= 0) 
        {
            DeadScene();

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.I)) {
                _secondPlayer.transform.position = new Vector3(9.09f,-3.0f,0);
                _player.transform.position = new Vector3(-9.57f,-3.0f,0);
                _player.healthone = _player.maxHealthone;
                _secondPlayer.healhttwo = _secondPlayer.maxHealthtwo;
                _deadContinue.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.K)) {
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                SceneManager.LoadScene(0);
            }
        }
        else if (_player.healthone <= 0)
        {
            DeadScene();

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.I))
            {
                _secondPlayer.transform.position = new Vector3(9.09f, -3.0f, 0);
                _player.transform.position = new Vector3(-9.57f, -3.0f, 0);
                _player.healthone = _player.maxHealthone;
                _secondPlayer.healhttwo = _secondPlayer.maxHealthtwo;
                _player._alreadyScored = false;
                _deadContinue.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.K))
            {
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                SceneManager.LoadScene(0);
            }
        }
        else { _deadText.gameObject.SetActive(false);}
    }

    private void DeadScene()
    {
        _deadContinue.gameObject.SetActive(true);
        _deadText.gameObject.SetActive(true);
        _deadText.text = "Player is dead";
        _deadContinue.text = "Please jump to continue";
        _deadQuit.text = "Please crouch to quit";
    }
}
