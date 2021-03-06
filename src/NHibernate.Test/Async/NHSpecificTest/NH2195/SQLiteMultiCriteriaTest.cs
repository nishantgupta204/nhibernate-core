﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2195
{
	using System.Threading.Tasks;
	[TestFixture]
	public class SQLiteMultiCriteriaTestAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			base.OnSetUp();
			using (ISession session = this.OpenSession())
			{
				DomainClass entity = new DomainClass();
				entity.Id = 1;
				entity.StringData = "John Doe";
				entity.IntData = 1;
				session.Save(entity);

				entity = new DomainClass();
				entity.Id = 2;
				entity.StringData = "Jane Doe";
				entity.IntData = 2;
				session.Save(entity);
				session.Flush();
			}
		}

		protected override void OnTearDown()
		{
			base.OnTearDown();
			using (ISession session = this.OpenSession())
			{
				string hql = "from System.Object";
				session.Delete(hql);
				session.Flush();
			}
		}

		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect as SQLiteDialect != null;
		}

		[Test]
		public async Task SingleCriteriaQueriesWithIntsShouldExecuteCorrectlyAsync()
		{
			// Test querying IntData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Le("IntData",2));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());

				IList<DomainClass> list = await (criteriaWithPagination.ListAsync<DomainClass>());

				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task SingleCriteriaQueriesWithStringsShouldExecuteCorrectlyAsync()
		{
			// Test querying StringData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Like("StringData", "%Doe%"));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());

				IList<DomainClass> list = await (criteriaWithPagination.ListAsync<DomainClass>());

				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task MultiCriteriaQueriesWithIntsShouldExecuteCorrectlyAsync()
		{
			var driver = Sfi.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);

			// Test querying IntData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Le("IntData", 2));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());

				IMultiCriteria multiCriteria = session.CreateMultiCriteria();
				multiCriteria.Add(criteriaWithPagination);
				multiCriteria.Add(criteriaWithRowCount);

				IList results = await (multiCriteria.ListAsync());
				long numResults = (long)((IList)results[1])[0];
				IList list = (IList)results[0];

				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task MultiCriteriaQueriesWithStringsShouldExecuteCorrectlyAsync()
		{
			var driver = Sfi.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);

			// Test querying StringData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Like("StringData", "%Doe%"));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());

				IMultiCriteria multiCriteria = session.CreateMultiCriteria();
				multiCriteria.Add(criteriaWithPagination);
				multiCriteria.Add(criteriaWithRowCount);

				IList results = await (multiCriteria.ListAsync());

				long numResults = (long)((IList)results[1])[0];
				IList list = (IList)results[0];

				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
