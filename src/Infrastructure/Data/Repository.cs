using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> : SpecificationEvaluatorBase<T> where T : class
    {
    }

    public class Repository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        private readonly ISpecificationEvaluator<T> _specificationEvaluator;

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _specificationEvaluator = new SpecificationEvaluator<T>();
        }

        public async Task<T> GetByIdAsync(int id) => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> ListAllAsync() => await _dbContext.Set<T>().ToListAsync();

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            var specificationResult = await ApplySpecification(spec);
            return await specificationResult.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var specificationResult = await ApplySpecification(spec);
            return await specificationResult.CountAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> FirstAsync(ISpecification<T> spec)
        {
            var specificationResult = await ApplySpecification(spec);
            return await specificationResult.FirstAsync();
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            var specificationResult = await ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync();
        }

        protected async Task<IQueryable<T>> ApplySpecification(ISpecification<T> spec)
        {
            return await Task.Run(() =>
            {
                return _specificationEvaluator.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
            });
        }
    }
}
