using System;
using System.Runtime.CompilerServices;
using UnityEngine.ResourceManagement;
using Object = UnityEngine.Object;

public static class IAsyncOperationExtensions
{
    public static AsyncOperationAwaiter GetAwaiter(this IAsyncOperation operation)
    {
        return new AsyncOperationAwaiter(operation);
    }

    public static AsyncOperationAwaiter<T> GetAwaiter<T>(this IAsyncOperation<T> operation) where T : Object
    {
        return new AsyncOperationAwaiter<T>(operation);
    }

    public struct AsyncOperationAwaiter : INotifyCompletion
    {
        private readonly IAsyncOperation _operation;

        public AsyncOperationAwaiter(IAsyncOperation operation)
        {
            _operation = operation;
        }

        public bool IsCompleted => _operation.Status != AsyncOperationStatus.None;

        public void OnCompleted(Action continuation) => _operation.Completed += (op) => continuation?.Invoke();

        public object GetResult() => _operation.Result;
    }

    public struct AsyncOperationAwaiter<T> : INotifyCompletion where T : Object
    {
        private readonly IAsyncOperation<T> _operation;

        public AsyncOperationAwaiter(IAsyncOperation<T> operation)
        {
            _operation = operation;
        }

        public bool IsCompleted => _operation.Status != AsyncOperationStatus.None;

        public void OnCompleted(Action continuation) => _operation.Completed += (op) => continuation?.Invoke();

        public T GetResult() => _operation.Result;
    }
}