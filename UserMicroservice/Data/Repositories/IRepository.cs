﻿namespace UserMicroservice.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> UpdateRefreshTokenAsync(T entity);
        Task<bool> Delete(int id);
        Task<T> getByEmail(string email);
    }

}
