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
    public class ReviewBll
    {
        private readonly IUnitOfWork _uow;
        public ReviewBll(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Result<List<Review>> GetAllReview(int? GameId=null)
        {
            try
            {
                List<Review> reviews = _uow.Review.GetAll().ToList();
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
                List<Review> reviews = _uow.Review.GetAll(r=>r.IsApproved==false, IncludeProperties: "ApplicationUser,Game").ToList();
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
                review.IsApproved = true;
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
				return new Result<object> { Status = true, Data = null, StatusCode = 200 };
			}
		}
	}
}
