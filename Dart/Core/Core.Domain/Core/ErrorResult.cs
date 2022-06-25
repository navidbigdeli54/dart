namespace Domain.Core
{
    public class ErrorResult<T> : IResult<T>
    {
        #region Constructors
        public ErrorResult()
        {
        }

        public ErrorResult(string error)
        {
            ((IResult)this).Errors.Add(error);
        }

        public ErrorResult(IList<string> errors)
        {
            ((IResult)this).Errors.AddRange(errors);
        }

        public ErrorResult(string error, T message) : this(error)
        {
            ((IResult<T>)this).Message = message;
        }
        #endregion

        #region IResult Implementation
        bool IResult.IsSuccessful => false;

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
    }
    #endregion
}
