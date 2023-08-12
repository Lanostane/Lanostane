using Lanostane.Charts;
using LST.Player.Graphics;
using LST.Player.Judge;
using LST.Player.Motions;
using LST.Player.Scrolls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using Utils.Maths;

namespace LST.Player
{
    public interface IChartUpdater
    {
        void TimeUpdate(float chartTime);
        void CleanUp();
    }

    public interface IBGUpdater : IChartUpdater
    {

    }

    public interface INoteGraphicUpdater : IChartUpdater
    {
        ISingleNoteGraphic AddSingleNote(LST_SingleNoteInfo info);
        ILongNoteGraphic AddLongNote(LST_LongNoteInfo info);
        void Prepare();
    }

    public interface INoteJudgeUpdater : IChartUpdater
    {
        bool AutoPlay { get; }
        void InitializeScoring();
        void AddSingleJudgeHandle(LST_SingleNoteInfo info, ISingleNoteGraphic graphic);
        void AddLongJudgeHandle(LST_LongNoteInfo info, ILongNoteGraphic graphic);
        bool TryGetInputHandle(int index, out InputHandle handle);
        void InputHandleUpdated(InputHandle updatedInputHandle);
    }

    public interface IMotionUpdater : IChartUpdater
    {
        float CurrentBPM { get; }
        float CurrentRotation { get; }
        float StartingTheta { get; }
        float StartingRho { get; }
        float StartingHeight { get; }
        Vector3 StartingPosition { get; }
        PolarPoint StartingPolar { get; }
        float StartingRotation { get; }

        IMotionWorker Main { get; }

        void AddMotions(LST_Chart chart);
        void Prepare();
        void StartDefaultMotion(float duration);
        bool TryGetBPMByTime(float time, out float bpm);
        void SetDefaultMotion(LST_DefaultMotion defaultMotion);
    }

    public interface IScrollUpdater : IChartUpdater
    {
        float ScrollingSpeed { get; set; }
        void AddScroll(LST_ScrollChange scrollChange);
        void Prepare();
        bool TryGetGroup(ushort scrollGroupID, out ScrollGroup group);
        Millisecond GetScrollTimingByTime(ushort scrollGroupID, float time);
        float GetProgressionSingle(ushort scrollGroupID, float chartTime, float timing, out bool isInScreen);
        float GetProgressionSingleFast(ushort scrollGroupID, Millisecond scrollTiming, out bool isInScreen);
        bool IsScrollRangeVisible(ushort scrollGroupID, Millisecond minAmount, Millisecond maxAmount);
        ScrollProgress[] GetProgressions(ushort scrollGroupID, float chartTime, float[] timings);
        void GetNativeScrollRangeData(ref NativeHashMap<ushort, ScrollRangeData> hashMap);
    }
}
