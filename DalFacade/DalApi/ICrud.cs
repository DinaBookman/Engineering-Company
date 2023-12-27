using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DalApi
{
    public interface ICrud<T> where T : class
    {
        int Create(T item); //Creates new entity object in DAL
        T? Read(int id); //Reads entity object by its ID 
        T? Read(Func<T, bool> filter); // stage 2 Reads entity object by one attribute.
        IEnumerable<T?>? ReadAll(Func<T, bool>? filter = null); //stage 2, Reads all entity objects by filter.
        void Update(T item); //Updates entity object
        void Delete(int id); //Deletes an object by its Id
    }
}