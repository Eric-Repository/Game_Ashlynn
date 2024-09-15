using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public KeyCode _Key;
    public GameObject obj;
    public Button _Button;
    public Sprite img1;
    public Sprite img2;
    public Sprite img3;



    void Awake()
    {
    }



    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

       


        if (Input.GetKeyDown(_Key))
        {
            //dino
            if(_Key == KeyCode.Alpha4 && QuestGiver2.instance.dinoActive)
            {
                obj.GetComponent<Button>().onClick.Invoke();
                obj.GetComponent<Image>().sprite = img3;
                //_Button.onClick.Invoke();
                //dolphin
            }else if (_Key == KeyCode.Alpha3 && QuestGiver3.instance.dolphinActive)
            {
                obj.GetComponent<Button>().onClick.Invoke();
                obj.GetComponent<Image>().sprite = img2;
                //human
            }
            else if (_Key == KeyCode.Alpha1)
            {
                _Button.onClick.Invoke();

            }
            else if (_Key == KeyCode.Alpha2 && QuestGiver4.instance.birdActive)
            {
                obj.GetComponent<Button>().onClick.Invoke();
                obj.GetComponent<Image>().sprite = img1;

            }

        }
    }
}
