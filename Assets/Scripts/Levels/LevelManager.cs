using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour

{
    [SerializeField] AudioClip levelMusic;




    // Start is called before the first frame update
    void Start()
    { 
        AudioManager.instance.PlayMusic(levelMusic);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
