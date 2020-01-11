# efcore-parallel-query

ef core does not allow multiple query with same context instance.Therefore I have to use "new Context()" instance with every task query.
