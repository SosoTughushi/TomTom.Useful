using System;
using System.Collections.Generic;
using System.Text;

namespace TomTom.Useful.DataTypes.Tests.Result
{
    public enum ErrorTypes1
    {
        Error,
        SecondError,
        ThirdError,
        FourthError,
        FifthError
    }

    public enum ErrorTypes2
    {
        Error,
        SecondError,
        ThirdError,
        FourthError,
        FifthError
    }

    public enum ErrorTypes3
    {
        Error,
        SecondError,
        ThirdError,
        FourthError,
        FifthError
    }

    public class ComplexError
    {
        public ComplexError(ErrorTypes3 errorType, string text)
        {
            ErrorType = errorType;
            Text = text;
        }

        public ErrorTypes3 ErrorType { get; }
        public string Text { get; }
    }

    public class ErrorWithException
    {
        public ErrorWithException(string error, Exception ex)
        {
            Error = error;
            Ex = ex;
        }

        public string Error { get; }
        public Exception Ex { get; }
    }

    public class NestedError 
    {
        private readonly string text;
        private readonly NestedError innerError;

        public NestedError(string text, NestedError innerError = null)
        {
            this.text = text;
            this.innerError = innerError;
        }
    }
}
