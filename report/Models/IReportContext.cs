using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Models
{
    public interface IReportContext
    {
         Task<ReportTotal> GetReportTotal(string tripId);
    }
}