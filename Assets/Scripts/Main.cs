using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public GameObject currentCube;
    public GameObject lastCube;
    public Text scoreText;
    public Text highScore;
    int speed = 100;
    public AudioClip bloop;
    public AudioClip yay;
    public AudioClip boo;
    public int level;
    public Text Text;
    public bool finish;


    // Start is called before the first frame update
    void Start()
    {
        newBlock();
        highScore.text = "High Score " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        currentCube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        
    }
    private void newBlock(){
        AudioSource.PlayClipAtPoint(bloop, Camera.main.transform.position);
        if(lastCube!= null){
            currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x),
            currentCube.transform.position.y,
            Mathf.Round(currentCube.transform.position.z));

            currentCube.transform.localScale = new Vector3(lastCube.transform.localScale.x - Mathf.Abs(currentCube.transform.position.x - lastCube.transform.position.x),
                                                            lastCube.transform.localScale.y,
                                                            lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z - lastCube.transform.position.z));
            
            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, lastCube.transform.position, 0.5f) + Vector3.up * 5f;
            
            
            if(currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f){
                if(level > PlayerPrefs.GetInt("HighScore", 0)) {
                    AudioSource.PlayClipAtPoint(yay, Camera.main.transform.position);
                } else {
                    AudioSource.PlayClipAtPoint(boo, Camera.main.transform.position);

                }
                Text.gameObject.SetActive(true);
                highScore.gameObject.SetActive(false);
                Text.text = "Final Score " + level;
                if(level > PlayerPrefs.GetInt("HighScore", 0)) {
                    PlayerPrefs.SetInt("HighScore",level);
                    highScore.text = level.ToString();
                }
                Invoke("reset", 4);
                
                return;
            }

        }
        
        lastCube = currentCube;
        currentCube = Instantiate(lastCube);
        currentCube.name = level + "";
        level++;
        Camera.main.transform.position = currentCube.transform.position + new Vector3(90,90,90);
        Camera.main.transform.LookAt(currentCube.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(finish){
            return;
        }

        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);
        var pos1 = lastCube.transform.position + Vector3.up * 10f;
        var pos2 = pos1 + ((level % 2 == 0) ? Vector3.left : Vector3.back) * speed;

        if(level % 2  == 0){
            currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        } else{
            currentCube.transform.position = Vector3.Lerp(pos1,pos2,time);
        }

        if(Input.GetMouseButtonDown(0)){
            
            newBlock();
            currentCube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            speed += 20;
        }
        
    }

    public void reset(){
        SceneManager.LoadScene("SampleScene");
 
    }
}
