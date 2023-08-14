# ActuarialApplications

### Description
ASP.NET MVC application for actuarial applications.




## Setup

Databases and API from [github](https://github.com/holmen1/airflow-pipelines)


## Dependencies
https://github.com/holmen1/airflow-pipelines


## TODO
[x] Database  
[x] RFR  
[ ] ESG  
[x] Projections Life    
[ ] Projections P&C

     

    OverflowException: Numeric value does not fit in a System.Decimal
        Npgsql.Internal.TypeHandlers.NumericHandlers.NumericHandler.Read(NpgsqlReadBuffer buf, int len, bool async, FieldDescription fieldDescription)
        Npgsql.Internal.TypeHandling.NpgsqlTypeHandler.Read<TAny>(NpgsqlReadBuffer buf, int len, FieldDescription fieldDescription)
        Npgsql.NpgsqlDataReader.GetFieldValue<T>(int ordinal)
        lambda_method119(Closure , QueryContext , DbDataReader , ResultContext , SingleQueryResultCoordinator )
        Microsoft.EntityFrameworkCore.Query.Internal.SingleQueryingEnumerable<T>+AsyncEnumerator.MoveNextAsync()
        Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken)
        Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken)
        ActuarialApplications.Controllers.LifeController.GetCashFlowsAsync(Contract contract) in LifeController.cs

            var cashFlows = await _context.CashFlows.Where(c => c.ContractNo == contract.ContractNo).OrderBy(c => c.Month)

ActuarialApplications.Controllers.LifeController.Index(int selectedContractNo) in LifeController.cs

            var cf = await GetCashFlowsAsync(selectedContract);

