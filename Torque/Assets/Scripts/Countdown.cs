using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    public GameObject Countdown;
    public AudioSource GetReady;
    public AudioSource GoAudio;
    public GameObject LapTimer;
    public GameObject Car;
    public GameObject CarAI;
    private Rigidbody rb;
    private Rigidbody rbAI;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(CountStart());
        rb = Car.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        rbAI = CarAI.GetComponent<Rigidbody>();
        rbAI.isKinematic = true;
    }


    IEnumerator CountStart()
    {
        yield return new WaitForSeconds(0.5f);
        Countdown.GetComponent<Text>().text = "3";
        GetReady.Play();
        Countdown.SetActive(true);
        yield return new WaitForSeconds(1);
        Countdown.SetActive(false);
        Countdown.GetComponent<Text>().text = "2";
        GetReady.Play();
        Countdown.SetActive(true);
        yield return new WaitForSeconds(1);
        Countdown.SetActive(false);
        Countdown.GetComponent<Text>().text = "1";
        GetReady.Play();
        Countdown.SetActive(true);
        yield return new WaitForSeconds(1);
        Countdown.SetActive(false);
        GoAudio.Play();
        yield return new WaitForSeconds(0.5f);
        LapTimer.SetActive(true);
        rb.isKinematic = false;
        rbAI.isKinematic = false;

    }

}
