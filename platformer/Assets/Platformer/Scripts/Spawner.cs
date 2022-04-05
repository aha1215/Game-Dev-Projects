using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject smallBricks;
    public GameObject coin;

    public void SpawnCoins(Vector3 pos, float destroyTime)
    {
        var coin1 = CreateInstance(this.coin, pos, destroyTime);
        coin1.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);
    }

    public void SpawnBricks(Vector3 pos, float destroyTime)
    {
        var brick1 = CreateInstance(smallBricks, pos, destroyTime);
        brick1.AddForce(new Vector3(-5f,5f,0f), ForceMode.Impulse);
        brick1.AddTorque(new Vector3(-5f, 5f, 1f), ForceMode.Impulse);

        var brick2 = CreateInstance(smallBricks, pos, destroyTime);
        brick2.AddForce(new Vector3(5f,5f,0f), ForceMode.Impulse);
        brick2.AddTorque(new Vector3(5f, 5f, 1f), ForceMode.Impulse);

        var brick3 = CreateInstance(smallBricks, pos, destroyTime);
        brick3.AddForce(new Vector3(-3f,3f,0f), ForceMode.Impulse);
        brick3.AddTorque(new Vector3(-3f, 3f, 1f), ForceMode.Impulse);

        var brick4 = CreateInstance(smallBricks, pos, destroyTime);
        brick4.AddForce(new Vector3(3f,3f,0f), ForceMode.Impulse);
        brick4.AddTorque(new Vector3(3f, 3f, 1f), ForceMode.Impulse);
    }

    Rigidbody CreateInstance(GameObject instanceType, Vector3 pos, float destroyTime)
    {
        var instance = Instantiate(instanceType, pos, Quaternion.identity);
        var rb = instance.GetComponent<Rigidbody>();

        Destroy(instance, destroyTime);
        return rb;
    }
}
