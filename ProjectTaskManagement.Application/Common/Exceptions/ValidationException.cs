using ProjectTaskManagement.Application.Common.Behaviours;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTaskManagement.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
      : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationEroor> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.Name, e => e.Message)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
