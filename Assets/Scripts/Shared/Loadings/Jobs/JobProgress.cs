using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loadings
{
    public struct JobProgress
    {
        public string BatchName;
        public float? Progress;

        public JobProgress CopyWithNewProgress(float progress)
        {
            return new JobProgress()
            {
                BatchName = BatchName,
                Progress = progress
            };
        }

        public JobProgress CopyWithNewProgress(float progress, float minProgress, float maxProgress)
        {
            return new JobProgress()
            {
                BatchName = BatchName,
                Progress = Mathf.InverseLerp(minProgress, maxProgress, progress)
            };
        }

        public static JobProgress CreateNew(string batchName)
        {
            return new JobProgress()
            {
                BatchName = batchName,
                Progress = 0.0f
            };
        }

        public static JobProgress CreateDone(string batchName)
        {
            return new JobProgress()
            {
                BatchName = batchName,
                Progress = 1.0f
            };
        }
    }
}
