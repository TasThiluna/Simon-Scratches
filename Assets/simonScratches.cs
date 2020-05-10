using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using rnd = UnityEngine.Random;

public class simonScratches : MonoBehaviour
{
    public new KMAudio audio;
    public KMBombInfo bomb;
    public KMBombModule module;

    public Renderer record;
    public Transform arm;

    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
    }

    void Start()
    {
        record.materials[1].color = Color.blue;
        record.materials[0].color = Color.cyan;
        StartCoroutine(MoveArm());
    }

    IEnumerator MoveArm()
    {
        yield return new WaitForSeconds(2f);
        var elapsed = 0f;
        var duration = 1f;
        while (elapsed < duration)
        {
            arm.localEulerAngles = new Vector3(0f, Easing.OutSine(elapsed, 0f, 35f, duration), 0f);
            yield return null;
            elapsed += Time.deltaTime;
        }
    }

    // Twitch Plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = "!{0} ";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string input)
    {
        yield return null;
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
    }
}
