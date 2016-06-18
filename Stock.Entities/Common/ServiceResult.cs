using System.Collections.Generic;
using System.Linq;

namespace Stock.Entities.Common
{
    /// <summary>
    /// Wrapper for responses from Business Layer
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Determines if request succeeded
        /// </summary>
        public virtual bool Succeeded
        {
            get
            {
                return _succeeded;
            }
        }

        /// <summary>
        /// List of errors if any
        /// </summary>
        public IEnumerable<string> Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                if (_errors != null && _errors.Any())
                {
                    _succeeded = false;
                }
            }
        }

        public ServiceResult()
        {
        }

        public ServiceResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public ServiceResult(string error)
            : this(new[] { error })
        {
        }

        private bool _succeeded = true;
        private IEnumerable<string> _errors;
    }

    /// <summary>
    /// Generic Wrapper for responses from Business Layer
    /// </summary>
    /// <typeparam name="T">Type of expected result</typeparam>
    public class ServiceResult<T> : ServiceResult where T : class
    {
        public ServiceResult() { }

        public ServiceResult(T result)
        {
            Result = result;
        }

        public ServiceResult(IEnumerable<string> errors) : base(errors) { }

        public ServiceResult(string error) : base(error) { }

        /// <summary>
        /// Expected Result
        /// </summary>
        public T Result { get; set; }

        public override bool Succeeded
        {
            get
            {
                return Result != null && base.Succeeded;
            }
        }
    }
}
