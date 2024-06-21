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
    public class GameBll
    {
        private readonly IUnitOfWork _uow;
        public GameBll(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public Result<List<Game>> GetAllGame()
        {
            try
            {
                List<Game> games = _uow.Game.GetAll().ToList();
                return new Result<List<Game>>
                {
                    Status = true,
                    Data = games,
                    StatusCode = 200
                };
            }
            catch (Exception ex) 
            { 
                return new Result<List<Game>> { Status=false, Message = ex.Message };            
            }
        }
        public Result<Game> GetGame(int? id)
        {
            try
            {
                Game game = _uow.Game.Get(u=> u.Id==id);
                return new Result<Game>
                {
                    Status = true,
                    Data = game,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<Game> { Status = false, Message = ex.Message };
            }
        }
        public Result<object> AddGame(Game game)
        {
            try
            {
                _uow.Game.Add(game);
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
        public Result<object> UpdateGame(Game game)
        {
            try
            {
                _uow.Game.Update(game);
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

        public Result<object> DeleteGame(int id) 
        {
            try
            {
                var GameToBeDeleted = GetGame(id).Data;
                if (GameToBeDeleted == null)
                {
                    return new Result<object> { Status = false, Message = "Error Deleting Game" };
                }
                else
                {
                    _uow.Game.Remove(GameToBeDeleted);
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
