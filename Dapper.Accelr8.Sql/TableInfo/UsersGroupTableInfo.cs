
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Accelr8.Domain;
using Dapper.Accelr8.TableInfos;
using Dapper;
using Dapper.Accelr8.Sql;
using Dapper.Accelr8.Repo;
using Dapper.Accelr8.Repo.Parameters;
using Dapper.Accelr8.Repo.Contracts.Readers;

namespace Dapper.Accelr8.TableInfos
{
	public enum UsersGroupColumnNames
	{	
		UsersGroupsId, 	
		UserId, 	
		GroupId, 	
	}

	public enum UsersGroupCascadeNames
	{	
		
		group, 	
		user, 	
	}

	public class UsersGroupTableInfo : Dapper.Accelr8.Sql.TableInfo
	{
		public UsersGroupTableInfo(IServiceLocatorMarker locator) : base(locator)
		{
			UniqueId = true;
			IdColumn = UsersGroupColumnNames.UsersGroupsId.ToString();
			TableName = "UsersGroups";
			TableAlias = "usersgroup";
			ColumnNames = Enum.GetNames(typeof(UsersGroupColumnNames));

			Joins = new JoinInfo[] {
						//For Key FK_UsersGroups_Groups
			new JoinInfo() {
			Reader = new Func<IEntityReader>(() => GetReader(typeof(int), typeof(Group))),
			TableName = "Groups",
			Alias = TableAlias + "_" + "group",
			Load = (entity, row) =>
				{ 
					var reader = GetReader(typeof(int), typeof(Group));
					var st = (entity as UsersGroup);

					if (st == null || row == null)
						return st;

					if (row.Id == default(int))
						return st;

					st.Group = reader.LoadEntityObject(row);

					return st;
				},
			JoinQuery = new JoinQueryElement[]
			{
				new JoinQueryElement() 
				{ 
					JoinField = GroupColumnNames.GroupId.ToString(),
					Operator = Operator.Equals,
					ParentField = UsersGroupColumnNames.UsersGroupsId.ToString(),
					ParentTableAlias = TableAlias
				}
			} },
						//For Key FK_UsersGroups_Users
			new JoinInfo() {
			Reader = new Func<IEntityReader>(() => GetReader(typeof(int), typeof(User))),
			TableName = "Users",
			Alias = TableAlias + "_" + "user",
			Load = (entity, row) =>
				{ 
					var reader = GetReader(typeof(int), typeof(User));
					var st = (entity as UsersGroup);

					if (st == null || row == null)
						return st;

					if (row.Id == default(int))
						return st;

					st.User = reader.LoadEntityObject(row);

					return st;
				},
			JoinQuery = new JoinQueryElement[]
			{
				new JoinQueryElement() 
				{ 
					JoinField = UserColumnNames.UserId.ToString(),
					Operator = Operator.Equals,
					ParentField = UsersGroupColumnNames.UsersGroupsId.ToString(),
					ParentTableAlias = TableAlias
				}
			} },
						};
		}
	}
}
		