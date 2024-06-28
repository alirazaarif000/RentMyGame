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
    public class NotificationBll
    {
        private readonly IUnitOfWork _uow;
        public NotificationBll(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Result<List<Notification>> GetAllNotification()
        {
            try
            {
                List<Notification> notifications = _uow.Notification.GetAll(IncludeProperties:"ApplicationUser").ToList();
                return new Result<List<Notification>>
                {
                    Status = true,
                    Data = notifications,
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            { 
                return new Result<List<Notification>> { Status=false, Message = ex.Message };            
            }
        }
        public Result<Notification> GetNotification(int? id)
        {
            try
            {
                Notification notification = _uow.Notification.Get(u=> u.Id==id);
                return new Result<Notification>
                {
                    Status = true,
                    Data = notification,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<Notification> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddNotification(Notification notification)
        {
            try
            {
                _uow.Notification.Add(notification);
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
        public Result<object> UpdateNotification(Notification notification)
        {
            try
            {
                _uow.Notification.Update(notification);
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

        public Result<object> DeleteNotification(int id) 
        {
            try
            {
                var NotificationToBeDeleted = GetNotification(id).Data;
                if (NotificationToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting Notification" };
                }
                else
                {
                    _uow.Notification.Remove(NotificationToBeDeleted);
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
