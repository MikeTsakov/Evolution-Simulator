using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeed : MonoBehaviour {

    private Button btnPlay;
    private Button btnTwice;
    private Button btnFour;
    private Button btnEight;
    private Button btnSixteen;
    private Button[] buttons;
    private bool selected;
    [HideInInspector] public bool paused = false;

    private void Awake() {
        buttons = Button.FindObjectsOfType<Button>();
        btnPlay = buttons[3];
        btnTwice = buttons[4];
        btnFour = buttons[2];
        btnEight = buttons[0];
        btnSixteen = buttons[1];
    }

    private void Start() {
        Play();
    }

    private void Update() {
        if (selected) {
            btnPlay.GetComponentInChildren<Text>().text = "Pause";
            btnPlay.onClick.AddListener(Pause);
        } else if (!selected) {
            btnPlay.GetComponentInChildren<Text>().text = "Play";
            btnPlay.onClick.AddListener(Play);
        }
    }

    public void Play () {
        Time.timeScale = 1.0F;
        btnPlay.Select();
        selected = true;
        paused = false;
    }

    public void Pause () {
        Time.timeScale = 0.0F;
        btnPlay.Select();
        selected = false;
        paused = true;
    }

    public void TwiceFast () {
        Time.timeScale = 2.0F;
        btnTwice.Select();
        selected = false;
        paused = false;
    }

    public void FourFast () {
        Time.timeScale = 4.0F;
        btnFour.Select();
        selected = false;
        paused = false;
    }

    public void EightFast () {
        Time.timeScale = 8.0F;
        btnEight.Select();
        selected = false;
        paused = false;
    }

    public void SixteenFast () {
        Time.timeScale = 16.0F;
        btnSixteen.Select();
        selected = false;
        paused = false;
    }
}
