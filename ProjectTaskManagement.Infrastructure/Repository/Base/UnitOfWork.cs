using ProjectTaskManagement.Application.Common.Interfaces;
using ProjectTaskManagement.Domain.Entities;
using ProjectTaskManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectTaskManagement.Infrastructure.Repository.Base;

public class UnitOfWork : IunitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        UsersRepository = new BaseRepository<User>(context);
        ProjectsRepository = new BaseRepository<Project>(context);
        TasksRepository = new BaseRepository<ProjectTask>(context);
    }

    public IBaseRepository<User> UsersRepository { get; }
    public IBaseRepository<Project> ProjectsRepository { get; }
    public IBaseRepository<ProjectTask> TasksRepository { get; }

    public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();

    public Task<IDbContextTransaction> BeginTransactionAsync() =>
        _context.Database.BeginTransactionAsync();

    public void CommitTransaction() => _context.Database.CommitTransaction();

    public Task CommitTransactionAsync() => _context.Database.CommitTransactionAsync();

    public void RollbackTransaction() => _context.Database.RollbackTransaction();

    public Task RollbackTransactionAsync() => _context.Database.RollbackTransactionAsync();

    public int SaveChanges() => _context.SaveChanges();

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
