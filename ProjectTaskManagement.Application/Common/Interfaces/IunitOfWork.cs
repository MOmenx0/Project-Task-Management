using ProjectTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace ProjectTaskManagement.Application.Common.Interfaces;

public interface IunitOfWork : IDisposable
{
    IBaseRepository<User> UsersRepository { get; }
    IBaseRepository<Project> ProjectsRepository { get; }
    IBaseRepository<ProjectTask> TasksRepository { get; }

    IDbContextTransaction BeginTransaction();
    Task<IDbContextTransaction> BeginTransactionAsync();
    void CommitTransaction();
    Task CommitTransactionAsync();
    void RollbackTransaction();
    Task RollbackTransactionAsync();
    int SaveChanges();
    Task<int> SaveChangesAsync();
}
