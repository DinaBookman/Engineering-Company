using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    sealed internal class DalList : IDal
    {
        /// <summary>
        /// creates a lazy singleton to avoid wasting system resources when the object is not used.
        /// the true parameter causes the initialization to be Thread Safe.
        /// </summary>
        public static IDal Instance { get; } = new Lazy<DalList>(true).Value;
        private DalList() { }

        public IEngineer Engineer => new EngineerImplementation();

        public ITask Task => new TaskImplementation();

        public IDependency Dependency => new DependencyImplementation();
    }
}