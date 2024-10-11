namespace WindowsApp;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        Log("App loaded");
        PrintCurrentThreadId();
    }

    private void PrintCurrentThreadId()
    {
        Log("ManagedThreadId: {0}", Environment.CurrentManagedThreadId);
    }

    private void Log(string logMessage, params object[] parameters)
    {
        InvokeIfRequired(lbLogs, delegate
        {
            var message = $"[{DateTime.Now:HH:mm:ss.fff}] - {string.Format(logMessage, parameters)}";
            lbLogs.Items.Add(message);
            lbLogs.SelectedIndex = lbLogs.Items.Count - 1;
        });
    }

    private void btnManagedThreadId_Click(object sender, EventArgs e)
    {
        PrintCurrentThreadId();
    }

    private void btnStartTimer_Click(object sender, EventArgs e)
    {
        //System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        //t.Tick += (sender, e) => { lblClock.Text = DateTime.Now.ToString("HH:mm:ss.fff"); };
        //t.Interval = 10;
        //t.Start();

        var timer = new System.Timers.Timer(10);
        timer.Elapsed += ((_, _) =>
        {
            InvokeIfRequired(lblClock, delegate
            {
                lblClock.Text = DateTime.Now.ToString("HH:mm:ss.fff");
            });
        });

        timer.Start();
    }

    private static void InvokeIfRequired(Control control, Action action)
    {
        if (control.InvokeRequired)
        {
            control.Invoke(action);
            return;
        }

        action();
    }

    private void btnFakeApi_Click(object sender, EventArgs e)
    {
        Log("Api Call Started");
        var randomWaitTime = Random.Shared.Next(1000, 5000);
        //await Task.Delay(randomWaitTime);

        var syncContext = SynchronizationContext.Current;

        Task.Run(() =>
        {
            var randomWaitTime = Random.Shared.Next(1000, 5000);

            Task.Delay(randomWaitTime).Wait();

            syncContext.Post(_ =>
            {
                Log("Api Call Finished");
            }, null);


            Log("Api Call Finished");
        });
    }

    private async void btnWait_Click(object sender, EventArgs e)
    {
        //using ManualResetEventSlim mre = new();


        await Task.Run(() =>
        {
            Thread.Sleep(3000);
            //await Task.Delay(5000);
            Log("Completed");
        });

        //mre.Wait();
    }

    private void btnSet_Click(object sender, EventArgs e)
    {
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        button1.Text = "Started";
        PrintCurrentThreadId();

        await Task.Delay(3000).ConfigureAwait(false);

        PrintCurrentThreadId();

        //button1.Text = "Finished";

        InvokeIfRequired(button1, () =>
        {
            button1.Text = "Finished";
        });
    }
}



