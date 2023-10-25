using ProjectPerevozki.DataBase;
using ProjectPerevozki.Models;
using System.Collections.Generic;

namespace ProjectPerevozki.Tools
{
    public class TripTools
    {
        private TripSql tripSql;
        public TripTools()
        {
            tripSql = TripSql.GetInstanse();
        }

        public void AddFullContract(Contract contract, List<Trip> trips, List<List<TripDriverList>> tripDriverLists, List<List<TripCargoList>> tripCargoLists)
        {
            ContractSql contractSql = new ContractSql();
            contractSql.WriteToDb(contract);

            for (int i = 0; i < trips.Count; i++)
            {
                tripSql.WriteTripToDb(trips[0]);
                tripSql.WriteCargoListToDb(tripCargoLists[0]);
                tripSql.WriteDriverListToDb(tripDriverLists[0]);
            }
        }

        public string checkData(TripCargoList tripCargoList)
        {
            string status;
            if (tripCargoList.CargoType == null || tripCargoList.Amount < 1 || tripCargoList.Weight < 1
                || tripCargoList.Insurance < 1 || tripCargoList.Name == "")
                status = "Поля заполнены некорректно";
            else
                status = "good";

            return status;
        }
    }
}
