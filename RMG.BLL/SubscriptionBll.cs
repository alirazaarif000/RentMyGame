using RMG.DAL;
using RMG.DAL.Repository;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMG.BLL
{
    public class SubscriptionBll
    {
        private readonly IUnitOfWork _uow;
        private readonly SubscriptionHistoryBll _subsHistoryBll;
        public SubscriptionBll(IUnitOfWork uow, SubscriptionHistoryBll subscriptionHistoryBll)
        {
            _uow = uow;
            _subsHistoryBll = subscriptionHistoryBll;
        }
        public Result<List<Subscription>> GetAllSubscription()
        {
            try
            {
                List<Subscription> subscriptions = _uow.Subscription.GetAll().ToList();
                return new Result<List<Subscription>>
                {
                    Status = true,
                    Data = subscriptions,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<List<Subscription>> { Status = false, Message = ex.Message };
            }
        }
        public Result<Subscription> GetSubscription(int? id)
        {
            try
            {
                Subscription subscription = _uow.Subscription.Get(u => u.Id == id);
                return new Result<Subscription>
                {
                    Status = true,
                    Data = subscription,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<Subscription> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddSubscription(Subscription subscription)
        {
            try
            {
                _uow.Subscription.Add(subscription);
                _uow.Save();
                return new Result<object>
                {
                    Status = true,
                    Data = null,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> UpdateSubscription(Subscription subscription)
        {
            try
            {
                _uow.Subscription.Update(subscription);
                _uow.Save();
                return new Result<object>
                {
                    Status = true,
                    Data = null,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = true, Data = null, StatusCode = 200 };
            }
        }

        public Result<object> DeleteSubscription(int id)
        {
            try
            {
                var SubscriptionToBeDeleted = GetSubscription(id).Data;
                if (SubscriptionToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting Subscription" };
                }
                else
                {
                    _uow.Subscription.Remove(SubscriptionToBeDeleted);
                    _uow.Save();
                    return new Result<object> { Status = true, Data = null, StatusCode = 200, Message = "Deleted Successfully" };
                }

            }
            catch (Exception ex)
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> BuySubscription(SubscribeDTO sub, string UserId)
        {
            try
            {
                ApplicationUser User = _uow.ApplicationUser.Get(u => u.Id == UserId);
                User.SubscriptionId = sub.SubcriptionId;
                SubscriptionHistory history = _subsHistoryBll.GetUserSubscription(UserId).Data;
                if (history != null)
                {
                    if(sub.SubcriptionId==history.SubscriptionId && history.RemainingMonths ==sub.NoOfMonths)
                    {
                        return new Result<object>
                        {
                            Status = false,
                            Message = $"You already have {history.RemainingMonths} month Subscription, Buy More than {history.RemainingMonths}."
                        };
                    }
                    if (sub.SubcriptionId < history.SubscriptionId)
                    {
                        List<Rental> rental = _uow.Rental.GetAll(u => u.ApplicationUserId == UserId && u.Status == SD.ActiveStatus).ToList();
                        if (rental.Count > 0)
                        {
                            return new Result<object>
                            {
                                Status = false,
                                Message = "Please Return the Games to Downgrade Subscription"
                            };
                        }
                    }
                    history.Status = SD.RenewedSubs;
                    _uow.SubscriptionHistory.Update(history);
                    _uow.Save();

                }
                SubscriptionHistory subscriptionHistory = new()
                {
                    ApplicationUserId = User.Id,
                    SubscriptionId = sub.SubcriptionId,
                    SubscribedDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(1),
                    RemainingMonths = sub.NoOfMonths,
                    NoOfMonths = sub.NoOfMonths,
                    PricePaid = sub.PricePaid,
                    Status = SD.ActiveStatus
                };
                _subsHistoryBll.AddSubscriptionHistory(subscriptionHistory);
                _uow.ApplicationUser.Update(User);
                _uow.Save();
                return new Result<object>
                {
                    Status = true,
                    Data = GetAllSubscription().Data,
                    Message = "Successfully Subscription Updated",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
    }
}
