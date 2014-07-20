using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GameUtils;
using System.Windows.Forms;

namespace ClientsPart
{
    public interface IGame
    {
        bool ConnectToService();
        object GetPlayerUid();
        void InitGame();
        void UpdateOptions(GameOptions options);
        void CreateShip(Coordinates shipCoord, string shipType, string shipTurning);
        void DeleteShip(Coordinates shipCoord);
        void HitTheShip(Coordinates shipCoord);
        void ReadyGo();
        bool GetReady();
        void SetReady();
        void SetAnimationControl(Control animationControl);

        //Drawing
        void SetGraphic(Graphics shipGraphics/*, Graphics hitShipGraphics*/);
        void DrawHitShip(Coordinates hitCoords, bool hit);
        void DrawDestroyShip(List<Coordinates> listOfDestroyShip);
        void ReDraw(Rectangle rectangle);
        //Context

        GameContext GetGameContext();

        //Animation

        void AnimatedShooting(Coordinates startCoord, Coordinates destination, bool reverse = false);
        Coordinates GetRandomCoords();
    }

}
