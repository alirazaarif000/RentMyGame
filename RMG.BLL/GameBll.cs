using Microsoft.AspNetCore.Mvc.Rendering;
using RMG.DAL;
using RMG.DAL.Repository;
using RMG.DAL.Repository.IRepository;
using RMG.Models;
using RMG.Models.ViewModels;
using RMG.Utility;
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
        private readonly ApplicationUserBll _userBll;
        private readonly RentalBll _rentalBll;
        private readonly ReviewBll _reviewBll;
        public GameBll(IUnitOfWork uow, ApplicationUserBll userBll, RentalBll rentalBll, ReviewBll reviewBll)
        {
            _uow = uow;
            _userBll = userBll;
            _rentalBll = rentalBll;
            _reviewBll = reviewBll;
        }
        public Result<List<Game>> GetAllGame()
        {
            try
            {
                List<Game> games = _uow.Game.GetAll(IncludeProperties:"Genre,Platform").ToList();
                foreach (var game in games)
                {
                    Result<List<Review>> result = _reviewBll.GetAllReview(game.Id);

                    if (result.Status && result.Data != null)
                    {
                        List<Review> reviews = result.Data;

                        if (reviews.Count > 0)
                        {
                            game.RatingCount = reviews.Count;
                            int averageRating = (int)Math.Round(reviews.Average(r => r.Rating));
                            game.Ratings = averageRating;
                        }
                        else
                        {
                            game.RatingCount = 0;
                            game.Ratings = 0;
                        }
                    }
                    else
                    {
                        game.RatingCount = 0;
                        game.Ratings = 0;
                    }
                }
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
                Game game = _uow.Game.Get(u=> u.Id==id, IncludeProperties: "Genre,Platform");
                Result<List<Review>> result = _reviewBll.GetAllReview(id);
                if (result.Status && result.Data!=null)
                {
                    List<Review> reviews = result.Data;
                    if (reviews.Count > 0)
                    {
                        game.RatingCount = reviews.Count;
                        int averageRating = (int)Math.Round(reviews.Average(r => r.Rating));
                        game.Ratings=averageRating;
                    }
                }
                else
                {
                    game.RatingCount=0;
                    game.Ratings = 0;
                }
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
        
        public Result<GameVM> BindGameDropdowns()
        {
            try
            {
                GameVM game = new()
                {
                    PlatformList = _uow.Platform.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.PlatformName,
                        Value = c.Id.ToString()

                    }),
                    GenreList = _uow.Genre.GetAll().Select(c => new SelectListItem
                    {
                        Text = c.GenreName,
                        Value = c.Id.ToString()
                    }),
                    Game = new Game()
                };
                return new Result<GameVM>
                {
                    Status = true,
                    Data = game,
                    StatusCode = 200,
                };
            }
            catch (Exception ex) 
            {
                return new Result<GameVM> { Status = false, Message = ex.Message };
            }
        }

        public Result<object> RentGame(string id, int gameId)
        {
            try
            {
                var userResult = _userBll.GetApplicationUser(id);
                if (!userResult.Status)
                {
                    return new Result<object> { Status = false, Message = "User not found." };
                }
                ApplicationUser user = userResult.Data;

                if (user.SubscriptionId == null)
                {
                    return new Result<object> { Status = false, Message = "User does not have an active subscription." };
                }

                Result<List<Rental>> rentGamesResult = _rentalBll.GetAllRental(id);
                if (!rentGamesResult.Status)
                {
                    return new Result<object> { Status = false, Message = "Error retrieving rentals." };
                }
                List<Rental> rentGames = rentGamesResult.Data ?? new List<Rental>();
                List<Rental> activeRentGames = rentGames.Where(r => r.Status == SD.ActiveStatus).ToList();
				Subscription subscription = user.Subscription;
                if (subscription == null)
                {
                    return new Result<object> { Status = false, Message = "User subscription not found." };
                }

                var gameResult = GetGame(gameId);
                if (!gameResult.Status)
                {
                    return new Result<object> { Status = false, Message = "Game not found." };
                }
                Game game = gameResult.Data;
                if (game.Stock == 0)
                {
                    return new Result<object> { Status = false, Message = "Game out of Stock." };
                }

                DateOnly past3M = DateOnly.FromDateTime(DateTime.Now.AddMonths(-3));

                switch (subscription.PackageName)
                {
                    case SD.Basic:
                        if (activeRentGames.Count >= 2)
                        {
                            return new Result<object> { Status = false, Message = "You have reached the rental limit for Basic subscription." };
                        }
                        if (game.ReleaseDate > past3M)
                        {
                            return new Result<object> { Status = false, Message = "You cannot rent games released in the past three months with a Basic subscription." };
                        }
                        break;

                    case SD.Premium:
                        if (activeRentGames.Count >= 5)
                        {
                            return new Result<object> { Status = false, Message = "You have reached the rental limit for Premium subscription." };
                        }
                        var activeRecentGamesCount = activeRentGames
                            .Where(r => GetGame(r.GameId).Data?.ReleaseDate > past3M)
                            .Count();
                        if (activeRecentGamesCount >= 1)
                        {
                            return new Result<object> { Status = false, Message = "You cannot rent more than one game released in the past 3 months with a Premium subscription." };
                        }
                        break;

                    case SD.PremiumMax:
                        if (activeRentGames.Count >= 10)
                        {
                            return new Result<object> { Status = false, Message = "You have reached the rental limit for PremiumMax subscription." };
                        }
                        var activeRecentGamesMax = activeRentGames
                            .Where(r => GetGame(r.GameId).Data?.ReleaseDate > past3M)
                            .Count();
                        if (activeRecentGamesMax >= 2)
                        {
                            return new Result<object> { Status = false, Message = "You cannot rent more than two games released in the past 3 months with a PremiumMax subscription." };
                        }
                        break;

                    default:
                        return new Result<object> { Status = false, Message = "Invalid subscription package." };
                }

                // Logic to rent the game
                Rental newRental = new Rental
                {
                    ApplicationUserId = user.Id,
                    GameId = gameId,
                    RentalDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddMonths(1),
                    Status = "Active"
                };
                _rentalBll.AddRental(newRental);
                game.Stock = game.Stock-1;
                UpdateGame(game);

                return new Result<object> { Status = true, Message = "Game rented successfully.", Data = newRental };
            }
            catch (Exception ex)
            {
                return new Result<object> { Status = false, Message = ex.Message };
            }
        }


		public Result<object> ReturnGame(string id, int gameId)
		{
			try
			{
				var userResult = _userBll.GetApplicationUser(id);
               
				if (!userResult.Status)
				{
					return new Result<object> { Status = false, Message = "User not found." };
				}
				ApplicationUser user = userResult.Data;

				if (user.SubscriptionId == null)
				{
					return new Result<object> { Status = false, Message = "User does not have an active subscription." };
				}
				Result<List<Rental>> rentGamesResult = _rentalBll.GetAllRental(id);
				if (!rentGamesResult.Status)
				{
					return new Result<object> { Status = false, Message = "Error retrieving rentals." };
				}
                Rental returnGame= _rentalBll.GetRentalByUser(id, gameId, SD.ActiveStatus).Data;
				List<Rental> rentGames = rentGamesResult.Data ?? new List<Rental>();
				List<SubscriptionHistory> subscriptionHistories = _uow.SubscriptionHistory.GetAll(u => (u.ApplicationUserId == id && u.Status == SD.ActiveStatus)).ToList();
				List<Rental> allReturnedGames = rentGames.Where(r => r.Status == SD.ReturnedStatus &&
				subscriptionHistories.Any(sh => r.ReturnDate >= sh.StartDate && r.ReturnDate <= sh.EndDate)).ToList();

				Subscription subscription = user.Subscription;
				if (subscription == null)
				{
					return new Result<object> { Status = false, Message = "User subscription not found." };
				}

				var gameResult = GetGame(gameId);
				if (!gameResult.Status)
				{
					return new Result<object> { Status = false, Message = "Game not found." };
				}
				Game game = gameResult.Data;

				switch (subscription.PackageName)
				{
					case SD.Basic:
						return new Result<object> { Status = false, Message = "Replace or Return Game are not allowed in Basic subscription." };

					case SD.Premium:
                        if(allReturnedGames.Count >= 1)
                        {
							return new Result<object> { Status = false, Message = "Cannot Replace or Return more than 1 Games in Premium subscription." };
						}
						break;

					case SD.PremiumMax:
						if (allReturnedGames.Count >= 2)
						{
							return new Result<object> { Status = false, Message = "Cannot Replace or Return more than 2 Games in Premium subscription." };
						}

						break;

					default:
						return new Result<object> { Status = false, Message = "Invalid subscription package." };
				}

				// Logic to rent the game
				//Rental newRental = new Rental
				//{
    //                Id= 
				//	ApplicationUserId = user.Id,
				//	GameId = gameId,
				//	ReturnDate = DateTime.Now,
				//	Status = "Returned"
				//};

                returnGame.Status=SD.ReturnedStatus;
                returnGame.ReturnDate= DateTime.Now;
				_rentalBll.UpdateRental(returnGame);
                game.Stock += game.Stock;
                UpdateGame(game);
				return new Result<object> { Status = true, Message = "Game Returned successfully.", Data = returnGame };
			}
			catch (Exception ex)
			{
				return new Result<object> { Status = false, Message = ex.Message };
			}
		}



	}
}
