using Expense.Models;

namespace Expense.Contexts
{
    public class VersionContext : IVersionContext
    {
        protected readonly string _version;

        public VersionContext(string version)
        {
            _version = version;
        }
        
        public string GetVersion()
        {
          return _version;
        }
    }
}