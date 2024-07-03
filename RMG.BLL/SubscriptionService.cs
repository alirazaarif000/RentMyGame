using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RMG.DAL;
using RMG.Models;
using RMG.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.BLL
{
    public class SubscriptionService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SubscriptionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateSubscriptionData(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

        }

        private async Task UpdateSubscriptionData(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                List<SubscriptionHistory> subscriptions = context.SubscriptionsHistory.Where(s => s.StartDate <= DateTime.Now.AddMonths(-1) && s.Status == SD.ActiveSubs).ToList();
                foreach (var subscription in subscriptions)
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        //Cancellation requested
                        break;
                    }
                    if (subscription.NoOfMonths > 1)
                    {
                        subscription.Status = SD.RenewedSubs;

                        SubscriptionHistory history = new()
                        {
                            ApplicationUserId = subscription.ApplicationUserId,
                            SubscriptionId = subscription.SubscriptionId,
                            SubscribedDate = subscription.SubscribedDate,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddMonths(1),
                            NoOfMonths = subscription.NoOfMonths,
                            RemainingMonths = subscription.RemainingMonths - 1,
                            PricePaid = subscription.PricePaid,
                            Status = SD.ActiveStatus
                        };
                        context.Add(history);
                    }
                    else
                    {
                        subscription.Status = SD.ExpiredSubs;
                    }
                     context.Update(subscription);

                    await context.SaveChangesAsync(stoppingToken);

                    List<Rental> ActiveRents = context.Rentals
                    .Where(r => r.ApplicationUserId == subscription.ApplicationUserId
                                && r.Status == SD.ActiveStatus
                                && r.RentalDate >= subscription.StartDate
                                && r.ReturnDate <= subscription.EndDate)
                    .ToList();

                    SubscriptionHistory Currentsubscription = context.SubscriptionsHistory.Where(s => s.Status == SD.ActiveSubs).FirstOrDefault();
                    foreach (var rental in ActiveRents)
                    {
                        Rental rent = new()
                        {
                            ApplicationUserId = rental.ApplicationUserId,
                            GameId = rental.GameId,
                            RentalDate = Currentsubscription?.StartDate?? DateTime.Now,
                            ReturnDate = Currentsubscription?.EndDate ?? DateTime.Now.AddMonths(1),
                            Status = SD.ActiveStatus,

                        };
                        context.Add(rent);
                    }

                        await context.SaveChangesAsync(stoppingToken);
                }

            }
        }
    }
}
