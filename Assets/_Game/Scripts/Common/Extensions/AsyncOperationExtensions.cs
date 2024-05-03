using System.Threading.Tasks;
using UnityEngine;

public static class AsyncOperationExtensions
{
    /// <summary>
    /// Extension method that converts an AsyncOperation into a Task.
    /// </summary>
    /// <param name="operation">The AsyncOperation to convert.</param>
    /// <returns>A Task that represents the completion of the AsyncOperation.</returns>
    public static Task ToTask(this AsyncOperation operation)
    {
        var source = new TaskCompletionSource<bool>();
        operation.completed += _ => source.SetResult(true);
        return source.Task;
    }
}