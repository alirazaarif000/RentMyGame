using Microsoft.EntityFrameworkCore;
using RMG.DAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMG.DAL.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _context;
		internal DbSet<T> DbSet { get; set; }
		public Repository(ApplicationDbContext context)
		{
			_context = context;
			DbSet = _context.Set<T>();
		}
		public void Add(T entity)
		{
			DbSet.Add(entity);
		}

		public T Get(Expression<Func<T, bool>>? filter, string IncludeProperties = null)
		{
			IQueryable<T> query = DbSet.AsNoTracking();
			query = query.Where(filter);
			if (!String.IsNullOrEmpty(IncludeProperties))
			{
				foreach (var prop in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(prop);
				}
			}
			return query.FirstOrDefault();
		}

		public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string IncludeProperties = null)
		{
			IQueryable<T> query = DbSet.AsNoTracking();
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (!String.IsNullOrEmpty(IncludeProperties))
			{
				foreach (var prop in IncludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(prop);
				}
			}

			return query;
		}

		public void Remove(T entity)
		{
			DbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entity)
		{
			DbSet.RemoveRange(entity);
		}
	}
}
