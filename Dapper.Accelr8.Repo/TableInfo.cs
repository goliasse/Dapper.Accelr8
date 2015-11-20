﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Accelr8.Repo.Contracts.Readers;
using Dapper.Accelr8.Repo.Contracts.Writers;

namespace Dapper.Accelr8.Repo
{
    public class TableInfo
    {
        protected static object _syncRoot = new object();
        protected static Dictionary<string, object> _readers = new Dictionary<string, object>();
        protected static bool _cacheReaders = true;
        static protected IServiceLocatorMarker _locator;

        public TableInfo() : this(null)
        {

        }

        public TableInfo(IServiceLocatorMarker locator)
        {
            ColumnNames = new string[0];
            Joins = new JoinInfo[0];

            if (_locator == null)
                _locator = locator;
        }

        protected IEntityReader GetReader(Type idType, Type entityType)
        {
            if (_cacheReaders)
            {
                var key = idType + "." + entityType;
                lock (_syncRoot)
                    if (!_readers.ContainsKey(key))
                        _readers.Add(key, _locator.Resolve(typeof(IEntityReader<,>).MakeGenericType(idType, entityType)));

                return _readers[key] as IEntityReader;
            }

            return _locator.Resolve(typeof(IEntityReader<,>).MakeGenericType(idType, entityType)) as IEntityReader;
        }

        public TableInfo(bool uniqueId, string idColumn, string tableName, string tableAlias, string[] columnNames)
        {
            UniqueId = uniqueId;
            IdColumn = idColumn;
            TableName = tableName;
            TableAlias = tableAlias;
            ColumnNames = columnNames;
        }

        public bool UniqueId { get; set; }
        public string IdColumn { get; set; }
        public string TableName { get; set; }
        public string TableAlias { get; set; }
        public string[] ColumnNames { get; set; }
        public JoinInfo[] Joins { get; set; }
    }
}
