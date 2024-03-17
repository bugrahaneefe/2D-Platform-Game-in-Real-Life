using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    private Player _player;
    [SerializeField]
    private SecondPlayer _secondPlayer;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _secondPlayer = GameObject.Find("SecondPlayer").GetComponent<SecondPlayer>();
        _deadContinue.text = "";
    }

    void Update()
    {
        _scoreTextP.text = "Score: " + _player.getScoreP().ToString();
        _scoreTextSP.text = "Score: " + _secondPlayer.getScoreSP().ToString();
        if (_secondPlayer.healhttwo <= 0) 
        {
            
             _deadText.gameObject.SetActive(true);
            _deadText.text = "Second player is dead";
            _deadContinue.text = "Fire to continue";

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.P)) {
                _secondPlayer.transform.position = new Vector3(9.09f,-3.0f,0);
                _player.transform.position = new Vector3(-9.57f,-3.0f,0);
                _player.healthone = _player.maxHealthone;
                _secondPlayer.healhttwo = _secondPlayer.maxHealthtwo;
            }
        }
        else if (_player.healthone <= 0) 
        {
            _deadText.gameObject.SetActive(true);
            _deadText.text = "Player is dead";
            _deadContinue.text = "Fire to continue";

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.P)) {
                _secondPlayer.transform.position = new Vector3(9.09f,-3.0f,0);
                _player.transform.position = new Vector3(-9.57f,-3.0f,0);
                _player.healthone = _player.maxHealthone;
                _secondPlayer.healhttwo = _secondPlayer.maxHealthtwo;
            }

        } else { _deadText.gameObject.SetActive(false);}
    }
}