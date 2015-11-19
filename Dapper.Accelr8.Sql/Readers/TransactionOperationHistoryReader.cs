

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
    public class TransactionOperationHistoryReader : EntityReader<int, TransactionOperationHistory>
    {
        public TransactionOperationHistoryReader(
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
		/// Sets the children of type TransactionOperationDetail on the parent on TransactionOperationDetails.
		/// From foriegn key FK_TransactionOperationDetails_History
		/// </summary>
		/// <param name="results"></param>
		/// <param name="children"></param>
		protected void SetChildrenTransactionOperationDetails(IList<TransactionOperationHistory> results, IList<object> children)
		{
			//Child Id Type: int
			//Child Type: TransactionOperationDetail

			if (results == null || results.Count < 1 || children == null || children.Count < 1)
				return;

			var typedChildren = children.OfType<TransactionOperationDetail>();

			foreach (var r in results)
			{
				r.TransactionOperationDetails = typedChildren.Where(b => b.TransactionOperationDetailsId == r.Id).ToList();
				r.TransactionOperationDetails.ToList().ForEach(b => b.TransactionOperationHistory = r);
			}
		}

			/// <summary>
		/// Loads the table TransactionOperationHistory into class TransactionOperationHistory
		/// </summary>
		/// <param name="results">TransactionOperationHistory</param>
		/// <param name="row"></param>
        public override TransactionOperationHistory LoadEntity(dynamic row)
        {
            var dataRow = (IDictionary<string, object>)row;
            var domain = new TransactionOperationHistory();

			domain.Id = GetRowData<int>(dataRow, IdField);
			
			domain.Name = GetRowData<string>(dataRow, TransactionOperationHistoryColumnNames.Name.ToString()); 	
			domain.Timestamp = GetRowData<DateTime?>(dataRow, TransactionOperationHistoryColumnNames.Timestamp.ToString()); 	
			domain.Username = GetRowData<string>(dataRow, TransactionOperationHistoryColumnNames.Username.ToString()); 	
			domain.TransactionCount = GetRowData<int?>(dataRow, TransactionOperationHistoryColumnNames.TransactionCount.ToString()); 	
			domain.TransactionSuccessCount = GetRowData<int?>(dataRow, TransactionOperationHistoryColumnNames.TransactionSuccessCount.ToString()); 	
			domain.Status = GetRowData<string>(dataRow, TransactionOperationHistoryColumnNames.Status.ToString()); 	
			domain.OperationType = GetRowData<string>(dataRow, TransactionOperationHistoryColumnNames.OperationType.ToString());             
			
			return domain;
        }

        public override IEntityReader<int, TransactionOperationHistory> WithAllChildrenForId(int id)
        {
			base.WithAllChildrenForId(id);

			
			WithChildForParentId(_locator.Resolve<IEntityReader<int , TransactionOperationDetails>(), id, IdField, SetChildren);
			
            return this;
        }

        public override void SetAllChildrenForExisting(TransactionOperationHistory entity)
        {
			ClearAllQueries();

            if (entity == null)
                return;

			WithChildForParentId(_locator.Resolve<IEntityReader<int , TransactionOperationDetails>(), id, IdField, SetChildrenTransactionOperationDetails);
			QueryResultForChildrenOnly(new List<TransactionOperationHistory>() { entity });

			_locator.Resolve<IEntityReader<int , TransactionOperationDetails>().SetAllChildrenForExisting(entity.TransactionOperationDetails);
				
		}

		public override void SetAllChildrenForExisting(IList<TransactionOperationHistory> entities)
        {
			ClearAllQueries();

            if (entities == null || entities.Count < 1)
                return;

			WithChildForParentId(_locator.Resolve<IEntityReader<int , TransactionOperationDetails>(), id, IdField, SetChildrenTransactionOperationDetails);
					
			QueryResultForChildrenOnly(entities);

			_locator.Resolve<IEntityReader<int, TransactionOperationDetails>().SetAllChildrenForExisting(entities.SelectMany(e => e.TransactionOperationDetails));
					
		}
    }
}
		