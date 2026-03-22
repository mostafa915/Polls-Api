namespace SurveyBasket.Abstractions
{
    public class Result
    {
        public Result(bool isSucess, Erorr _erorr)
        {
            if((isSucess &&  _erorr != Erorr.None) || (!isSucess && _erorr == Erorr.None))
            {
                throw new InvalidOperationException();
            }
            IsSucess = isSucess;
            erorr = _erorr;
        }
        public bool IsSucess { get; }
        public bool IsFaliuer => !IsSucess;
        public Erorr erorr { get; } = default!;
        public static Result Succuess() => new (true, Erorr.None);
        public static Result Faliuer(Erorr erorr) => new Result(false, erorr);
        public static Result<TValue> Succuess<TValue>(TValue value) => new(value, true, Erorr.None);
        public static Result<TValue> Faliuer<TValue>(Erorr erorr) => new(default!, false, erorr);
    }
    public class Result<TValue> : Result { 
        private readonly TValue? _value;
        public Result(TValue value, bool isSuccess, Erorr erorr) : base(isSuccess, erorr) 
        {
            _value = value;
        }
        public TValue Value => IsSucess ? _value! : throw new InvalidOperationException("Faliuer Result Can not Have Value");

    }
}
