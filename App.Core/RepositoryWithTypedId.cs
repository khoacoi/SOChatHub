using NHibernate;
using NHibernate.Linq;
using SharpLite.Domain.DataInterfaces;
using SharpLite.NHibernateProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Security.Authentication;

namespace App.Core
{
    public class RepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId> where T : class
    {
        private readonly ISessionFactory _sessionFactory;

        protected virtual ISession Session
        {
            get
            {
                return this._sessionFactory.GetCurrentSession();
            }
        }

        public virtual IDbContext DbContext
        {
            get
            {
                return (IDbContext)new DbContext(this._sessionFactory);
            }
        }

        public RepositoryWithTypedId(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null)
                throw new ArgumentNullException("sessionFactory may not be null");
            this._sessionFactory = sessionFactory;
        }

        public virtual T Get(TId id)
        {
            return this.Session.Get<T>((object)id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return LinqExtensionMethods.Query<T>(this.Session);
        }

        public virtual T SaveOrUpdate(T entity)
        {
            var id = GetEntityId(entity);
            this.Session.SaveOrUpdate((object)entity);

            return entity;
        }

        public virtual void Delete(T entity)
        {
            this.Session.Delete((object)entity);
            this.Session.Flush();
        }

        #region Private Methods

        #region Audit Activities
        //private bool CanLogAuditActivity
        //{
        //    get
        //    {
        //        var configCanAudit = ConfigurationManager.AppSettings["CanLogAuditActivity"];
        //        return !string.IsNullOrEmpty(configCanAudit) && configCanAudit.ToLower() == "true";
        //    }
        //}

        //private void UpdateAuditLogActivity(T entity)
        //{
        //    if (!CanLogAuditActivity) return;
        //    bool isChangedBySiteAdmin = false;
        //    var entityCode = GetEntityCode(entity);
        //    if (string.IsNullOrEmpty(entityCode))
        //        return;
        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            con.Open();
        //            var list = GetChangedProperties(entity);
        //            foreach (var auditInfo in list)
        //            {
        //                if (CurrentUser != null)
        //                {
        //                    var tenantContext = ServiceLocator.Current.GetInstance<ITenantContext>();
        //                    if (!tenantContext.IsSystem)
        //                    {
        //                        using (var cmd = con.CreateCommand())
        //                        {
        //                            cmd.CommandType = CommandType.StoredProcedure;
        //                            cmd.CommandText = "SaveAudit";
        //                            var userID = CurrentUser.UserID;
        //                            if (CurrentUser.Role == ChoiceRole.SiteAdmin)
        //                            {
        //                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = "" });
        //                                cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = userID });
        //                                isChangedBySiteAdmin = true;
        //                            }
        //                            else
        //                            {
        //                                if (CurrentUser.Impersonator != null && (CurrentUser.Impersonator.Role == ChoiceRole.CompanyAdmin || CurrentUser.Impersonator.Role == ChoiceRole.SiteAdmin))
        //                                {
        //                                    if (CurrentUser.Impersonator.Role == ChoiceRole.SiteAdmin)
        //                                        isChangedBySiteAdmin = true;
        //                                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = CurrentUser.Impersonator.UserID });
        //                                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = CurrentUser.UserID });
        //                                }
        //                                else
        //                                {
        //                                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = userID });
        //                                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = "" });
        //                                }
        //                            }
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@IsChangedBySiteAdmin", Value = isChangedBySiteAdmin });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@TenantID", Value = tenantContext.Key.ToString() });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@EntityName", Value = auditInfo.EntityName });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@ActionName", Value = "Update" });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@OldValue", Value = auditInfo.OldData });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@NewValue", Value = auditInfo.NewData });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@FieldName", Value = auditInfo.FieldName });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@Code", Value = GetEntityCode(entity) });
        //                            cmd.ExecuteNonQuery();

        //                        }
        //                    }
        //                }
        //            }
        //            con.Close();
        //            con.Dispose();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void LogAuditSaveActivity(T entity, bool isCreatedNew, Guid id)
        //{
        //    if (!CanLogAuditActivity) return;
        //    var entityCode = GetEntityCode(entity);
        //    if (string.IsNullOrEmpty(entityCode))
        //        return;
        //    if (CurrentUser != null)
        //    {
        //        var isChangedBySiteAdmin = false;
        //        var tenantContext = ServiceLocator.Current.GetInstance<ITenantContext>();
        //        if (!tenantContext.IsSystem)
        //        {
        //            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //            using (var con = new SqlConnection(connectionString))
        //            {
        //                con.Open();
        //                using (var cmd = con.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.CommandText = "SaveAudit";
        //                    var userID = CurrentUser.UserID;

        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@TenantID", Value = tenantContext.Key.ToString() });
        //                    if (CurrentUser.Role == ChoiceRole.SiteAdmin)
        //                    {
        //                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = "" });
        //                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = userID });
        //                        isChangedBySiteAdmin = true;
        //                    }
        //                    else
        //                    {
        //                        if (CurrentUser.Impersonator != null && (CurrentUser.Impersonator.Role == ChoiceRole.CompanyAdmin || CurrentUser.Impersonator.Role == ChoiceRole.SiteAdmin))
        //                        {
        //                            if (CurrentUser.Impersonator.Role == ChoiceRole.SiteAdmin)
        //                                isChangedBySiteAdmin = true;
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = CurrentUser.Impersonator.UserID });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = CurrentUser.UserID });
        //                        }
        //                        else
        //                        {
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = userID });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = "" });
        //                        }
        //                    }
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@IsChangedBySiteAdmin", Value = isChangedBySiteAdmin });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@EntityName", Value = GetEntityName(entity) });
        //                    cmd.Parameters.Add(isCreatedNew
        //                                           ? new SqlParameter { ParameterName = "@ActionName", Value = "Add" }
        //                                           : new SqlParameter { ParameterName = "@ActionName", Value = "Update" });
        //                    cmd.Parameters.Add(isCreatedNew
        //                                           ? new SqlParameter { ParameterName = "@OldValue", Value = string.Empty }
        //                                           : new SqlParameter { ParameterName = "@OldValue", Value = id.ToString() });
        //                    cmd.Parameters.Add(!isCreatedNew
        //                                           ? new SqlParameter { ParameterName = "@NewValue", Value = string.Empty }
        //                                           : new SqlParameter { ParameterName = "@NewValue", Value = entityCode });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Code", Value = entityCode });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@FieldName", Value = entity.GetType().Name + "Code" });
        //                    cmd.ExecuteNonQuery();
        //                }
        //                con.Close();
        //                con.Dispose();
        //            }
        //        }
        //    }
        //}

        //private void LogAuditDeleteActivity(T entity)
        //{
        //    if (!CanLogAuditActivity) return;
        //    var entityCode = GetEntityCode(entity);
        //    if (string.IsNullOrEmpty(entityCode))
        //        return;
        //    if (CurrentUser != null)
        //    {
        //        var tenantContext = ServiceLocator.Current.GetInstance<ITenantContext>();
        //        if (!tenantContext.IsSystem)
        //        {
        //            var isChangedBySiteAdmin = false;
        //            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //            using (var con = new SqlConnection(connectionString))
        //            {
        //                con.Open();
        //                using (SqlCommand cmd = con.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.CommandText = "SaveAudit";
        //                    var userID = CurrentUser.UserID;
        //                    if (CurrentUser.Role == ChoiceRole.SiteAdmin)
        //                    {
        //                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = "" });
        //                        cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = userID });
        //                        isChangedBySiteAdmin = true;
        //                    }
        //                    else
        //                    {
        //                        if (CurrentUser.Impersonator != null && (CurrentUser.Impersonator.Role == ChoiceRole.CompanyAdmin || CurrentUser.Impersonator.Role == ChoiceRole.SiteAdmin))
        //                        {
        //                            if (CurrentUser.Impersonator.Role == ChoiceRole.SiteAdmin)
        //                                isChangedBySiteAdmin = true;
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = CurrentUser.Impersonator.UserID });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = CurrentUser.UserID });
        //                        }
        //                        else
        //                        {
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@ChangeBy", Value = userID });
        //                            cmd.Parameters.Add(new SqlParameter { ParameterName = "@EmployeeId", Value = "" });
        //                        }
        //                    }
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@IsChangedBySiteAdmin", Value = isChangedBySiteAdmin });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@TenantID", Value = tenantContext.Key.ToString() });
        //                    var entityName = GetEntityName(entity);
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@EntityName", Value = entityName });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@ActionName", Value = "Delete" });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@FieldName", Value = entityName + "Code" });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@OldValue", Value = GetEntityCode(entity) });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@Code", Value = GetEntityCode(entity) });
        //                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@NewValue", Value = string.Empty });

        //                    cmd.ExecuteNonQuery();
        //                }
        //                con.Close();
        //                con.Dispose();
        //            }
        //        }
        //    }

        //}

        #endregion Audit Activities

        /// <summary>
        /// Current user logged in to system
        /// </summary>
        private User CurrentUser
        {
            get { return User.Current; }
        }

        /// <summary>
        /// Get Id of Model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Guid</returns>
        private static Guid GetEntityId(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("Id");
            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(entity);
                return propertyValue != null ? (Guid)propertyValue : Guid.Empty;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Get value of propery name is [Entity + Code]
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string GetEntityCode(T entity)
        {
            var type = entity.GetType();
            var propertyInfo = type.GetProperty(type.Name + "Code");            

            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(entity);                
                return propertyValue != null ? propertyValue.ToString() : string.Empty;
            }
            //In case there is no [Entiy + code] column
            var propertyCode = type.GetProperties().FirstOrDefault(e => e.Name.ToLower().Contains("code"));
            if (propertyCode != null)
            {
                var propertyValue = propertyCode.GetValue(entity);
                return propertyValue != null ? propertyValue.ToString() : string.Empty;
            }
            return string.Empty;
        }

        //private static string GetEntityName(T entity)
        //{
        //    var type = entity.GetType();
        //    var planYearInfo = type.GetProperty("PlanYear");
        //    if (planYearInfo != null)
        //    {
        //        var planYearCode = GetPlanYearCode(entity, planYearInfo);

        //        if (!string.IsNullOrEmpty(planYearCode))
        //        {
        //            return string.Format("PlanYear[{0}].{1}", planYearCode, type.Name);
        //        }
        //    }

        //    return type.Name;
        //}

        //private static string GetPlanYearCode(T entity, PropertyInfo planYearInfo)
        //{
        //    if (planYearInfo != null)
        //    {
        //        var planYear = planYearInfo.GetValue(entity);
        //        if (planYear != null)
        //        {
        //            var propertyPlanYearCodeInfo = planYear.GetType().GetProperty("PlanYearCode");
        //            if (propertyPlanYearCodeInfo != null)
        //            {
        //                var propertyPlanYearCode = propertyPlanYearCodeInfo.GetValue(planYear);
        //                return propertyPlanYearCode + string.Empty;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //private List<AuditInfo> GetChangedProperties(T entity)
        //{
        //    var tenantContext = ServiceLocator.Current.GetInstance<ITenantContext>();
        //    if (!tenantContext.IsSystem)
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //        using (var con = new SqlConnection(connectionString))
        //        {
        //            con.Open();
        //            using (var cmd = con.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                var type = entity.GetType();
        //                cmd.CommandText = string.Format("Select * From [{0}] Where {0}Id = '{1}'", type.Name, GetEntityId(entity));
        //                var reader = cmd.ExecuteReader();
        //                var list = new List<AuditInfo>();
        //                if (reader.Read())
        //                {
        //                    foreach (PropertyInfo property in type.GetProperties())
        //                    {
        //                        Type comparable = property.PropertyType.GetInterface("System.IComparable");
        //                        if (comparable != null)
        //                        {
        //                            var value = property.GetValue(entity, null);
        //                            var newPropertyValue = value != null ? value.ToString() : "";                                   
        //                            try
        //                            {
        //                                var originalValue = reader[property.Name] + string.Empty;
        //                                if (property.PropertyType.IsEnum && value != null)
        //                                {
        //                                    newPropertyValue = value + string.Empty;
        //                                    var enumType = property.GetValue(entity, null).GetType();
        //                                    var values = Enum.GetValues(enumType);
        //                                    foreach (var item in values)
        //                                    {
        //                                        if (!string.IsNullOrEmpty(item.ToString()))
        //                                        {
        //                                            if (originalValue == item.GetHashCode().ToString())
        //                                            {    
        //                                                originalValue = item.ToString();
        //                                                break;
        //                                            }                                                    
        //                                        }

        //                                    }
        //                                }

        //                                if (originalValue.ToUpper() != newPropertyValue.ToUpper())
        //                                {
        //                                    list.Add(new AuditInfo
        //                                    {
        //                                        EntityName = GetEntityName(entity),
        //                                        FieldName = property.Name,
        //                                        OldData = originalValue,
        //                                        NewData = newPropertyValue
        //                                    });
        //                                }
        //                            }
        //                            catch (Exception)
        //                            { }
        //                        }
        //                    }
        //                }
        //                return list;
        //            }
        //            con.Close();
        //            con.Dispose();
        //        }
        //    }
        //    return null;
        //}

        #endregion End Private Methods
    }
}
