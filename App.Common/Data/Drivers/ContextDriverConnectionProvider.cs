using NHibernate.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Common.Data.Drivers
{
    public class ContextDriverConnectionProvider : DriverConnectionProvider
    {
        /// <summary>
        /// Gets a new open <see cref="T:System.Data.IDbConnection" /> through
        /// the <see cref="T:NHibernate.Driver.IDriver" />.
        /// </summary>
        /// <returns>
        /// An Open <see cref="T:System.Data.IDbConnection" />.
        /// </returns>
        public override System.Data.IDbConnection GetConnection()
        {
            var connection = base.GetConnection();

            //BindContext(connection);

            return connection;
        }

        /// <summary>
        /// Binds the tenant ID to CONTEXT_INFO.
        /// </summary>
        /// <param name="connection">The connection.</param>
        //private void BindContext(System.Data.IDbConnection connection)
        //{
        //    if (HttpContext.Current.Session != null)
        //    {
        //        var tenantContext = ServiceLocator.Current.GetInstance<ITenantContext>();
        //        if (!tenantContext.IsSystem)
        //        {
        //            var cmd = connection.CreateCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "SetContextTenantID";

        //            var param = cmd.CreateParameter();
        //            param.ParameterName = "@TenantID";
        //            param.DbType = DbType.Guid;
        //            param.Value = tenantContext.Key;

        //            cmd.Parameters.Add(param);
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}
    }
}
