namespace Shared.Infra.Cqrs
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BaseQueryList<TResponse> : BaseCommand<TResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public int ItemsPerPage { get; set; } = 100;

        /// <summary>
        /// 
        /// </summary>
        public bool OrderedAsc { get; set; } = true;
    }
}
