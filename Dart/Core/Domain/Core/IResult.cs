namespace Domain.Core
{
    public interface IResult
    {
        #region Fields
        bool IsSuccessful { get; }

        List<string> Errors { get; }
        #endregion

        #region Public Methods
        void AddError(string error);

        void AddError(List<string> errors);
        #endregion
    }

    public interface IResult<T> : IResult
    {
        #region Fields
        T Message { get; set; }
        #endregion
    }
}
