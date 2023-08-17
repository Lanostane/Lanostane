using Cysharp.Threading.Tasks;
using Loadings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Unity;

namespace LST.GamePlay
{
    internal class LoadGamePlayJob : ILoadJob
    {
        public AudioClip MusicToPlay;
        public TextAsset ChartToLoad;

        public LoadGamePlayJob(AudioClip musicToPlay, TextAsset chartToLoad)
        {
            MusicToPlay = musicToPlay;
            ChartToLoad = chartToLoad;
        }

        public async UniTask Job(IProgress<JobProgress> progressHandle)
        {
            progressHandle.Report(JobProgress.CreateNew("Loading GamePlay"));
            await GamePlays.ChartPlayer.LoadChart(MusicToPlay, ChartToLoad.text, Progress.Create<LoadChartSteps>((step) =>
            {
                progressHandle.Report(JobProgress.CreateNew($"Loading Chart: {step}"));
            }));
        }
    }
}
