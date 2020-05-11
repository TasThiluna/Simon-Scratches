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

    public KMSelectable startButton;
    public KMSelectable armButton;
    public Color[] recordColors;

    public Renderer record;
    public Transform arm;

    private bool armOnRecord;
    private float savedElapsed;

    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        armButton.OnInteract += delegate () { PressArm(); return false; };
    }

    void Start()
    {
        record.materials[1].color = recordColors.PickRandom();
        record.materials[0].color = recordColors.Where(x => record.materials[1].color != x).PickRandom();
    }

    void PressArm()
    {
        if (armOnRecord)
            return;
        StartCoroutine(MoveArm());
    }

    IEnumerator MoveArm()
    {
        armOnRecord = true;
        var startRotation = arm.localEulerAngles.y;
        var endRotation = 35f;
        var elapsed = 0f;
        var duration = 1f;
        while (elapsed < duration)
        {
            arm.localEulerAngles = new Vector3(0f, Easing.OutSine(elapsed, startRotation, endRotation, duration), 0f);
            yield return null;
            elapsed += Time.deltaTime;
        }
        arm.localEulerAngles = new Vector3(0f, 35f, 0f);
        module.GetComponent<KMSelectable>().Children[0] = null;
        module.GetComponent<KMSelectable>().UpdateChildren();
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
