namespace Domain.Core
{
    public class Result<T> : IResult<T>
    {
        #region Constructors
        public Result()
        {
        }

        public Result(T message)
        {
            ((IResult<T>)this).Message = message;
        }
        #endregion

        #region IResult Implementation
        bool IResult.IsSuccessful => ((IResult<T>)this).Errors.Count == 0;

        List<string> IResult.Errors { get; } = new List<string>();

        void IResult.AddError(string error)
        {
            ((IResult<T>)this).Errors.Add(error);
        }

        void IResult.AddError(List<string> errors)
        {
            ((IResult<T>)this).Errors.AddRange(errors);
        }
        #endregion

        #region IResult<T> Implementation
        T IResult<T>.Message { get; set; }
        #endregion
    }
}
