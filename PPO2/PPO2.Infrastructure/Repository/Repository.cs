using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PPO2.Core.Interfaces;
using PPO2.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Infrastructure.Repository
{
    // шаблон, в якому все передбачено щоб працювати з БД. За допомогою нього не треба робити куча репозиторіїв для всіх модельок
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        internal DataContext _context;
        internal DbSet<TEntity> _dbSet;
        public Repository(DataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();    
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<TEntity?> GetByID(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task Insert(TEntity entity)
        {
            await _context.AddAsync(entity);
        }
        public async Task Delete(object id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                await Delete(entityToDelete);
            }
        }

        public Task Delete(TEntity entityToDelete)
        {
            return Task.Run(() =>
            {
                if (_context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
            });
        }
        public Task Update(TEntity entityToUpdate)
        {
            return Task.Run(() =>
            {
                _dbSet.Attach(entityToUpdate);
                _context.Entry(entityToUpdate).State = EntityState.Modified;
            });
        }
        public async Task<TEntity?> GetItemBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetListBySpec(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(_dbSet, specification);
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
