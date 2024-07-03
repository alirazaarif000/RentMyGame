using Microsoft.EntityFrameworkCore;
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
    public class RentalBll
    {
        private readonly IUnitOfWork _uow;
        public RentalBll(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Result<List<Rental>> GetAllRental(string id, string? status=null)
        {
            try
            {
                List<Rental> rentals = _uow.Rental.GetAll(u=>u.ApplicationUserId==id, IncludeProperties:"Game" ).ToList();
                if (status != null) {
                rentals= rentals.Where(r=>r.Status == status).ToList();
                }
                return new Result<List<Rental>>
                {
                    Status = true,
                    Data = rentals,
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            { 
                return new Result<List<Rental>> { Status=false, Data=null, Message = ex.Message };            
            }
        }
        public Result<Rental> GetRental(int? id)
        {
            try
            {
                Rental rental = _uow.Rental.Get(u=> u.Id==id);
                return new Result<Rental>
                {
                    Status = true,
                    Data = rental,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<Rental> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddRental(Rental rental)
        {
            try
            {
                _uow.Rental.Add(rental);
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
        public Result<object> UpdateRental(Rental rental)
        {
            try
            {
                _uow.Rental.Update(rental);
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

        public Result<object> DeleteRental(int id) 
        {
            try
            {
                var RentalToBeDeleted = GetRental(id).Data;
                if (RentalToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting Rental" };
                }
                else
                {
                    _uow.Rental.Remove(RentalToBeDeleted);
                    _uow.Save();
                    return new Result<object> { Status = true, Data = null, StatusCode = 200, Message="Deleted Successfully" };
                }

            }
            catch(Exception ex) 
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }

        public Result<Rental> GetRentalByUser(string userId, int gameId, string Status)
        {
            try
            {
                var rental =  _uow.Rental.Get(r => r.ApplicationUserId == userId && r.GameId == gameId && r.Status == Status);

                if (rental != null)
                {
                    return new Result<Rental> { Status = true, Data = rental };
                }

                return new Result<Rental> { Status = false, Data= new Rental(), Message = "Rental not found." };
            }
            catch (Exception ex)
            {
                return new Result<Rental> { Status = false, Message = ex.Message };
            }
        }

    }
}
