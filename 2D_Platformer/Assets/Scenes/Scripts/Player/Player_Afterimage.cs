using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_Afterimage : MonoBehaviour
{
    Player player;
    public GameObject[] _playerImages;
    public Transform[] _imagesTransforms;
    public float _frequency;
    private Vector3 lastPosition;
    Vector3 deltaMovement;
    // after Image
    // create 5 image array
    // each image has own position and show in order

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        InitializeArray();
    }

    void Start()
    {
        lastPosition = player.transform.position;
    }

    void Update()
    {
        transform.position = player.transform.position;
    }

    void LateUpdate()
    {
        // need position
        //for (int i = 0; i < _imagesTransforms.Length; i++)
        //{
        //    deltaMovement = player.transform.position - lastPosition;
        //    lastPosition = player.transform.position;
        //}

    }

    public IEnumerator CreateAfterImage()
    {
        for (int i = 0; i < _imagesTransforms.Length; i++)
        {
            _playerImages[i].SetActive(true);
            //_imagesTransforms[i].position += (transform.localPosition + deltaMovement); 
            yield return new WaitForSeconds(_frequency);
        }

        for (int i = 0; i < _imagesTransforms.Length; i++)
        {
            _playerImages[i].SetActive(false);
            yield return new WaitForSeconds(_frequency);
        }

    }

    void InitializeArray()
    {
        _imagesTransforms = new Transform[_playerImages.Length]; // create array

        for(int i = 0; i < _imagesTransforms.Length; i++)
        {
            _imagesTransforms[i] = _playerImages[i].transform;
        }
    }
}
