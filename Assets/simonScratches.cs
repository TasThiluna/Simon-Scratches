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
    public GameObject[] ports;
    public Light[] rjLights;

    private int[] recordColorIndices = new int[2];
    private int portIndex;
    private bool isCCW;

    private Coroutine recordMovement;
    private bool spinning;
    private bool fullSpeed;
    private bool armOnRecord;

    private static readonly string[] colorNames = new string[8] { "red", "green", "blue", "yellow", "cyan", "magenta", "black", "white" };

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
        float scalar = transform.lossyScale.x;
        foreach (Light l in rjLights)
            l.range *= scalar;
        portIndex = rnd.Range(0, ports.Length);
        foreach (GameObject port in ports)
            port.SetActive(Array.IndexOf(ports, port) == portIndex);
        isCCW = rnd.Range(0, 2) == 0;
        recordColorIndices[0] = rnd.Range(0, 8);
        recordColorIndices[1] = rnd.Range(0, 8);
        while (recordColorIndices[1] == recordColorIndices[0])
            recordColorIndices[1] = rnd.Range(0, 8);
        record.materials[1].color = recordColors[recordColorIndices[0]];
        record.materials[0].color = recordColors[recordColorIndices[1]];
        for (int i = 0; i < 2; i++)
            Debug.LogFormat("[Simon Scratches #{0}] The {1} part of the record is {2}.", moduleId, i == 0 ? "inner" : "outer", colorNames[recordColorIndices[i]]);
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
        if (moduleSolved)
            return;
        if (!spinning)
            StartCoroutine(RecordTransition(0f, 100f, true));
        else if (spinning && !fullSpeed)
            return;
        else if (spinning && fullSpeed)
        {
            StopCoroutine(recordMovement);
            StartCoroutine(RecordTransition(100f, 0f, false));
        }
    }

    void PressArmDial()
    {

    }

    IEnumerator RecordTransition(float startSpeed, float endSpeed, bool spinningAfter)
    {
        spinning = true;
        var elapsed = 0f;
        var duration = 4f;
        while (elapsed < duration)
        {
            var accel = Mathf.Lerp(startSpeed, endSpeed, elapsed / duration);
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
        spinning = spinningAfter;
        if (startSpeed == 0f)
            recordMovement = StartCoroutine(SpinRecord());
        yield break;
    }

    IEnumerator SpinRecord()
    {
        spinning = true;
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
