using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Exceptions;
using OnlineStore.Domain.Helpers;
using System.Linq.Expressions;
namespace OnlineStore.DataAccess.Repositories
{
	public abstract class GenericRepository<T> : IRepository<T> where T : class
	{
		internal readonly ApplicationDbContext _dbContext;
		internal DbSet<T> DbSet => _dbContext.Set<T>();

		public GenericRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public virtual async Task<IEnumerable<T>> Get(
			List<Expression<Func<T, bool>>> filters = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Expression<Func<T, object>>[] includeProperties = null)
		{
			IQueryable<T> query = _dbContext.Set<T>();

			if (filters != null)
			{
				foreach (var filter in filters)
				{
					query = query.Where(filter);
				}
			}
			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
				{
					query = query.Include(includeProperty).AsNoTracking();
				}
			}


			if (orderBy != null)
			{
				return orderBy(query).ToList();
			}
			else
			{
				return query.ToList();
			}
		}
		public virtual async Task<PaginatedDataViewModel<T>> GetPaginatedData(
			List<Expression<Func<T, bool>>> filters = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Expression<Func<T, object>>[] includeProperties = null,
			int pageNumber = 0, int pageSize = 5)
		{
			IQueryable<T> query = _dbContext.Set<T>();
			if (filters != null && filters.Count != 0)
			{
				foreach (var filter in filters)
				{
					query = query.Where(filter);
				}
			}

			if (includeProperties != null)
			{
				foreach (var includeProperty in includeProperties)
				{
					query = query.Include(includeProperty);
				}
			}

			int totalCount = await query.CountAsync();

			if (orderBy != null)
			{
				var data = await orderBy(query).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
				return new PaginatedDataViewModel<T>(data, totalCount);
			}
			else
			{
				var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

				return new PaginatedDataViewModel<T>(data, totalCount);
			}
		}
		public virtual async Task<IEnumerable<T>> GetAll()
		{
			return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
		}

		public virtual async Task<T> GetById<Tid>(Tid id)
		{
			var data = await _dbContext.Set<T>().FindAsync(id);
			if (data == null)
				throw new NotFoundException(ErrorMessages.NOT_FOUND);
			return data;
		}

		public virtual async Task<bool> IsExists<Tvalue>(string key, Tvalue value)
		{
			var parameter = Expression.Parameter(typeof(T), "x");
			var property = Expression.Property(parameter, key);
			var constant = Expression.Constant(value);
			var equality = Expression.Equal(property, constant);
			var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

			return await _dbContext.Set<T>().AnyAsync(lambda);
		}

		//Before update existence check
		public virtual async Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value)
		{
			var parameter = Expression.Parameter(typeof(T), "x");
			var property = Expression.Property(parameter, key);
			var constant = Expression.Constant(value);
			var equality = Expression.Equal(property, constant);

			var idProperty = Expression.Property(parameter, "Id");
			var idEquality = Expression.NotEqual(idProperty, Expression.Constant(id));

			var combinedExpression = Expression.AndAlso(equality, idEquality);
			var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

			return await _dbContext.Set<T>().AnyAsync(lambda);
		}


		public virtual async Task<T> Create(T model)
		{
			await _dbContext.Set<T>().AddAsync(model);
			return model;
		}

		public virtual async Task Update(T model)
		{
			_dbContext.Attach(model);
			_dbContext.Entry(model).State = EntityState.Modified;
		}

		public virtual async Task Delete(T model)
		{
			if (_dbContext.Entry(model).State == EntityState.Detached)
			{
				_dbContext.Attach(model);
			}
			_dbContext.Set<T>().Remove(model);
		}

	}
}
