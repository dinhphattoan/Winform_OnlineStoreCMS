namespace OnlineStore.Domain.Mapper
{
	public interface IBaseMapper<TSource, TDestination>
	{
		TDestination MapModel(TSource source);
		IEnumerable<TDestination> MapList(IEnumerable<TSource> source);
		List<TDestination> MapEnumerable(IEnumerable<TSource> source);

	}
}
