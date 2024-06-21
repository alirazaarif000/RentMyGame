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
    public class GenreBll
    {
        private readonly IUnitOfWork _uow;
        public GenreBll(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Result<List<Genre>> GetAllGenre()
        {
            try
            {
                List<Genre> genres = _uow.Genre.GetAll().ToList();
                return new Result<List<Genre>>
                {
                    Status = true,
                    Data = genres,
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            { 
                return new Result<List<Genre>> { Status=false, Message = ex.Message };            
            }
        }
        public Result<Genre> GetGenre(int? id)
        {
            try
            {
                Genre genre = _uow.Genre.Get(u=> u.Id==id);
                return new Result<Genre>
                {
                    Status = true,
                    Data = genre,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<Genre> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddGenre(Genre genre)
        {
            try
            {
                _uow.Genre.Add(genre);
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
        public Result<object> UpdateGenre(Genre genre)
        {
            try
            {
                _uow.Genre.Update(genre);
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

        public Result<object> DeleteGenre(int id) 
        {
            try
            {
                var GenreToBeDeleted = GetGenre(id).Data;
                if (GenreToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting Genre" };
                }
                else
                {
                    _uow.Genre.Remove(GenreToBeDeleted);
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
