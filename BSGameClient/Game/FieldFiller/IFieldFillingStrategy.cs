using BattleShipGameClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSGameClient.Game.FieldFiller
{
    internal interface IFieldFillingStrategy
    {
        public IEnumerable<ShipData> Generate();
    }
}
