using System;
using System.Threading;

namespace MyClassLib
{
  public class Helper
  {
    public static void DoProcessing(IProgress<int> progress, CancellationToken token)
    {
      for (int i = 0; i != 100; ++i)
      {
        Thread.Sleep(100); // CPU-bound work
        if (progress != null)
          progress.Report(i);

        if (token.IsCancellationRequested)
          break;
      }
    }
  }
}
