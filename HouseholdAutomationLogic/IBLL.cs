using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdAutomationLogic
{
    public interface IBLL<T> where T : class
    {
        public IRedactor<T> Redactor { get; }
    }
}
