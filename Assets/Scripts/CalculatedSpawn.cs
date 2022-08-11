using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class CalculatedSpawn : MonoBehaviour
{
    public GameObject ballSpawnee;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("BallSpawn", spawnTime, spawnDelay);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BallSpawn();
        }
    }

    public void BallSpawn()
    {
        int n = 100; // matrix NxN

        int r1 = n; //number of rows in matrix a
        int c1 = n; //number of columns in matrix a
        int r2 = n; //number of rows in matrix b
        int c2 = n; //number of columns in matrix b

        //int[,] matrixA = { { 10, 5 }, { 50, 75 } };
        //int[,] matrixB = { { 75, 5 }, { 10, 50 } };
        int[,] matrixA = new int[n,n];
        int[,] matrixB = new int[n,n];

        Debug.Log("MatrixA");
        StringBuilder sbA = new StringBuilder();
        for (int rows = 0; rows < r1; rows++)
        {

            for (int cols = 0; cols < c1; cols++)
            {
                matrixA[rows, cols] = Random.Range(0, 100);
                sbA.Append(matrixA[rows, cols]);
                sbA.Append(' ');
            }
            sbA.AppendLine();
        }
        Debug.Log(sbA.ToString());

        Debug.Log("MatrixB");
        StringBuilder sbB = new StringBuilder();
        for (int rows = 0; rows < r2; rows++)
        {

            for (int cols = 0; cols < c2; cols++)
            {
                matrixB[rows, cols] = Random.Range(0, 100);
                sbB.Append(matrixB[rows, cols]);
                sbB.Append(' ');
            }
            sbB.AppendLine();
        }
        Debug.Log(sbB.ToString());


        
        CalculateForSpawnTasks(matrixA, matrixB, r1, c1, r2, c2, n);
        //CalculateForSpawn(matrixA, matrixB, r1, c1, r2, c2, n);
        Instantiate(ballSpawnee, transform.position, transform.rotation);
        if(stopSpawning)
        {
            CancelInvoke("BallSpawn");
        }
    }


    public async void CalculateForSpawnTasks(int[,] matrixA, int[,] matrixB, int r1, int c1, int r2, int c2, int n)
    {
        float matrixDivide;
        int halfMatrix;

        matrixDivide = (n / 2);

        halfMatrix = (int)matrixDivide;

        int[,] resultMatrix = new int[n, n];
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        resultMatrix = await Task.Factory.StartNew(() =>
        {
            Profiler.BeginThreadProfiling("My threads", "My thread foo");
            Thread.Sleep(1000);
            StringBuilder sb = new StringBuilder();

            Debug.Log("Result Matrix");
            for (int rows = 0; rows < r1; rows++)
            {
                for (int cols = 0; cols < c2; cols++)
                {
                    for (int pos = 0; pos < r2; pos++)
                    {
                        resultMatrix[rows, cols] += matrixA[rows, pos] * matrixB[pos, cols];
                    }
                    sb.Append(resultMatrix[rows, cols]);
                    sb.Append(' ');
                }
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
            Profiler.EndThreadProfiling();
            return resultMatrix;
        });
        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Debug.Log("RunTime " + elapsedTime);




    }

    public void CalculateForSpawn(int[,] matrixA, int[,] matrixB, int r1, int c1, int r2, int c2, int n)
    {
        int[,] resultMatrix = new int[n, n];
        StringBuilder sb = new StringBuilder();

        Debug.Log("Result Matrix");
        for (int rows = 0; rows < r1; rows++)
        {
            for (int cols = 0; cols < c2; cols++)
            {
                for (int pos = 0; pos < r2; pos++)
                {
                    resultMatrix[rows, cols] += matrixA[rows, pos] * matrixB[pos, cols];
                }
                sb.Append(resultMatrix[rows, cols]);
                sb.Append(' ');
            }
            sb.AppendLine();
        } 
        Debug.Log(sb.ToString());
    }


}
