using System.Collections;
using System.Threading;

public class ThreadedJob
{
    private bool isDone = false;
    private object handle = new object();
    private Thread thread = null;

    public bool IsDone 
    { 
        get 
        { 
            bool tmp; 
            lock (handle) 
            {
                tmp = isDone; 
            }
            return tmp; 
        }
        set 
        {
            lock (handle) 
            {
                isDone = value;
            }
        }
    }

    public virtual void Start()
    {
        thread = new Thread(Run);
        thread.Start();
    }

    public virtual void Abort()
    {
        thread.Abort();
    }

    protected virtual void ThreadFunction()
    { }

    protected virtual void OnFinished() 
    { }

    public virtual bool Update()
    {
        if (isDone)
        {
            OnFinished();
            return true;
        }

        return false;
    }

    public IEnumerator WaitFor()
    {
        while (!Update())
        {
            yield return null;
        }
    }

    public void Run()
    {
        ThreadFunction();
        isDone = true;
    }
}
