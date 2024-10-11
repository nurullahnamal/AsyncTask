

using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

//Example_1();


//Console.ReadLine();

//void PrintCurrentThreadId()
//{
//    Console.WriteLine("ManagedThreadId: {0}", Environment.CurrentManagedThreadId);
//}

//async Task Example_ThreadId()
//{
//    PrintCurrentThreadId();

//    await Task.Delay(3000).ConfigureAwait(false);

//    PrintCurrentThreadId();
//}

void Example_1()
{
    List<Task> tasks = new();
    AsyncLocal<int> local = new();
    for (int i = 0; i < 10; i++)
    {
        local.Value = i;
        var t = Task.Run(() =>
        {
            Console.WriteLine("Value: {0}", local.Value);
            Task.Delay(1000).Wait();
        });

        tasks.Add(t);
    }

    Task.WhenAll(tasks).Wait();
}

void Example_2()
{
    void Print(int val)
    {
        Console.WriteLine(val);
        var randomValue = Random.Shared.Next(500, 1500);
        Task.Delay(randomValue).Wait();
    }

    Task.Run(() =>
    {
        Thread.Sleep(1000);
        Console.WriteLine("Main tasks completed");
    })
    .ContinueWith((_) => Print(1))
    .ContinueWith((_) => Print(2))
    .ContinueWith((_) => Print(3))
    .ContinueWith((_) => Print(4));
}

async void Example_3()
{
    var task = Task.Run(() =>
    {
        Task.Delay(1000).Wait();
    });

    await task;

    _ = task.ContinueWith((_) =>
    {
        Console.WriteLine("Continued with...");
    });



}

async Task Example_4()
{
    void PrintLog(string logMessage) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] - {logMessage}");

    var task = Task.Run(() =>
    {
        Task.Delay(1000).Wait();
    })
    .ContinueWith((_) =>
    {
        return Task.Delay(2000);
    });

    PrintLog("Before");
    var ct = task.Result;

    await task;
    await ct;
    PrintLog("After");
}

//var tt = new MyTask();
//try
//{
//    tt.SetAction(() => { });
//    tt.SetResult();
//}
//catch (Exception ex)
//{
//    tt.SetException(ex);
//}

//tt.Wait();

//var t = Task.Run(() => { Console.WriteLine(""); });
//t.ContinueWith(t => Console.WriteLine("bir şey"));
//t.Wait();

//var ttt = Task.Run(() => { });
//ttt.Wait();
//ttt.ContinueWith(() => { });


//using ManualResetEventSlim mre = new();


//var t = Task.Run(() =>
//{
//    Thread.Sleep(3000);
//    //await Task.Delay(5000);
//    //Log("Completed");
//    mre.Set();
//});

//Console.WriteLine("BEFORE");
//mre.Wait();
//Console.WriteLine("AFTER");

////Console.WriteLine("Bir şey");


//var t = MyTask.Run(() =>
//{
//    Thread.Sleep(3000);
//    Console.WriteLine("bekledim");
//});

//Console.WriteLine("Before");
//t.Wait();
//t.Wait();
//Console.WriteLine("AFTER");

//var tasks = new List<MyTask>();

//tasks.Add(MyTask.Run(() => { Thread.Sleep(3000); }));
//tasks.Add(MyTask.Run(() => { Thread.Sleep(10000); }));
//tasks.Add(MyTask.Run(() => { Thread.Sleep(12500); }));

//await foreach (var item in MyTask.WhenEach(tasks))
//{
//    Console.WriteLine("Task tamamlandi");
//}

//Console.WriteLine("BEFORE");
//t.Wait();
//Console.WriteLine("AFTER");

//var t = MyTask.Delay(3000);

//Console.WriteLine("BEFORE");
//t.Wait();
//Console.WriteLine("AFTER");

//Console.ReadLine();

_ = MyTask.Run(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("Main method");
})
.ContinueWith(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("İlk Continue With");
})
.ContinueWith(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("İkinci Continue With");
})
.ContinueWith(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("Üçüncü Continue With");
})
.ContinueWith(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("Son Continue With");
});


var myt = Task.Run(() =>
{

});

var ct = myt.ContinueWith(async (t) =>
{
    await Task.Delay(1000);
});



ct.Wait();

var t = MyTask.FromResult<int>(10);

var tt = GetAnotherTask();
await tt;

async Task GetAnotherTask()
{
    try
    {
        await GetTask();

        await Task.Delay(1000);
    }
    catch (Exception ex)
    {
        // 
    }
}


Task GetTask()
{
    return Task.Delay(1000);
}



Console.ReadLine();


