using EduFlow.Application.Interfaces.Repositories;
using EduFlow.Application.Interfaces.UnitOfWork;
using EduFlow.Domain.Entities;
using EduFlow.Infrastructure.Persistence.Context;
using EduFlow.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly EduDbContext _context;

    public IAccessCodeRepository AccessCodes { get; private set; }
    public ISessionRepository Sessions { get; private set; }
    public IBookingRepository Bookings { get; private set; }
    public IMaterialRepository Materials { get; private set; }
    public INotificationRepository Notifications { get; private set; }
    public IAuthRepository Auths { get; private set; }
    public IWaitingListRepository WaitingList { get; private set; }

    private IDbContextTransaction _transaction;

    public UnitOfWork(EduDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;

        AccessCodes = new AccessCodeRepository(_context);
        Sessions = new SessionRepository(_context);
        Bookings = new BookingRepository(_context);
        Materials = new MaterialRepository(_context);
        Notifications = new NotificationRepository(_context);
        Auths = new AuthRepository(userManager, context);
        WaitingList = new WaitingListRepository(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync() => await _transaction.RollbackAsync();

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}