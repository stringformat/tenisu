namespace Tenisu.Common;

public record Result
{
    public Error Error { get; }
    public bool IsSuccess => !IsFail;
    public bool IsFail { get; }

    protected Result(Error error)
    {
        Error = error;
        IsFail = true;
    }
    
    protected Result()
    {
        Error = default!;
        IsFail = false;
    }

    public static Result Success => new();
    
    public static Result Failure => new(default!);

    public static implicit operator Result(Error error) => new(error);

    public void Deconstruct(
        out bool isFailed,
        out Error error
    ) => (isFailed, error) = (IsFail, Error);
}

public record Result<TResult> : Result
{
    public TResult Value { get; }

    private Result(Error error)
        : base(error)
    {
        Value = default!;
    }

    private Result(TResult value)
    {
        Value = value;
    }

    public new static Result<TResult> Failure => new(default(Error)!);

    public static Result<TResult> Success(TResult result) => new(result);

    public bool TryGet(out TResult value)
    {
        value = Value;
        return IsSuccess;
    }

    public static implicit operator Result<TResult>(TResult value) => new(value);
    
    public static implicit operator TResult(Result<TResult> value) => value.Value;
    
    public static implicit operator Result<TResult>(Error error) => new(error);

    public void Deconstruct(
        out bool isSuccess,
        out TResult value
    ) => (isSuccess, value) = (IsSuccess, Value);
    
    public void Deconstruct(
        out bool isSuccess,
        out TResult value,
        out Error error
    ) => (isSuccess, value, error) = (IsSuccess, Value, Error);
}