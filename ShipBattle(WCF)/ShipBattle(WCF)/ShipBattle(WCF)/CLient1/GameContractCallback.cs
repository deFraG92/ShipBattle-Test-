using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using GameUtils;

namespace ClientsPart
{
    public class GameContractCallback : IGameContractCallback
    {
        private IGame _game;
        public GameContractCallback(IGame game)
        {
            _game = game;
        }

        public void HitTheShipCallBack(Coordinates coord, bool hit)
        {
            //_game.DrawHitShip(coord, hit);
            _game.AnimatedShooting(_game.GetRandomCoords(), coord, true);
        }

        public void DestroyTheShipCallBack(List<Coordinates> coord)
        {
            _game.DrawDestroyShip(coord);
        }

        public void ReadyGoCallBack()
        {
            _game.SetReady();
        }

        public void UpdateContext(GameContext context, ShipAction action)
        {
            var gameContext = _game.GetGameContext();
            switch (action)
            {
                case ShipAction.CreateShip:
                    {
                        foreach (var coords in context.ShipCoords)
                        {
                            gameContext.UpdateOnShipCoords(coords, ShipAction.CreateShip);
                        }
                        break;
                    }
                    
                case ShipAction.DeleteShip:
                    {
                        foreach (var coords in context.DeleteShipCoords)
                        {
                            gameContext.UpdateOnShipCoords(coords, ShipAction.DeleteShip);
                        }
                        break;
                    }
                case ShipAction.HitTheShip:
                    {
                        foreach (var coords in context.EnemyHitCoords)
                        {
                            gameContext.UpdateOnHitShipCoords(coords.Value[coords.Value.Count - 1], coords.Key);
                        }
                        break;
                    }
                case ShipAction.DestroyShip:
                    {
                        foreach (var coords in context.EnemyDestroyShipCoords)
                        {
                            gameContext.UpdateOnShipCoords(coords, ShipAction.DestroyShip);
                        }
                        break;
                    }

            }
        }
    }
}
