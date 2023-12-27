using OnlineStore.Domain.Helpers;
using System.Linq.Expressions;
namespace OnlineStore.DataAccess.Repositories
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAll();
		Task<IEnumerable<T>> Get(
			List<Expression<Func<T, bool>>> filters = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Expression<Func<T, object>>[] includeProperties = null);
		Task<PaginatedDataViewModel<T>> GetPaginatedData(
			List<Expression<Func<T, bool>>> filters = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			Expression<Func<T, object>>[] includeProperties = null,
			int pageNumber = 0, int pageSize = 5);
		Task<T> GetById<Tid>(Tid id);
		Task<bool> IsExists<Tvalue>(string key, Tvalue value);
		Task<bool> IsExistsForUpdate<Tid>(Tid id, string key, string value);
		Task<T> Create(T model);
		Task Update(T model);
		Task Delete(T model);
	}
}
