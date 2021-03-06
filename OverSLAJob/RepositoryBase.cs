﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OverSLAJob
{
    public class RepositoryBase
    {
        #region Aeh

        readonly protected string _connString = System.Configuration.ConfigurationSettings.AppSettings["DefaultConnection"];

        public enum ParamStyle
        {
            Undefined,
            General,
            LikeBeginWith,
            LikeEndWith,
            LikeContain,
            LikeCustom,
        }

        public CRMORIGINEntities GetDBContext()
        {
            var ctx = new CRMORIGINEntities();
            ctx.Configuration.AutoDetectChangesEnabled = true;
            ctx.Configuration.ValidateOnSaveEnabled = false;
            // added for manage execute timeout in 3 minutes
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)ctx).ObjectContext.CommandTimeout = 180;
            return ctx;
        }

        public List<T> ExecStoredProcedure<T>(string strName, params ObjectParameter[] objParams)
        {
            using(var db = this.GetDBContext())
            {
                var objPrepare = new List<ObjectParameter>();

                foreach (var p in objParams)
                {
                    if (p != null)
                    {
                        objPrepare.Add(p);
                    }
                }
                                                
                return ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext.ExecuteFunction<T>(strName, objPrepare.ToArray()).ToList();

            }
        }

        public int ExecStoredProcedureNoReturn(string strName, params ObjectParameter[] objParams) 
        { 
            using(var db = this.GetDBContext())
            {
                var objPrepare = new List<ObjectParameter>();

                foreach (var p in objParams)
                {
                    if (p != null)
                    {
                        objPrepare.Add(p);
                    }
                }

                return ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db).ObjectContext.ExecuteFunction(strName, objPrepare.ToArray());
            }
        }

        public ObjectParameter GenObjectParam(string name, object value, ParamStyle style = ParamStyle.General)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
           
            object valuePrepare = value;

            switch (style)
            {
                case ParamStyle.Undefined:
                    break;
                case ParamStyle.General:
                    if (valuePrepare is string)
                    {
                        if (valuePrepare.Equals(string.Empty))
                        {
                            valuePrepare = null;
                        }
                    }
                    break;
                case ParamStyle.LikeBeginWith:
                    valuePrepare = this.PrepareSearchParam(valuePrepare) + "%";
                    break;
                case ParamStyle.LikeEndWith:
                    valuePrepare = "%" + this.PrepareSearchParam(valuePrepare);
                    break;
                case ParamStyle.LikeContain:
                    valuePrepare = "%" + this.PrepareSearchParam(valuePrepare) + "%";
                    break;
                case ParamStyle.LikeCustom:
                    valuePrepare = this.PrepareSearchParam(valuePrepare).Replace("*", "%");
                    break;
                default:
                    break;
            }

            if (valuePrepare == null)
            {
                return null;
            }

            return new ObjectParameter(name, valuePrepare);
        }

        public object GenObjectParam(object value, ParamStyle style = ParamStyle.General)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }

            object valuePrepare = value;

            switch (style)
            {
                case ParamStyle.Undefined:
                    break;
                case ParamStyle.General:
                    if (valuePrepare is string)
                    {
                        if (valuePrepare.Equals(string.Empty))
                        {
                            valuePrepare = null;
                        }
                    }
                    break;
                case ParamStyle.LikeBeginWith:
                    valuePrepare = this.PrepareSearchParam(valuePrepare) + "%";
                    break;
                case ParamStyle.LikeEndWith:
                    valuePrepare = "%" + this.PrepareSearchParam(valuePrepare);
                    break;
                case ParamStyle.LikeContain:
                    valuePrepare = "%" + this.PrepareSearchParam(valuePrepare) + "%";
                    break;
                case ParamStyle.LikeCustom:
                    valuePrepare = this.PrepareSearchParam(valuePrepare).Replace("*", "%");
                    break;
                default:
                    break;
            }

            if (valuePrepare == null)
            {
                return null;
            }

            return  valuePrepare;
        }

        public string PrepareSearchParam(object param, ParamStyle style = ParamStyle.General)
        {

            string paramResult = string.Empty;

            //Validate Param
            if (param != null)
            {
                paramResult = param.ToString();
            }

            //Wildcard
            paramResult = paramResult.Replace("%", "[%]").Replace("_", "[_]");

            return paramResult;
        }

        public void ExecSQLNoReturn(string _sql)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(_sql, connection))
                {
                    if (connection.State == ConnectionState.Closed) 
                        connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Aood
        public static class CaseStatus
        {
            public static string Open = "Open";
            public static string Closed = "Closed";
            public static string Cancel = "Cancel";
            public static string Pending = "Pending";
        }
        #endregion
    }
}