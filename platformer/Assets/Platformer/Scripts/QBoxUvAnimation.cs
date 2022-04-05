using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QBoxUvAnimation : MonoBehaviour
{
    // How many seconds should frame be on screen?
    public float secondsPerFrame = 0.25f;
    
    // What's the size of each frame? Percentage one frame (question mark) takes up on the texture
    public float frameSize = 0.2f; // 20 percent
    
    // What's the direction the frame is moving to get to the next question mark on the texture?
    // It would be vertical direction so (0, 1)
    public Vector2 frameDirection = new Vector2(0,1);

    public float currentTime = 0f;
    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }
    
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > secondsPerFrame)
        {
            currentTime = 0f;
            _material.mainTextureOffset = new Vector2(_material.mainTextureOffset.x, _material.mainTextureOffset.y + frameSize);
            _material.mainTextureOffset = _material.mainTextureOffset + frameDirection;
        }

    }
}
