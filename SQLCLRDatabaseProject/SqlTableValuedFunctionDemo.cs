using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Telerik.Web.UI;

public partial class UserDefinedFunctions
{
    [SqlFunction(FillRowMethodName = "FillRow",
         TableDefinition = "StartDate DATETIME, EndDate DATETIME")]
    public static IEnumerable ExpandRecurrence(string recurrenceRule, SqlDateTime rangeStart, SqlDateTime rangeEnd)
    {
        //Sample Recurrence Rule
        //DTSTART:20110317T160000Z
        //DTEND:20110317T170000Z
        //RRULE:FREQ=WEEKLY;UNTIL=20120815T000000Z;INTERVAL=3;BYDAY=MO,TH,SA

        //Refer https://www.ietf.org/rfc/rfc2445.txt for more Recurrence rules.

        var occurrences = new List<OccurrenceInfo>();
        RecurrenceRule rrule;
        if (RecurrenceRule.TryParse(recurrenceRule, out rrule))
        {
            rrule.SetEffectiveRange(rangeStart.Value, rangeEnd.Value);
            foreach (DateTime occStart in rrule.Occurrences)
            {
                var info = new OccurrenceInfo(occStart, occStart.Add(rrule.Range.EventDuration));
                occurrences.Add(info);
            }
        }

        return occurrences;
    }
    public static void FillRow(Object obj, out SqlDateTime startDate, out SqlDateTime endDate)
    {
        var info = (OccurrenceInfo)obj;
        startDate = new SqlDateTime(info.Start);
        endDate = new SqlDateTime(info.End);
    }
}

public class OccurrenceInfo
{
    public OccurrenceInfo(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }
    public DateTime Start
    {
        get;
        set;
    }
    public DateTime End
    {
        get;
        set;
    }
}
