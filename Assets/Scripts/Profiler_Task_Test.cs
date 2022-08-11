using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;




public class Profiler_Task_Test : MonoBehaviour
{
    List<Task<int>> tasks;
    List<Task<int>> mainTasks;
    int [] result;
    List<int> tasksInput;
    //CustomSampler sampler = CustomSampler.Create("Usless_Task");
    CustomSampler sampler;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        sampler = CustomSampler.Create("Async_Task");
        ThreadCreate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ThreadStart();
        }
    }

    public async void ThreadStart()
    {
        Debug.Log("Starting tasks");
        foreach(Task tasks in mainTasks)
        {
            tasks.Start();
        }
        result = await Task.WhenAll(tasks);
        foreach(int n in result)
        {
            Debug.Log(n);
        }
        Debug.Log("Threads Starting");
    }

    public void ThreadCreate()
    {
        tasks = new List<Task<int>>();
        mainTasks = new List<Task<int>>();
        tasksInput = new List<int>();
        for(int i = 1; i < 61; i++)
        {
            tasksInput.Add(i);
        }
        int taskIndex = 0;
        int threadsNum = 10;
        int continuousTasksPerThread = 6;
        for(int i = 0; i < threadsNum; i++)
        {
            int mainTaskInput = tasksInput[taskIndex];
            Task<int> mainTask = new Task<int>(()=> Usless(mainTaskInput));
            tasks.Add(mainTask);
            mainTasks.Add(mainTask);
            taskIndex++;


            for(int j = 1; j < continuousTasksPerThread; j++)
            {
                int continuousTaskInput = tasksInput[taskIndex];
                Task<int> continuousTask = tasks[taskIndex - 1].ContinueWith(_ =>
                {
                    return Usless(continuousTaskInput);
                }, TaskContinuationOptions.ExecuteSynchronously);
                tasks.Add(continuousTask);
                taskIndex++;
            }

        }
        Debug.Log("Created threads correctly");
    }



  public int Usless(int a)
    {
        //sampler = CustomSampler.Create("Usless_Task");
        Debug.Log(Thread.CurrentThread.ManagedThreadId);
        Profiler.BeginThreadProfiling("My_threads", "My_thread");
        sampler.Begin();
        Thread.Sleep(10);
        int result;
        result = a;
        sampler.End();
        Profiler.EndThreadProfiling();
        return result;
    }
}
