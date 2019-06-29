using MyClassLib;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressBar
{

  //https://blog.stephencleary.com/2012/02/reporting-progress-from-async-tasks.html
  public partial class Form1 : Form
  {
    private int[] progressArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    CancellationTokenSource cts = new CancellationTokenSource();

    public Form1()
    {
      InitializeComponent();
    }

    private async void Button1_Click(object sender, EventArgs e)
    {
      if (cts.Token.IsCancellationRequested)
      {
        cts.Dispose();
        cts = new CancellationTokenSource();
      }

      progressBar1.Minimum = 0;
      progressBar1.Maximum = 100;
      progressBar1.Value = 0;
      progressBar1.Step = 1;

      // The Progress<T> constructor captures our UI context,
      //  so the lambda will be run on the UI thread.
      var progress = new Progress<int>(percent =>
      {
        textBox1.Text = percent + "%";
        progressBar1.PerformStep();
      });

      //DoProcessing is run on the thread pool.
      await Task.Run(() => Helper.DoProcessing(progress, cts.Token));
      if (cts.Token.IsCancellationRequested)
        textBox1.Text = "Stopped";
      else
        textBox1.Text = "Done!";
    }

    private void button2_Click(object sender, EventArgs e)
    {
      cts.Cancel();
      MessageBox.Show("Operation cancelled");
    }
  }
}
