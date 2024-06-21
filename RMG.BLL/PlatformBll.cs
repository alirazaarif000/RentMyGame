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
    public class PlatformBll
    {
        private readonly IUnitOfWork _uow;
        public PlatformBll(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Result<List<Platform>> GetAllPlatform()
        {
            try
            {
                List<Platform> platforms = _uow.Platform.GetAll().ToList();
                return new Result<List<Platform>>
                {
                    Status = true,
                    Data = platforms,
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            { 
                return new Result<List<Platform>> { Status=false, Message = ex.Message };            
            }
        }
        public Result<Platform> GetPlatform(int? id)
        {
            try
            {
                Platform platform = _uow.Platform.Get(u=> u.Id==id);
                return new Result<Platform>
                {
                    Status = true,
                    Data = platform,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<Platform> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddPlatform(Platform platform)
        {
            try
            {
                _uow.Platform.Add(platform);
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
        public Result<object> UpdatePlatform(Platform platform)
        {
            try
            {
                _uow.Platform.Update(platform);
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

        public Result<object> DeletePlatform(int id) 
        {
            try
            {
                var PlatformToBeDeleted = GetPlatform(id).Data;
                if (PlatformToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting Platform" };
                }
                else
                {
                    _uow.Platform.Remove(PlatformToBeDeleted);
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
