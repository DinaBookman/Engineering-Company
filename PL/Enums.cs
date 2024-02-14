using System;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    // Enumerates EngineerExperience values.
    internal class EngineerExperience : IEnumerable
    {
        // Static collection of EngineerExperience values.
        static readonly IEnumerable<BO.EngineerExperience> s_enums = (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

        // Implementation of IEnumerable.GetEnumerator.
        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }

    // Enumerates Status values.
    internal class Status : IEnumerable
    {
        // Static collection of Status values.
        static readonly IEnumerable<BO.Status> s_enums = (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;

        // Implementation of IEnumerable.GetEnumerator.
        public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
    }
}