using Charts.DTO;
using GamePlay.Graphics;
using GamePlay.Judge;
using GamePlay.Motions;
using GamePlay.Scrolls;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

public sealed class ChartPlayer : MonoBehaviour
{
    public static bool ChartLoaded { get; private set; }
    public static float MusicTime { get; private set; }
    public static float ChartTime { get; private set; }
    public static float OffsetChartTime { get; private set; }

    public TextAsset Chart;
    public AudioClip Music;

    public AudioSource Audio;
    public float Offset = 0.01f;
    [Range(0.1f, 8.0f)] public float PlaySpeed = 1.0f;
    public bool StartChartOnLoaded;

    private bool _ChartPlaying = false;

    void Start()
    {
        LoadChart(Music, Chart.text);
    }

    void OnDestroy()
    {
        ResetValues();
    }

    void OnValidate()
    {
        Audio.pitch = PlaySpeed;

    }

    private void LoadChart(AudioClip music, string json)
    {
        ResetValues();

        Audio.clip = music;
        MusicTime = Audio.clip.length;

        var laChart = JsonConvert.DeserializeObject<LaChart>(json);
        var chart = laChart.CreateLanostaneChart();

        MotionManager.Instance.SetDefaultMotion(chart.Default);
        MotionManager.Instance.AddMotions(chart);
        MotionManager.Instance.UpdateAbsValue();

        foreach (var scroll in chart.Scrolls)
        {
            ScrollManager.Instance.AddScroll(scroll);
        }
        ScrollManager.Instance.UpdateAbsValue();

        foreach (var note in chart.TapNotes)
        {
            var graphic = NoteGraphicManager.Instance.AddSingleNote(note.NoteInfo);
            NoteJudgeManager.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
        }

        foreach (var note in chart.CatchNotes)
        {
            var graphic = NoteGraphicManager.Instance.AddSingleNote(note.NoteInfo);
            NoteJudgeManager.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
        }

        foreach (var note in chart.FlickNotes)
        {
            var graphic = NoteGraphicManager.Instance.AddSingleNote(note.NoteInfo);
            NoteJudgeManager.Instance.AddSingleJudgeHandle(note.NoteInfo, graphic);
        }

        foreach (var note in chart.HoldNotes)
        {
            var graphic = NoteGraphicManager.Instance.AddLongNote(note.NoteInfo);
            NoteJudgeManager.Instance.AddLongJudgeHandle(note.NoteInfo, graphic);
        }

        NoteJudgeManager.Instance.InitializeScoring();

        ChartLoaded = true;
        if (StartChartOnLoaded)
        {
            StartChart();
        }
    }

    private void ResetValues()
    {
        ChartLoaded = false;
        MusicTime = 0.0f;
        ChartTime = 0.0f;
        OffsetChartTime = 0.0f;
    }

    private void CleanupCharts()
    {

    }

    public void Pause()
    {
        if (ChartLoaded && _ChartPlaying)
        {
            Audio.Pause();
        }
    }

    public void Resume()
    {
        if (ChartLoaded && !_ChartPlaying)
        {
            Audio.UnPause();
        }
    }

    void StartChart()
    {
        Audio.Play();
        Audio.pitch = PlaySpeed;
        ChartTime = 0.0f;
        OffsetChartTime = 0.0f;
        _ChartPlaying = true;
    }

#if !UNITY_EDITOR
    void FixedUpdate()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
#endif

    void Update()
    {
        if (_ChartPlaying && Audio.isPlaying)
        {
            ChartTime += (Time.deltaTime * PlaySpeed);

            var audioTime = Audio.time;
            if (!MathfE.AbsApprox(ChartTime, audioTime, 0.05f))
            {
                ChartTime = audioTime;
            }

            OffsetChartTime = ChartTime + Offset;
            ScrollManager.Instance.UpdateChart(OffsetChartTime);
            NoteJudgeManager.Instance.UpdateChart(OffsetChartTime);
            MotionManager.Instance.UpdateChart(OffsetChartTime);

            //_sw.Restart();
            NoteGraphicManager.Instance.UpdateChart(OffsetChartTime);
            //_sw.Stop();
            //Debug.LogError($"NoteGraphic Batch: {_sw.ElapsedTicks} ticks");
        }
    }
}
