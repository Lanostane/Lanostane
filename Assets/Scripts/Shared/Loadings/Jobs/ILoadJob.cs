using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Loadings
{
    public interface ILoadJob
    {
        UniTask Job(IProgress<JobProgress> progressHandle);
    }
}
