using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.Models
{
    public interface IInflatable
    {
        void Inflate(IDataReader reader);
    }
}
