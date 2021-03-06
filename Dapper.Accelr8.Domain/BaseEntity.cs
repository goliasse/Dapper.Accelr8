﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Accelr8.Domain;

namespace Dapper.Accelr8.Repo.Domain
{
    public abstract class BaseEntity<IdType> : IEntity, IHaveId<IdType>
        where IdType : IComparable<IdType>
    {
        public BaseEntity()
        {
            IsDirty = true;
        }

        public bool IsDirty { get; set; }

        public IdType Id { get; set; }

        internal static int? TypeHashCode;
        public int GetTypeHashCode()
        {
            if (!TypeHashCode.HasValue)
                TypeHashCode = this.GetType().GetHashCode();

            return TypeHashCode.Value;
        }

        public bool IsNew()
        {
            return Id.CompareTo(default(IdType)) == 0;
        }
    }
}
