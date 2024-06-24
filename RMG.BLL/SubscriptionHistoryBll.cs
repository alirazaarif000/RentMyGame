using RMG.DAL;
using RMG.DAL.Repository;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.BLL
{
    public class SubscriptionHistoryBll
    {
        private readonly IUnitOfWork _uow;
        public SubscriptionHistoryBll(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Result<List<SubscriptionHistory>> GetAllSubscriptionHistory()
        {
            try
            {
                List<SubscriptionHistory> subscriptionHistorys = _uow.SubscriptionHistory.GetAll().ToList();
                return new Result<List<SubscriptionHistory>>
                {
                    Status = true,
                    Data = subscriptionHistorys,
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            { 
                return new Result<List<SubscriptionHistory>> { Status=false, Message = ex.Message };            
            }
        }
        public Result<SubscriptionHistory> GetSubscriptionHistory(int? id)
        {
            try
            {
                SubscriptionHistory subscriptionHistory = _uow.SubscriptionHistory.Get(u=> u.Id==id);
                return new Result<SubscriptionHistory>
                {
                    Status = true,
                    Data = subscriptionHistory,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<SubscriptionHistory> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddSubscriptionHistory(SubscriptionHistory subscriptionHistory)
        {
            try
            {
                _uow.SubscriptionHistory.Add(subscriptionHistory);
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
        public Result<object> UpdateSubscriptionHistory(SubscriptionHistory subscriptionHistory)
        {
            try
            {
                _uow.SubscriptionHistory.Update(subscriptionHistory);
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
                return new Result<object> { Status = true, Data=null, StatusCode=200 };
            }
        }

        public Result<object> DeleteSubscriptionHistory(int id) 
        {
            try
            {
                var SubscriptionHistoryToBeDeleted = GetSubscriptionHistory(id).Data;
                if (SubscriptionHistoryToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting SubscriptionHistory" };
                }
                else
                {
                    _uow.SubscriptionHistory.Remove(SubscriptionHistoryToBeDeleted);
                    _uow.Save();
                    return new Result<object> { Status = true, Data = null, StatusCode = 200, Message="Deleted Successfully" };
                }

            }
            catch(Exception ex) 
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
    }
}
