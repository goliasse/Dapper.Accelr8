

using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Accelr8.Domain;
using Dapper.Accelr8.TableInfo;
using Dapper;
using Dapper.Accelr8.Sql;
using Dapper.Accelr8.Repo;
using Dapper.Accelr8.Repo.Parameters;
using Dapper.Accelr8.Repo.Contracts.Readers;

namespace Dapper.Accelr8.Readers
{
    public class AdminSettingReader : EntityReader<int, AdminSetting>
    {
        public AdminSettingReader(
            TableInfo tableInfo
            , string connectionStringName
            , DapperExecuter executer
            , QueryBuilder queryBuilder
            , JoinBuilder joinBuilder
            , IServiceLocatorMarker serviceLocator) 
            : base(tableInfo, connectionStringName, executer, queryBuilder, joinBuilder, serviceLocator)
        { }

		//Child Count 0
		//Parent Count 0

			/// <summary>
		/// Loads the table AdminSettings into class AdminSetting
		/// </summary>
		/// <param name="results">AdminSetting</param>
		/// <param name="row"></param>
        public override AdminSetting LoadEntity(dynamic row)
        {
            var dataRow = (IDictionary<string, object>)row;
            var domain = new AdminSetting();

			domain.Id = GetRowData<int>(dataRow, IdField);
			
			domain.Name = GetRowData<string>(dataRow, AdminSettingColumnNames.Name.ToString()); 	
			domain.Type = GetRowData<string>(dataRow, AdminSettingColumnNames.Type.ToString()); 	
			domain.Description = GetRowData<string>(dataRow, AdminSettingColumnNames.Description.ToString()); 	
			domain.Value = GetRowData<string>(dataRow, AdminSettingColumnNames.Value.ToString());             
			
			return domain;
        }

        public override IEntityReader<int, AdminSetting> WithAllChildrenForId(int id)
        {
			base.WithAllChildrenForId(id);

			
            return this;
        }

        public override void SetAllChildrenForExisting(AdminSetting entity)
        {
				
		}

		public override void SetAllChildrenForExisting(IList<AdminSetting> entities)
        {
					
		}
    }
}
		