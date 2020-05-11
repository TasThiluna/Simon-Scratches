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
    public KMSelectable armDial;
    public Color[] recordColors;

    public Renderer record;
    public Transform arm;

    private bool isCCW;

    private Coroutine recordMovement;
    private bool spinning;
    private bool fullSpeed;
    private bool armOnRecord;

    private static int moduleIdCounter = 1;
    private int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        armButton.OnInteract += delegate () { PressArm(); return false; };
        startButton.OnInteract += delegate () { PressStart(); return false; };
        armDial.OnInteract += delegate () { PressArmDial(); return false; };
    }

    void Start()
    {
        isCCW = rnd.Range(0, 2) == 0;
        record.materials[1].color = recordColors.PickRandom();
        record.materials[0].color = recordColors.Where(x => record.materials[1].color != x).PickRandom();
    }

    void PressArm()
    {
        if (armOnRecord || !spinning || moduleSolved)
            return;
        StartCoroutine(MoveArm(arm.localEulerAngles.y, 30f));
    }

    void PressStart()
    {
        audio.PlaySoundAtTransform("click", startButton.transform);
        startButton.AddInteractionPunch(.1f);
        if (spinning || moduleSolved)
            return;
        StartCoroutine(StartRecordSpin());
    }

    void PressArmDial()
    {

    }

    IEnumerator StartRecordSpin()
    {
        spinning = true;
        var elapsed = 0f;
        var duration = 4f;
        while (elapsed < duration)
        {
            var accel = Mathf.Lerp(0f, 100f, elapsed / duration);
			var framerate = 1f / Time.deltaTime;
			var rotation = accel / framerate;
			if (isCCW)
				rotation *= -1;
			var y = record.transform.localEulerAngles.y;
			y += rotation;
			record.transform.localEulerAngles = new Vector3(0f, y, 90f);
			yield return null;
			elapsed += Time.deltaTime;
        }
        recordMovement = StartCoroutine(SpinRecord());
        yield break;
    }

    IEnumerator SpinRecord()
    {
        fullSpeed = true;
        while (true)
		{
			var framerate = 1f / Time.deltaTime;
			var rotation = 100f / framerate;
			if (isCCW)
				rotation *= -1;
			var y = record.transform.localEulerAngles.y;
			y += rotation;
			record.transform.localEulerAngles = new Vector3(0f, y, 90f);
			yield return null;
		}
    }

    IEnumerator MoveArm(float startRotation, float endRotation)
    {
        armOnRecord = true;
        var elapsed = 0f;
        var duration = 1f;
        while (elapsed < duration)
        {
            arm.localEulerAngles = new Vector3(0f, Easing.OutSine(elapsed, startRotation, endRotation, duration), 0f);
            yield return null;
            elapsed += Time.deltaTime;
        }
        arm.localEulerAngles = new Vector3(0f, endRotation, 0f);
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