class MyTask<TResult> : MyTask
{
    public TResult? Result { get; private set; }

    public void SetResult(TResult result)
    {
        Result = result;
        base.SetResult();
    }



    public new Awaiter GetAwaiter() => new(this);

    public new struct Awaiter(MyTask<TResult> t) : INotifyCompletion
    {
        public readonly bool IsCompleted => t.IsCompleted;

        public readonly TResult GetResult()
        {
            t.Wait();
            return t.Result!;
        }

        public readonly void OnCompleted(Action continuation) => t.ContinueWith(continuation);
    }
}


class MyTask
{
    private bool completed;
    private Exception? exception;
    private Action? continuation;
    private ExecutionContext? context;
    private object lockObject = new();

    public bool IsCompleted => completed;

    public Exception? Exception => exception;

    public static MyTask CompletedTask
    {
        get
        {
            MyTask t = new();
            t.SetResult();
            return t;
        }
    }

    public static MyTask<T> FromResult<T>(T resultValue)
    {
        MyTask<T> t = new();
        t.SetResult(resultValue);
        return t;
    }


    public struct Awaiter(MyTask t) : INotifyCompletion
    {
        public Awaiter GetAwaiter() => this;
        public bool IsCompleted => t.IsCompleted;
        public void OnCompleted(Action continuation) => t.ContinueWith(continuation);
        public void GetResult() => t.Wait();
    }

    public Awaiter GetAwaiter() => new(this);


    public void SetResult() => Complete(null);

    public void SetException(Exception ex) => Complete(ex);



    private void Complete(Exception? exception)
    {
        lock (lockObject)
        {
            if (completed)
                return;

            completed = true;
            this.exception = exception;

            if (continuation is not null)
            {
                MyThreadPool.QueueUserWorkItem(() =>
                {
                    if (context is null)
                    {
                        continuation();
                    }
                    else
                    {
                        ExecutionContext.Run(context, (object? state) => ((Action)state!).Invoke(), continuation);
                    }
                });
            }
        }
    }

    public void Wait()
    {
        ManualResetEventSlim? mre = null;

        if (!completed)
        {
            mre = new();
            // bekle, benim Set metodumu çağır
            ContinueWith(() => mre.Set());
        }

        mre?.Wait();

        if (exception is not null)
        {
            //throw new AggregateException(exception);
            ExceptionDispatchInfo.Throw(exception);
        }
    }


    private MyTask<TResult> ContinueWithInternal<TResult>(Delegate action, bool isTaskFunc, bool isGenericTaskFunc)
    {
        MyTask<TResult> t = new();

        void callback()
        {
            try
            {
                if (isTaskFunc)
                {
                    if (isGenericTaskFunc && action is Func<MyTask> taskFunc)
                    {
                        MyTask next = taskFunc();
                        next.ContinueWith(() =>
                        {
                            if (next.exception is not null)
                            {
                                t.SetException(next.exception);
                            }
                            else
                            {
                                t.SetResult();
                            }
                        });
                    }
                    else if (!isGenericTaskFunc && action is Func<MyTask, TResult> genericFunc)
                    {
                        TResult result = genericFunc(this);
                        t.SetResult(result);
                    }
                }
                else if (action is Action<MyTask> taskAction)
                {
                    taskAction(this);
                    t.SetResult();
                }
                else if (action is Action simpleAction)
                {
                    simpleAction();
                    t.SetResult();
                }
            }
            catch (Exception e)
            {
                t.SetException(e);
            }
        }

        lock (lockObject)
        {
            if (IsCompleted)
            {
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                continuation = callback;
                context = ExecutionContext.Capture();
            }
        }

        return t;
    }



    public MyTask ContinueWith(Action<MyTask> action)
    {
        return ContinueWithInternal<object>(action, false, false);

        MyTask t = new();

        void callback()
        {
            try
            {
                action(this);
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }

            t.SetResult();
        }

        lock (lockObject)
        {
            if (completed)
            {
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                continuation = callback;
                context = ExecutionContext.Capture();
            }
        }

        return t;
    }

    public MyTask ContinueWith(Action action)
    {
        return ContinueWithInternal<object>(action, false, false);

        MyTask t = new();

        void callback()
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }

