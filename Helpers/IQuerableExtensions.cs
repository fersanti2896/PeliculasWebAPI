using SPeliculasAPI.DTOs;

namespace SPeliculasAPI.Helpers {
    public static class IQuerableExtensions {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> query, PaginacionDTO paginacionDTO) {
            return query.Skip((paginacionDTO.Pagina - 1) * paginacionDTO.Elementos)
                        .Take(paginacionDTO.Elementos);
        }
    }
}
