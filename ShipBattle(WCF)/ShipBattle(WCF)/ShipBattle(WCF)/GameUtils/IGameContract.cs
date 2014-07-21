using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace GameUtils
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IGameContractCallback))]
    //[ServiceKnownType(typeof())]
    public interface IGameContract
    {
        [OperationContract]
        object GetPlayerUid();
        [OperationContract]
        void SetToList(string uid);
        [OperationContract]
        void InitGame(string uid, Coordinates gameOptions, Coordinates gridOptions, Coordinates deltaCoord, int shipsCount);
        [OperationContract]
        void ShipInit(string shipType, string shipTurning, string uid);
        [OperationContract]
        void RotateShip();

        [OperationContract]
        List<Coordinates> GetShipImage();

        [OperationContract]
        List<Coordinates> CreateShip(Coordinates shipCoord);

        [OperationContract]
        List<Coordinates> DeleteShip(Coordinates shipCoord);

        [OperationContract]
        List<Coordinates> HitTheShip(Coordinates hitCoord, string uid, out bool hit, out bool isDestroy);

        [OperationContract(IsOneWay = true)]
        void ReadyGo(string uid);

        [OperationContract(IsOneWay = true)]
        void UpdateContext(ShipAction action);
    }

    public interface IGameContractCallback
    {
        [OperationContract(IsOneWay = true)]
        void HitTheShipCallBack(Coordinates coord, bool hit);

        [OperationContract(IsOneWay = true)]
        void DestroyTheShipCallBack(List<Coordinates> coord);

        [OperationContract(IsOneWay = true)]
        void ReadyGoCallBack();

        [OperationContract(IsOneWay = true)]
        void UpdateContext(GameContext context, ShipAction action);
    }

} 
