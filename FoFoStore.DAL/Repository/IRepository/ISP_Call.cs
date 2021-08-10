using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoFoStore.DAL.Repository
{
   public interface ISP_Call :IDisposable
    {
        //Scaler return integer  or bool value
        T Single<T>(string procedureName, DynamicParameters param = null);

        void Execute(string procedureName, DynamicParameters param = null);

        //complete record

        T OneRecord<T>(string procedureName, DynamicParameters param = null);

        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);

        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null);
    }
}
