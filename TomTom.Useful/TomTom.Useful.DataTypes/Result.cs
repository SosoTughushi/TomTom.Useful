namespace TomTom.Useful.DataTypes
{
    public abstract class Result<TError>
    {
        protected Result()
        {
            this.IsSuccess = true;
        }

        protected Result(TError error)
        {
            Error = error;
            this.IsSuccess = false;
        }

        public bool IsSuccess { get; }

        public TError Error { get; }
    }

    public abstract class Result<T, TError> : Result<TError>
    {
        protected Result(T result) : base()
        {
            this.Value = result;
        }

        public Result(TError error) : base(error)
        {

        }

        public T Value { get; }
    }
}
