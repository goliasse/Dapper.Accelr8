

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
    public class ServerReader : EntityReader<int, Server>
    {
        public ServerReader(
            TableInfo tableInfo
            , string connectionStringName
            , DapperExecuter executer
            , QueryBuilder queryBuilder
            , JoinBuilder joinBuilder
            , IServiceLocatorMarker serviceLocator) 
            : base(tableInfo, connectionStringName, executer, queryBuilder, joinBuilder, serviceLocator)
        { }

		//Child Count 1
		//Parent Count 0

		/// <summary>
		/// Sets the children of type Endpoint on the parent on Endpoints.
		/// From foriegn key FK_Endpoints_Servers
		/// </summary>
		/// <param name="results"></param>
		/// <param name="children"></param>
		protected void SetChildrenEndpoints(IList<Server> results, IList<object> children)
		{
			//Child Id Type: int
			//Child Type: Endpoint

			if (results == null || results.Count < 1 || children == null || children.Count < 1)
				return;

			var typedChildren = children.OfType<Endpoint>();

			foreach (var r in results)
			{
				r.Endpoints = typedChildren.Where(b => b.EndpointId == r.Id).ToList();
				r.Endpoints.ToList().ForEach(b => b.Server = r);
			}
		}

			/// <summary>
		/// Loads the table Servers into class Server
		/// </summary>
		/// <param name="results">Server</param>
		/// <param name="row"></param>
        public override Server LoadEntity(dynamic row)
        {
            var dataRow = (IDictionary<string, object>)row;
            var domain = new Server();

			domain.Id = GetRowData<int>(dataRow, IdField);
			
			domain.Name = GetRowData<string>(dataRow, ServerColumnNames.Name.ToString()); 	
			domain.Direction = GetRowData<string>(dataRow, ServerColumnNames.Direction.ToString()); 	
			domain.Description = GetRowData<string>(dataRow, ServerColumnNames.Description.ToString()); 	
			domain.Protocol = GetRowData<string>(dataRow, ServerColumnNames.Protocol.ToString()); 	
			domain.Configuration = GetRowData<byte[]>(dataRow, ServerColumnNames.Configuration.ToString());             
			
			return domain;
        }

        public override IEntityReader<int, Server> WithAllChildrenForId(int id)
        {
			base.WithAllChildrenForId(id);

			
			WithChildForParentId(_locator.Resolve<IEntityReader<int , Endpoints>(), id, IdField, SetChildren);
			
            return this;
        }

        public override void SetAllChildrenForExisting(Server entity)
        {
			ClearAllQueries();

            if (entity == null)
                return;

			WithChildForParentId(_locator.Resolve<IEntityReader<int , Endpoints>(), id, IdField, SetChildrenEndpoints);
			QueryResultForChildrenOnly(new List<Server>() { entity });

			_locator.Resolve<IEntityReader<int , Endpoints>().SetAllChildrenForExisting(entity.Endpoints);
				
		}

		public override void SetAllChildrenForExisting(IList<Server> entities)
        {
			ClearAllQueries();

            if (entities == null || entities.Count < 1)
                return;

			WithChildForParentId(_locator.Resolve<IEntityReader<int , Endpoints>(), id, IdField, SetChildrenEndpoints);
					
			QueryResultForChildrenOnly(entities);

			_locator.Resolve<IEntityReader<int, Endpoints>().SetAllChildrenForExisting(entities.SelectMany(e => e.Endpoints));
					
		}
    }
}
		