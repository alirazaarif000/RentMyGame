using RMG.DAL;
using RMG.DAL.Repository;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMG.BLL
{
    public class ReviewBll
    {
        private readonly IUnitOfWork _uow;
        private readonly NotificationBll _notificationBll;
        public ReviewBll(IUnitOfWork uow, NotificationBll notificationBll)
        {
            _uow = uow;
            _notificationBll = notificationBll;
        }
        public Result<List<Review>> GetAllReview(int? GameId=null)
        {
            try
            {
                List<Review> reviews = _uow.Review.GetAll(r=>r.Status==SD.ApprovedReview).ToList();
                if (GameId != null)
                {
                    reviews= reviews.Where(u=>u.GameId == GameId).ToList();
                }
                return new Result<List<Review>>
                {
                    Status = true,
                    Data = reviews,
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            { 
                return new Result<List<Review>> { Status=false, Message = ex.Message };            
            }
        }
        public Result<Review> GetReview(int? id)
        {
            try
            {
                Review review = _uow.Review.Get(u=> u.Id==id);
                return new Result<Review>
                {
                    Status = true,
                    Data = review,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<Review> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddReview(Review review)
        {
            try
            {
                _uow.Review.Add(review);
                _uow.Save();
                var notification = new Notification
                {
                    ApplicationUserId = review.ApplicationUserId,
                    Message = review.Comment,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };
                _notificationBll.AddNotification(notification);
                return new Result<object>
                {
                    Status = true,
                    Data = null,
                    StatusCode = 200,
                    Message="Review Added Successfully"
                };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> UpdateReview(Review review)
        {
            try
            {
                _uow.Review.Update(review);
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

        public Result<object> DeleteReview(int id) 
        {
            try
            {
                var ReviewToBeDeleted = GetReview(id).Data;
                if (ReviewToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting Review" };
                }
                else
                {
                    _uow.Review.Remove(ReviewToBeDeleted);
                    _uow.Save();
                    return new Result<object> { Status = true, Data = null, StatusCode = 200, Message="Deleted Successfully" };
                }

            }
            catch(Exception ex) 
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }
        public Result<List<Review>> ReviewsForApproval()
        {
            try
            {
                List<Review> reviews = _uow.Review.GetAll(r=>r.Status==SD.ApprovalPendingReview, IncludeProperties: "ApplicationUser,Game").ToList();
                return new Result<List<Review>>
                {
                    Status = true,
                    Data = reviews,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<List<Review>> { Status = false, Message = ex.Message };
            }
        }
		public Result<object> ApproveReview(int id)
		{
			try
			{
                Review review= _uow.Review.Get(r => r.Id == id);
                review.Status = SD.ApprovedReview;
                review.IsApproved = true;
				_uow.Review.Update(review);
				_uow.Save();
				return new Result<object>
				{
					Status = true,
					Data = null,
					StatusCode = 200,
                    Message="Review Approved"
				};
			}
			catch (Exception ex)
			{
				return new Result<object> { Status = true, Data = null, StatusCode = 200 };
			}
		}
        public Result<object> RejectReview(int id)
        {
            try
            {
                Review review = _uow.Review.Get(r => r.Id == id);
                review.Status = SD.RejectedReview;
                review.IsApproved = false;
                _uow.Review.Update(review);
                _uow.Save();
                return new Result<object>
                {
                    Status = true,
                    Data = null,
                    StatusCode = 200,
                    Message = "Review Rejected"
                };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = true, Data = null, StatusCode = 200 };
            }
        }
    }
}