            t.SetResult();
        };

        lock (lockObject)
        {
            if (completed)
            {
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                continuation = callback;
                context = ExecutionContext.Capture();
            }
        }

        return t;
    }


    public MyTask ContinueWith(Func<MyTask> action)
    {
        return ContinueWithInternal<object>(action, true, true);

        MyTask t = new();

        void callback()
        {
            try
            {
                MyTask next = action();

                next.ContinueWith(() =>
                {
                    if (next.exception is not null)
                    {
                        t.SetException(next.exception);
                    }
                    else
                    {
                        t.SetResult();
                    }
                });

            }
            catch (Exception e)
            {
                t.SetException(e);
                return;
            }
        }

        lock (lockObject)
        {
            if (completed)
            {
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                continuation = callback;
                context = ExecutionContext.Capture();
            }
        }

        return t;
    }

    public MyTask<TResult> ContinueWith<TResult>(Func<MyTask, TResult> action)
    {
        return ContinueWithInternal<TResult>(action, true, false);

        MyTask<TResult> t = new();

        void callback()
        {
            try
            {
                TResult result = action(this);
                t.SetResult(result);
            }
            catch (Exception e)
            {
                t.SetException(e);
            }
        }

        lock (lockObject)
        {
            if (IsCompleted)
            {
                MyThreadPool.QueueUserWorkItem(callback);
            }
            else
            {
                continuation = callback;
                context = ExecutionContext.Capture();
            }
        }

        return t;
    }


    #region Helpers

    public static MyTask Run(Action action)
    {
        var t = new MyTask();

        MyThreadPool.QueueUserWorkItem(delegate
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                t.SetException(ex);
                return;
            }

            t.SetResult();
        });

        return t;
    }

    public static MyTask Run(Action<MyTask> action)
    {
        var t = new MyTask();

        MyThreadPool.QueueUserWorkItem(delegate
        {
            try
            {
                action(t);
            }
            catch (Exception ex)
            {
                t.SetException(ex);
                return;
            }

            t.SetResult();
        });

        return t;
    }

    public static MyTask<TResult> Run<TResult>(Func<TResult> func)
    {
        var t = new MyTask<TResult>();

        MyThreadPool.QueueUserWorkItem(() =>
        {
            try
            {
                var result = func();
                t.SetResult(result);
            }
            catch (Exception ex)
            {
                t.SetException(ex);
            }
        });

        return t;
    }

    public static MyTask WhenAll(List<MyTask> tasks)
    {
        MyTask t = new();

        var remainingCount = tasks.Count;

        if (remainingCount == 0)
        {
            t.SetResult();
            return t;
        }


        foreach (var task in tasks)
        {
            task.ContinueWith(() =>
            {
                if (Interlocked.Decrement(ref remainingCount) == 0)
                {
                    t.SetResult();
                }
            });
        }

        return t;
    }

    public static MyTask WhenAny(List<MyTask> tasks)
    {
        MyTask t = new();

        if (tasks.Count == 0)
        {
            t.SetResult();
            return t;
        }

        foreach (var task in tasks)
        {
            if (task.completed)
            {
                t.SetResult();
                break;
            }

            task.ContinueWith(() =>
            {
                t.SetResult();
            });
        }

        return t;
    }

    public static MyTask Delay(int value)
    {
        MyTask t = new();

        var timer = new Timer((_) =>
        {
            t.SetResult();
        });

        timer.Change(value, Timeout.Infinite);

        return t;
    }


    public static async IAsyncEnumerable<MyTask> WhenEach(List<MyTask> tasks)
    {
        if (tasks == null || tasks.Count == 0)
            yield break;

        var remainingTasks = new ConcurrentBag<MyTask>(tasks);
        var taskCompletionSource = new TaskCompletionSource<MyTask>();

        foreach (var task in remainingTasks)
        {
            _ = task.ContinueWith(() =>
            {
                if (remainingTasks.TryTake(out _))
                {
                    taskCompletionSource.TrySetResult(task);
                }
            });
        }

        while (!remainingTasks.IsEmpty)
        {
            var completedTask = await taskCompletionSource.Task;

            yield return completedTask;

            if (!remainingTasks.IsEmpty)
                taskCompletionSource = new TaskCompletionSource<MyTask>();
        }
    }

    #endregion
}




class MyThreadPool
{
    private static readonly
        BlockingCollection<(Action, ExecutionContext?)> actions = [];

    public static void QueueUserWorkItem(Action action)
        => actions.Add((action, ExecutionContext.Capture()));

    static MyThreadPool()
    {
        for (int i = 0; i < Environment.ProcessorCount; i++)
        {
            var t = new Thread(() =>
            {
                while (true)
                {
                    var (action, context) = actions.Take();

                    if (context is null)
                    {
                        action();
                    }
                    else
                    {
                        ExecutionContext.
                            Run(context, (object? state) => ((Action)state!).Invoke(), action);
                    }
                }
            });

            t.IsBackground = true;
            t.Start();
        }
    }



}



