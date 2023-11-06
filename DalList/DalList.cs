using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    sealed public class DalList : IDal
    {
        public IEngineer Engineer => new EngineerImplementation();

        public ITask Task => new TaskImplementation();

        public IDependency Dependency => new DependencyImplementation();
    }
}